using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AdventureController : Controller<AdventureController>
{
	public Deck pathDeck;
    public GameObject hand;
	public GameObject notificationDialog;
	public GameObject chooseDialog;

	private CardsLayout cardsLayout;

	public void Start()
	{
		cardsLayout = hand.GetComponent<CardsLayout>();
		cardsLayout.Load(pathDeck);
		cardsLayout.FadeIn();
	}

    public bool PlayCard(Card card)
	{
		foreach (var effect in card.effectsList)
			effect.effect.Activate();
		return true;
	}
}