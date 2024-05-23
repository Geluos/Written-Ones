using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBaseScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected Canvas canvas;
    protected RectTransform rectTransform;
    protected Vector2 startPosition;
	public Card card;
	public CardGFX cardGFX;

    private void Start()
    {
        canvas = transform.parent.parent.gameObject.GetComponent<Canvas>();
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
        if (this.GetComponent<CardBuyer>() != null) { return; }
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.GetComponent<CardBuyer>() != null) { return; }
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        if (this.GetComponent<CardBuyer>() != null) { return; }
        var cardBottom = rectTransform.anchoredPosition.y - rectTransform.rect.height / 4;
		Debug.Log(cardBottom);
		if (cardBottom > 0)
        {
            CardActivate();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public virtual void CardActivate()
    {
		Debug.Log("Try play");
		if (FightController.main.playCard(card))
		{
			Destroy(gameObject);
		}
		else
		{
			rectTransform.anchoredPosition = startPosition;
		}
    }
}
