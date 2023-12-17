using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBaseScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 startPosition;
	public Card card;
    
    private void Start()
    {
        canvas = transform.parent.parent.gameObject.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        var cardBottom = rectTransform.anchoredPosition.y - rectTransform.rect.height / 2;
        if (cardBottom > 0)
        {
            CardActivate();
            Destroy(gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public virtual void CardActivate()
    {
        Debug.Log("Wow");
    }
}
