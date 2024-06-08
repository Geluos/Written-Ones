using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//TODO ADD Abilitus and EXTRACT
/*
public class Ability
{

}
*/

public enum CharacterEffect { weakness, vulnerability }

public class StatusEffect
{
	public int cnt;
	public CharacterEffect effect;

	public StatusEffect(int cnt, CharacterEffect effect)
	{
		this.cnt = cnt;
		this.effect = effect;
	}
}
public class Character : MonoBehaviour
{
	public uint max_hp = 10;
	public uint current_hp = 10;
	public Deck startDeck = null;
	public GameObject GFX = null;
	public List<StatusEffect> effects = new List<StatusEffect>();

	[HideInInspector]
	public uint shield = 0;
	
	public int getEffect(CharacterEffect effect)
	{
		var t = effects.Where<StatusEffect>(x => x.effect == effect);
		if (t.Count() > 0)
		{
			return t.First().cnt;
		}
		return 0;
	}

	public void setEffect(CharacterEffect effect, int power)
	{
		var t = effects.Where<StatusEffect>(x => x.effect == effect);
		if (t.Count() > 0)
		{
			t.First().cnt += power;
		}
		else
		{
			effects.Add(new StatusEffect(power, effect));
		}
	}

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

	public void popEffect(CharacterEffect effect)
	{
		switch (effect)
		{
			case CharacterEffect.weakness 
				or CharacterEffect.vulnerability:
			{
					var t = effects.Where<StatusEffect>(x => x.effect == effect);
					if (t.Count() > 0)
					{
						t.First().cnt--;
						if (t.Count() <= 0)
						{
							effects.RemoveAll(x => x.effect == effect);
						}
					}
					break;
			}
		}
		
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
