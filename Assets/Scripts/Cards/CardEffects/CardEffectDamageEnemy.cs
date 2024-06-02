using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageEnemy", menuName = "CardEffect/CardEffectDamageEnemy", order = -50)]
public class CardEffectDamageEnemy : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(Character target, int par)
	{
		FightController.main.DamageEnemy(target, (uint)par);
	}

	public override CardEffect copy()
	{
		return new CardEffectDamageEnemy();
	}
}
