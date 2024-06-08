using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectShieldSelf", menuName = "CardEffect/CardEffectShieldSelf", order = -50)]
public class CardEffectShieldSelf : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		FightController.main.AddShield(owner, (uint)par);
	}

	public override void Activate(Character target, int par)
	{
		FightController.main.AddShield(owner, (uint)par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectShieldSelf>();
	}
}
