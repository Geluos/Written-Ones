using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageRandomHero", menuName = "CardEffect/CardEffectDamageRandomHero", order = -50)]
public class CardEffectDamageRandomHero : CardEffect
{
	public override void Activate()
	{
		FightController.main.DamageRandomHero(10);
	}

	public override void Activate(int par)
	{
		FightController.main.DamageRandomHero((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}

	public override CardEffect copy()
	{
		return new CardEffectDamageRandomHero();
	}
}
