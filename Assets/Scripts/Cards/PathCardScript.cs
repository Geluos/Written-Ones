using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PathCardScript : CardBaseScript
{
	public override void UpdateView()
	{
		cardGFX.portraitImage.sprite = card.sprite;
		cardGFX.name.text = card.name;
        cardGFX.leftBallImage.sprite = card.ballSprite;
        cardGFX.rightBallImage.sprite = card.ballSprite;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {

		FightController.main.isDragCard = false;

		var cardBottom = rectTransform.position.y - startPosition.y;
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

    public override void CardActivate(GameObject target = null)
	{
        if (AdventureController.main.PlayCard(card))
        {
            cardsLayout.RemoveCard(gameObject);
            Destroy(gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }
}
