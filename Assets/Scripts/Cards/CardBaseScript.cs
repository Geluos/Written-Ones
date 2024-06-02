using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBaseScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected CardsLayout cardsLayout;
    protected Canvas canvas;
    protected RectTransform rectTransform;
    protected Vector2 startPosition;
	public Card card;
	public CardGFX cardGFX;
    public List<GameObject> cardInstances;

	[HideInInspector]
	public bool isPlayable = true;

    private void Start()
    {
        cardsLayout = transform.parent.GetComponent<CardsLayout>();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
		if (!isPlayable)
			return;
		cardsLayout.FocusCard(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (!isPlayable)
			return;
		cardsLayout.UnfocusCard(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
		if (!isPlayable)
			return;
		startPosition = transform.position;
		FightController.main.isDragCard = true;
        if (this.GetComponent<CardBuyer>() != null) { return; }
        //startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (!isPlayable)
			return;
		transform.position = eventData.position;
        //if (this.GetComponent<CardBuyer>() != null) { return; }
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData) 
    {
		if (!isPlayable)
			return;

		FightController.main.isDragCard = false;

		var cardBottom = rectTransform.position.y - startPosition.y;
        //if (this.GetComponent<CardBuyer>() != null) { return; }
        //var cardBottom = rectTransform.anchoredPosition.y - rectTransform.rect.height / 4;
		Debug.Log(cardBottom);
		if (cardBottom > 0)
        {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				Debug.Log(hit.collider.gameObject.name);
				CardActivate(hit.collider.gameObject);
			}
			else
			{
				CardActivate();
			}
        }
        else
        {
            rectTransform.position = startPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public virtual void CardActivate(GameObject target = null)
    {
		if (!isPlayable)
			return;
		//Debug.Log("Try play");
		if (FightController.main.playCard(card, target))
		{
			cardsLayout.RemoveCard(gameObject);
			Destroy(gameObject);
		}
		else
		{
			rectTransform.position = startPosition;
		}
    }
}
