using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInRitualScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private float scaleFactor = 1.2f;
    private Vector2 oldPosition;

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
        cardGFX.crystalImage.sprite = card.crystalSprite;
        cardGFX.leftBallImage.sprite = card.ballSprite;
        cardGFX.rightBallImage.sprite = card.ballSprite;
        cardGFX.manaString.text = card.manaPrice.ToString();
        cardGFX.descriptionString.text = card.description.ToString();
        cardGFX.name.text = card.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(rectTransform.localScale.x * scaleFactor, rectTransform.localScale.y * scaleFactor);
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(rectTransform.localScale.x / scaleFactor, rectTransform.localScale.y / scaleFactor);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        oldPosition = eventData.position;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var offset = eventData.position - oldPosition;
        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0f);
        oldPosition = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (IsInsideDropFrame())
            CardActivate();
        else
            transform.position = startPosition;
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
