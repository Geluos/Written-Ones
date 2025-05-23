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
    private Vector2 oldPosition;

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
        cardGFX.crystalImage.sprite = card.crystalSprite;
        cardGFX.leftBallImage.sprite = card.ballSprite;
		cardGFX.rightBallImage.sprite = card.ballSprite;
		cardGFX.manaString.text = card.manaPrice.ToString();
		cardGFX.descriptionString.text = card.description.ToString();
		cardGFX.name.text = card.name;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		if (this.GetComponent<CardBuyer>() != null)
			return;
		cardsLayout.FocusCard(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (this.GetComponent<CardBuyer>() != null)
			return;
		cardsLayout.UnfocusCard(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (this.GetComponent<CardBuyer>() != null)
			return;
		cardsLayout.cardIsDragged = true;
        oldPosition = eventData.position;
        startPosition = rectTransform.position;
		FightController.main.isDragCard = true;
		SoundController.main.PlaySound(SoundController.main.CardStartPlay);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		if (this.GetComponent<CardBuyer>() != null)
			return;
		var offset = eventData.position - oldPosition;
		rectTransform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0f);
		oldPosition = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
		if (this.GetComponent<CardBuyer>() != null)
			return;
        cardsLayout.cardIsDragged = false;
        FightController.main.isDragCard = false;

		var cardBottom = rectTransform.position.y - startPosition.y;
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
		if (this.GetComponent<CardBuyer>() != null)
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
