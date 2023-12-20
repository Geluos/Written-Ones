using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageAllHeroes", menuName = "CardEffect/CardEffectDamageAllHeroes", order = -50)]
public class CardEffectDamageAllHeroes : CardEffect
{
	public uint value = 10;
	public override void Activate()
	{
		FightController.main.DamageAllHeroes(value);
	}
}
