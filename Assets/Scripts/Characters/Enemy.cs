using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Character
{

	public Canvas monsterCanvas;
	private GameObject card;
	private GameObject targetIcon;
	[HideInInspector]
	public CardHolder nextCard;
	[HideInInspector]
	public Character nextTarget;
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

			card.GetComponent<CardBaseScript>().card = nextCard.card.Copy();
			card.GetComponent<CardBaseScript>().UpdateView();


			RectTransform uitransform = card.GetComponent<RectTransform>();
			uitransform.anchorMin = new Vector2(0.5f, 1);
			uitransform.anchorMax = new Vector2(0.5f, 1);
			uitransform.pivot = new Vector2(0.5f, 1);

			card.transform.localPosition += new Vector3(0f, 30f, 0f);
			card.transform.localScale = new Vector3( 0.05f, 0.05f, 0.05f );

			if (nextTarget != null)
			{
				targetIcon = new GameObject("TARGET_ICON");
				var renderer = targetIcon.AddComponent<SpriteRenderer>();
				renderer.sprite = GlobalSpritesController.main.blast;

				

				targetIcon.transform.SetParent(nextTarget.transform);
				targetIcon.transform.position = nextTarget.gameObject.transform.position;
				targetIcon.transform.rotation = new Quaternion();
				targetIcon.transform.localPosition += new Vector3(0f, 5f, 0f);
				targetIcon.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

			}
		}
	}

	public void OnMouseExit()
	{

		GFX.GetComponentInChildren<SpriteRenderer>().color = Color.white;

		if (card)
		{
			Destroy(card);
		}

		if (targetIcon)
		{
			Destroy(targetIcon);
		}
	}

}
