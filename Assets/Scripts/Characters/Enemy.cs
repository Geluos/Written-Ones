using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Character
{

	public Canvas monsterCanvas;
	private GameObject card;
	[HideInInspector]
	public CardHolder nextCard;
	public void OnMouseEnter()
	{
		if (FightController.main.isDragCard)
		{
			GFX.GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}
		if (card)
		{
			Destroy(card);
		}
		if (nextCard != null)
		{
			var cardsLayout = FightController.main.hand.GetComponent<CardsLayout>();
			card = Instantiate(cardsLayout.cardPrefab, monsterCanvas.transform);
			

			card.GetComponent<CardBaseScript>().isPlayable = false;

			card.GetComponent<CardBaseScript>().card = nextCard.card.copy();
			card.GetComponent<CardBaseScript>().UpdateView();


			RectTransform uitransform = card.GetComponent<RectTransform>();
			uitransform.anchorMin = new Vector2(0.5f, 1);
			uitransform.anchorMax = new Vector2(0.5f, 1);
			uitransform.pivot = new Vector2(0.5f, 1);

			card.transform.localPosition += new Vector3(0f, 30f, 0f);
			card.transform.localScale = new Vector3( 0.05f, 0.05f, 0.05f );
		}
	}

	public void OnMouseExit()
	{

		GFX.GetComponentInChildren<SpriteRenderer>().color = Color.white;

		if (card)
		{
			Destroy(card);
		}
	}

}