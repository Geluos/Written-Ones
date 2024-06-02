using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageSelf", menuName = "CardEffect/CardEffectDamageSelf", order = -50)]
public class CardEffectDamageSelf : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		FightController.main.DamageEnemy(owner, (uint)par);
	}

	public override void Activate(Character target, int par)
	{
		FightController.main.DamageEnemy(owner, (uint)par);
	}

	public override CardEffect copy()
	{
		return new CardEffectDamageSelf();
	}
}
