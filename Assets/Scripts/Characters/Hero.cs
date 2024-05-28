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
	public List<Card> currentDeck = null;
	public GameObject GFX = null;

	[HideInInspector]
	public uint shield = 0;

	public void Awake()
	{
		currentDeck = new List<Card>();
		foreach (Card card in startDeck.cards)
			currentDeck.Add(card.Copy());
		Shuffle(currentDeck);
	}

	public void GetDamage(uint value)
	{
		if (shield > value)
		{
			shield -= value;
			value = 0;
		}
		else
		{
			value -= shield;
			shield = 0;
		}

		if (value < current_hp)
			current_hp -= value;
		else
			current_hp = 0;
	}

	public bool IsAlive => current_hp > 0;
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
	public Card GetCard()
	{
		Card res;
		if (currentDeck.Count <= 0)
		{
			currentDeck = new();
			foreach (Card card in startDeck.cards)
				currentDeck.Add(card.Copy());
			Shuffle(currentDeck);
		}

		res = currentDeck[0];
		currentDeck.RemoveAt(0);
		return res;
	}

}

public class Hero : Character
{

}
