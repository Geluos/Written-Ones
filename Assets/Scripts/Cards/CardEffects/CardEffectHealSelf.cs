using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectHealSelf", menuName = "CardEffect/CardEffectHealSelf", order = -50)]
public class CardEffectHealSelf : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		FightController.main.HealHp(owner, (uint)par);
	}

	public override void Activate(Character target, int par)
	{
		FightController.main.HealHp(owner, (uint)par);
	}

	public override CardEffect copy()
	{
		return new CardEffectHealSelf();
	}
}
