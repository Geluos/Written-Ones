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
	public Deck deck = null;
	public GameObject GFX = null;

	public void getDamage(uint value)
	{
		current_hp = System.Math.Max(0, value);
	}

	public bool isAlive()
	{
		return current_hp > 0;
	}
	
}

public class Hero : Character
{
	//TODO ADD
	//public Ability ability;
}
