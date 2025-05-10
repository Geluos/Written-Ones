using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Decks/Deck", order = -1)]
public class Deck : ScriptableObject
{
	[SerializeField]
	public List<Card> cards;

	public Deck(Deck deck)
	{
		cards = new List<Card>();
		foreach (Card card in deck.cards)
		{
			cards.Add(card.copy());
		}
	}

	public Deck()
	{
		cards = new List<Card>();
	}
}
