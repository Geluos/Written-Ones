using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageRandomHero", menuName = "CardEffect/CardEffectDamageRandomHero", order = -50)]
public class CardEffectDamageRandomHero : CardEffect
{
	public uint value = 10;
	public override void Activate()
	{
		FightController.main.DamageRandomHero(value);
	}
}
