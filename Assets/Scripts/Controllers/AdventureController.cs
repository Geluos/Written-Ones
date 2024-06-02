using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AdventureController : Controller<AdventureController>
{
	public Deck pathDeckAct1;
	public Deck pathDeckAct2;
	public Deck pathDeckAct3;
	public GameObject hand;
	public GameObject notificationDialog;
	public GameObject chooseDialog;

	private CardsLayout cardsLayout;

	public void Start()
	{
		StartNewAct();
	}

	public void StartNewAct()
	{
		if (FightController.main.actNum == 0)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct1);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 1)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct2);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 2)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct2);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 3)
		{
			//WIN
		}
	}

    public bool PlayCard(Card card)
	{
		foreach (var effect in card.effectsList)
			effect.effect.Activate();
		return true;
	}
}