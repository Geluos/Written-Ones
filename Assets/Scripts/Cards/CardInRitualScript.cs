using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInRitualScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private float scaleFactor = 1.2f;

    public Card card;
    public CardGFX cardGFX;
    public RectTransform dropFrameRectTransform;
    public Action<Card> activateAction;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void UpdateView()
    {
        cardGFX.portraitImage.sprite = card.sprite;
        cardGFX.manaString.text = card.manaPrice.ToString();
        cardGFX.descriptionString.text = card.description.ToString();
        cardGFX.name.text = card.name;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        startPosition = transform.position;
        rectTransform.localScale = new Vector3(rectTransform.localScale.x * scaleFactor, rectTransform.localScale.y * scaleFactor);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (IsInsideDropFrame())
            CardActivate();
        else
        {
            rectTransform.localScale = new Vector3(rectTransform.localScale.x / scaleFactor, rectTransform.localScale.y / scaleFactor);
            transform.position = startPosition;
        }
    }

    private bool IsInsideDropFrame()
    {
        var cardMinX = rectTransform.position.x - rectTransform.rect.width / 2 * rectTransform.localScale.x;
        var cardMaxX = rectTransform.position.x + rectTransform.rect.width / 2 * rectTransform.localScale.x;
        var cardMinY = rectTransform.position.y - rectTransform.rect.height / 2 * rectTransform.localScale.y;
        var cardMaxY = rectTransform.position.y + rectTransform.rect.height / 2 * rectTransform.localScale.y;

        var dropFrameMinX = dropFrameRectTransform.position.x - dropFrameRectTransform.rect.width / 2 * dropFrameRectTransform.localScale.x;
        var dropFrameMaxX = dropFrameRectTransform.position.x + dropFrameRectTransform.rect.width / 2 * dropFrameRectTransform.localScale.x;
        var dropFrameMinY = dropFrameRectTransform.position.y - dropFrameRectTransform.rect.height / 2 * dropFrameRectTransform.localScale.y;
        var dropFrameMaxY = dropFrameRectTransform.position.y + dropFrameRectTransform.rect.height / 2 * dropFrameRectTransform.localScale.y;

        return dropFrameMinX < cardMinX &&
            dropFrameMaxX > cardMaxX &&
            dropFrameMinY < cardMinY &&
            dropFrameMaxY > cardMaxY;
    }

    public virtual void CardActivate(GameObject target = null)
    {
        activateAction.Invoke(card);
    }
}
