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
	protected Deck currentDeck = null;
	public GameObject GFX = null;

	public void Awake()
	{
		currentDeck = new Deck(startDeck);
		currentDeck.Shuffle();
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
	
}

public class Hero : Character
{
	public Card getCard()
	{
		Card res;
		if (currentDeck.cards.Count <= 0)
		{
			currentDeck = new Deck(startDeck);
			currentDeck.Shuffle();
		}

		res = currentDeck.cards[0];
		currentDeck.cards.RemoveAt(0);
		return res;
	}
}
