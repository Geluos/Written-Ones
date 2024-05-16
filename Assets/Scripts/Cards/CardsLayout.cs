using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class CardOrientation
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
}

public class YieldCollection : CustomYieldInstruction
{
    int _count;
    public IEnumerator CountCoroutine(IEnumerator coroutine)
    {
        _count++;
        yield return coroutine;
        _count--;
    }
    public override bool keepWaiting => _count != 0;
}

public class CardsLayout : MonoBehaviour
{
    public GameObject cardPrefab;

    private Canvas canvas;
    private Deck deck;
    private List<GameObject> cardInstances = new();
    private Vector2 offset = new();
    private float minAngle = 80;
    private float maxAngle = 100;
    private float radius = 2000f;
    private Vector3 cardScaleFactor = new(0.1f, 0.1f, 0f);
    private List<float> cardAngles = new();

    public void Start()
    {
        canvas = gameObject.transform.parent.gameObject.GetComponent<Canvas>();
        Setup();
    }

    private void Setup()
    {
        offset = canvas.pixelRect.center;
        offset.y = -radius * 0.925f;
        offset.x *= 0.925f;
    }

    private void SetAnglesBoundaries(int cardsNumber)
    {
        var halfDistance = (cardsNumber - 1) * 1.5f;
        minAngle = 90 - halfDistance;
        maxAngle = 90 + halfDistance;
    }

    public void Load(Deck _deck)
    {
        deck = _deck;
        SetAnglesBoundaries(deck.cards.Count);
        cardAngles = GetAngles(deck.cards.Count);
        for (var i = 0; i < deck.cards.Count; ++i)
        {
            var cardObject = Instantiate(cardPrefab, 
                new Vector3(offset.x, offset.y, 0f), 
                Quaternion.Euler(0f, 0f, maxAngle - 90f), 
                transform);
            cardObject.GetComponent<PathCardScript>().card = deck.cards[i].copy();
            cardObject.GetComponent<PathCardScript>().UpdateView();
            cardInstances.Add(cardObject);
        }
    }

    public void FadeIn()
    {
        if (cardInstances.Count == 0)
            return;
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut(Action afterCallback = null)
    {
        if (cardInstances.Count == 0)
        {
            afterCallback?.Invoke();
            return;
        }
        StartCoroutine(FadeOutCoroutine(afterCallback));
    }

    public IEnumerator FadeInCoroutine()
    {
        Vector3 startingPos = new(offset.x, offset.y, 0f);
        Vector3 finalPos = new(offset.x + radius * Mathf.Cos(maxAngle * Mathf.Deg2Rad), offset.y + radius * Mathf.Sin(maxAngle * Mathf.Deg2Rad), 0f);
        float time = 0.5f;
        float elapsedTime = 0;
        while (elapsedTime <= time)
        {
            cardInstances.ForEach(instance =>
            {
                instance.transform.SetPositionAndRotation(
                    Vector3.Lerp(startingPos, finalPos, 0.75f + 0.25f * (elapsedTime / time)), 
                    Quaternion.Euler(0, 0, maxAngle - 90f));
            });
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardInstances.ForEach(instance =>
        {
            instance.transform.position = finalPos;
        });

        var finalAngles = cardAngles;
        var currentAngle = finalAngles.Last();
        var indicesList = new List<int>(Enumerable.Range(0, cardInstances.Count));
        while (indicesList.Count > 0)
        {
            var indicesToRemove = new List<int>();
            for (var i = 0; i < indicesList.Count; ++i)
            {
                var index = indicesList[i];
                if (currentAngle > finalAngles[index])
                {
                    cardInstances[index].transform.SetPositionAndRotation(new Vector3(
                        offset.x + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                        offset.y + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad),
                        0f),
                        Quaternion.Euler(0, 0, currentAngle - 90f));
                }
                else
                {
                    indicesToRemove.Add(index);
                }
            }
            currentAngle -= 0.5f;
            foreach (var idx in indicesToRemove)
                indicesList.Remove(idx);
            yield return null;
        }
    }

    public IEnumerator FadeOutCoroutine(Action afterCallback = null)
    {
        List<float> currentAngles = new(cardAngles);
        var finalAngle = currentAngles.First();
        var indicesList = new List<int>(Enumerable.Range(0, cardInstances.Count));
        while (indicesList.Count > 0)
        {
            var indicesToRemove = new List<int>();
            for (var i = 0; i < indicesList.Count; ++i)
            {
                var index = indicesList[i];
                if (currentAngles[index] > finalAngle)
                {
                    cardInstances[index].transform.SetPositionAndRotation(new Vector3(
                        offset.x + radius * Mathf.Cos(currentAngles[index] * Mathf.Deg2Rad),
                        offset.y + radius * Mathf.Sin(currentAngles[index] * Mathf.Deg2Rad),
                        0f),
                        Quaternion.Euler(0, 0, currentAngles[i] - 90f));
                    currentAngles[index] -= 0.5f;
                }
                else
                {
                    indicesToRemove.Add(index);
                }
            }
            foreach (var idx in indicesToRemove)
                indicesList.Remove(idx);
            yield return null;
        }

        Vector3 startingPos = new(offset.x + radius * Mathf.Cos(minAngle * Mathf.Deg2Rad), offset.y + radius * Mathf.Sin(minAngle * Mathf.Deg2Rad), 0f);
        Vector3 finalPos = new(offset.x, offset.y, 0f);
        float time = 0.5f; 
        float elapsedTime = 0;
        while (elapsedTime <= time)
        {
            cardInstances.ForEach(instance =>
            {
                instance.transform.SetPositionAndRotation(
                    Vector3.Lerp(startingPos, finalPos, 0.25f * (elapsedTime / time)),
                    Quaternion.Euler(0, 0, minAngle - 90f));
            });
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cardInstances.ForEach(instance =>
        {
            instance.transform.position = finalPos;
        });

        afterCallback?.Invoke();
    }

    public void FocusCard(GameObject cardInstance)
    {
        var shiftedPos = GetCardOrientation(cardAngles[cardInstances.IndexOf(cardInstance)], 100f).Position;
        shiftedPos.x += 10f;
        cardInstance.transform.SetPositionAndRotation(shiftedPos, Quaternion.Euler(0, 0, 0));
        cardInstance.transform.localScale += cardScaleFactor;
        cardInstance.transform.SetAsLastSibling();
        var currentIndex = cardInstances.IndexOf(cardInstance);
        for (var i = 0; i < cardInstances.Count; ++i)
        {
            if (i == currentIndex)
                continue;
            var angleOffset = i < currentIndex ? -2f : 2f;
            var orientation = GetCardOrientation(cardAngles[i] + angleOffset);
            cardInstances[i].transform.SetPositionAndRotation(orientation.Position, orientation.Rotation);
        }
    }

    public void UnfocusCard(GameObject cardInstance)
    {
        cardInstance.transform.localScale -= cardScaleFactor;
        var orientations = GetCardsOrientations(cardAngles);
        for (var i = 0; i < cardInstances.Count; ++i)
        {
            cardInstances[i].transform.SetSiblingIndex(i);
            cardInstances[i].transform.SetPositionAndRotation(orientations[i].Position, orientations[i].Rotation);
        }
    }

    public void RemoveCard(GameObject cardInstance)
    {
        cardInstances.Remove(cardInstance);
        SetAnglesBoundaries(cardInstances.Count);
        cardAngles = GetAngles(cardInstances.Count);
        var cardsOrientations = GetCardsOrientations(cardAngles);
        for (var i = 0; i < cardInstances.Count; ++i)
        {
            cardInstances[i].transform.SetSiblingIndex(i);
            cardInstances[i].transform.SetPositionAndRotation(cardsOrientations[i].Position, cardsOrientations[i].Rotation);
        }
    }

    private CardOrientation GetCardOrientation(float angle, float radiusOffset = 0f) =>
        new()
        {
            Position = new Vector3(offset.x + (radius + radiusOffset) * Mathf.Cos(angle * Mathf.Deg2Rad), offset.y + (radius + radiusOffset) * Mathf.Sin(angle * Mathf.Deg2Rad), 0f),
            Rotation = Quaternion.Euler(0, 0, angle - 90f)
        };

    private List<CardOrientation> GetCardsOrientations(List<float> angles) => angles.Select(a => GetCardOrientation(a)).ToList();

    private List<float> GetAngles(int cardsNumber)
    {
        var angleAddition = 0f;
        if (cardsNumber > 1)
            angleAddition = (float)(maxAngle - minAngle) / (cardsNumber - 1);
        var angles = new List<float>();
        for (var i = 0; i < cardsNumber; ++i)
            angles.Add(minAngle + i * angleAddition);
        return angles;
    }
}
