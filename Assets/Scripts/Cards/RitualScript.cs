using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RitualScript : MonoBehaviour
{
    public GameObject cardPrefab;
    public int columnsNumber = 5;
    public int rowsNumber = 2;
    public float dropFrameSize = 0.2f;
    public float buttonsOffset = 0.05f;

    private List<Card> deck;
    private RectTransform rect;
    private GameObject dropFrame;
    private GameObject previousButton;
    private GameObject nextButton;
    private float height;
    private float width;
    private Vector2 center;
    private readonly List<Vector3> cardsPositions = new();
    private readonly List<GameObject> cardInstances = new();
    private int pageNumber = 0;

    private void OnRectTransformDimensionsChange()
    {
        FetchSize();
        RecalculateCardsPositions();
        RelocateCards();
        if (dropFrame == null)
            InitDropFrame();
        RelocateDropFrame();
        if (previousButton == null || nextButton == null)
            InitButtons();
        RelocateButtons();
    }

    private void FetchSize()
    {
        rect = gameObject.GetComponent<RectTransform>();
        height = rect.rect.height;
        width = rect.rect.width;
        center = new(width / 2, height / 2);
    }

    private void RecalculateCardsPositions()
    {
        var verticalShift = height / (rowsNumber + 1);
        var horizontalShift = width / (columnsNumber + 1);
        var scaler = 1 - dropFrameSize;
        cardsPositions.Clear();
        for (int rowIdx = rowsNumber - 1; rowIdx >= 0; --rowIdx)
        {
            for (int colIdx = 0; colIdx < columnsNumber; ++colIdx)
            {
                cardsPositions.Add(new Vector3(
                    (horizontalShift + colIdx * horizontalShift) * scaler,
                    verticalShift + rowIdx * verticalShift,
                    0f
                ));
            }
        }
    }

    private void LoadDeck(List<Card> _deck)
    {
        deck = _deck;
        var offScreenLocation = new Vector3(-center.x * 2, 0f, 0f);
        var defaultAngle = Quaternion.Euler(0f, 0f, 0f);
        for (var i = 0; i < _deck.Count; ++i)
        {
            var cardObject = Instantiate(cardPrefab, offScreenLocation, defaultAngle, transform);
            var script = cardObject.GetComponent<CardInRitualScript>();
            script.card = _deck[i].Copy();
            script.dropFrameRectTransform = dropFrame.GetComponent<RectTransform>();
            script.UpdateView();
            script.activateAction = (Card card) =>
            {
                foreach(var hero in FightController.main.heroList)
                {
                    if (hero.currentDeck.Remove(card))
                        break;
                }
                Destroy(gameObject);
            };
            cardInstances.Add(cardObject);
        }
    }

    private void RelocateCards()
    {
        if (cardInstances.Count == 0)
            return;
        var pageSize = columnsNumber * rowsNumber;
        var visibleCardsIndices = Enumerable.Range(pageNumber * pageSize, pageSize);
        var visibleCardsIdx = 0;
        var offScreenLocation = new Vector3(-center.x * 2, 0f, 0f);
        for(var i = 0; i < cardInstances.Count; ++i)
        {
            if (visibleCardsIndices.Contains(i))
                cardInstances[i].transform.position = cardsPositions[visibleCardsIdx++];
            else
                cardInstances[i].transform.position = offScreenLocation;
        }
    }

    private void InitDropFrame()
    {
        dropFrame = transform.Find("RitualFrame").gameObject;
    }

    private void RelocateDropFrame()
    {
        dropFrame.transform.position = new(width * ((1 - dropFrameSize) + dropFrameSize / 3), height / 2, 0f);
    }

    private void InitButtons()
    {
        previousButton = transform.Find("PreviousButton").gameObject;
        nextButton = transform.Find("NextButton").gameObject;
    }

    private void AddButtonsListeners()
    {
        var pagesCount = (double)cardInstances.Count / (columnsNumber * rowsNumber);
        int maxPageIdx;
        if ((pagesCount - (int)pagesCount) > 0)
            maxPageIdx = (int)pagesCount;
        else
            maxPageIdx = (int)pagesCount - 1;

        previousButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (pageNumber > 0)
            {
                --pageNumber;
                RelocateCards();
            }

            if (pageNumber == 0)
                previousButton.GetComponent<Button>().interactable = false;

            if (pageNumber == (maxPageIdx - 1))
                nextButton.GetComponent<Button>().interactable = true;
        });

        nextButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (pageNumber < maxPageIdx)
            {
                ++pageNumber;
                RelocateCards();
            }

            if (pageNumber == maxPageIdx)
                nextButton.GetComponent<Button>().interactable = false;

            if (pageNumber == 1)
                previousButton.GetComponent<Button>().interactable = true;
        });
    }

    private void RelocateButtons()
    {
        if (cardInstances.Count == 0)
            return;
        var x = cardsPositions.Take(columnsNumber).Select(p => p.x).Average();
        var rectTransform = cardInstances[0].GetComponent<RectTransform>();
        var halhCardSize = rectTransform.rect.height / 2 * rectTransform.localScale.y;
        var lowerY = cardsPositions.Min(p => p.y) - halhCardSize;
        var upperY = cardsPositions.Max(p => p.y) + halhCardSize;
        nextButton.transform.position = new Vector3(x, lowerY - height * buttonsOffset, 0f);
        previousButton.transform.position = new Vector3(x, upperY + height * buttonsOffset, 0f);
    }

    public void Activate(List<Card> deck)
    {
        LoadDeck(deck);
        RelocateCards();
        AddButtonsListeners();
        RelocateButtons();
    }
}
