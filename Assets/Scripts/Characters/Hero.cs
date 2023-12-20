using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO ADD Abilitus and EXTRACT
/*
public class Ability
{

}
*/


//TODO Extract
public class Character : MonoBehaviour
{
	public uint max_hp = 10;
	public uint current_hp = 10;
	public Deck startDeck = null;
	protected List<Card> currentDeck = null;
	public GameObject GFX = null;

	public void Awake()
	{
		currentDeck = new List<Card>();
		foreach (Card card in startDeck.cards)
			currentDeck.Add(card.copy());
		Shuffle<Card>(currentDeck);
	}

	public void getDamage(uint value)
	{
		current_hp = System.Math.Max(0, value);
	}

	public bool isAlive()
	{
		return current_hp > 0;
	}
	//TODO ADD
	//public Ability ability;


	//EXTRACT to UTILS
	public static void Shuffle<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			var temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}

}

public class Hero : Character
{
	public Card getCard()
	{
		Card res;
		if (currentDeck.Count <= 0)
		{
			currentDeck = new List<Card>();
			foreach (Card card in startDeck.cards)
				currentDeck.Add(card.copy());
			Shuffle<Card>(currentDeck);
		}

		res = currentDeck[0];
		currentDeck.RemoveAt(0);
		return res;
	}
}
