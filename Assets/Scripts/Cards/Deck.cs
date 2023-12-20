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
		foreach (Card card in deck.cards)
		{
			cards.Add(card.copy());
		}
	}
}
