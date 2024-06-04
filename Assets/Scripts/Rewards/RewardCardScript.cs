using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private readonly float scaleFactor = 1.2f;
    private RectTransform cardTransform;

    public Card.OwnerType ownerType;
    public Card card;
    public CardGFX cardGFX;
    public Action onSelectionEnd;

    private void Start()
    {
        cardTransform = GetComponent<RectTransform>();
    }

    public virtual void UpdateView()
    {
        cardGFX.portraitImage.sprite = card.sprite;
        cardGFX.manaString.text = card.manaPrice.ToString();
        cardGFX.descriptionString.text = card.description.ToString();
        cardGFX.name.text = card.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardTransform.localScale = new(
            cardTransform.localScale.x * scaleFactor,
            cardTransform.localScale.y * scaleFactor
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardTransform.localScale = new(
            cardTransform.localScale.x / scaleFactor,
            cardTransform.localScale.y / scaleFactor
        );
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FightController.main.characterDecks.Find(d => d.ownerType == ownerType).AddCard(card);
        onSelectionEnd();
    }
}
