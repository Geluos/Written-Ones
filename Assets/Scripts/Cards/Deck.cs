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

	public void Shuffle()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			var temp = cards[i];
			int randomIndex = Random.Range(i, cards.Count);
			cards[i] = cards[randomIndex];
			cards[randomIndex] = temp;
		}
	}
}
