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
	public GameObject GFX = null;

	[HideInInspector]
	public uint shield = 0;

	

	public void getDamage(uint value)
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
			current_hp = current_hp - value;
		else
			current_hp = 0;
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
