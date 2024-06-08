using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectShieldTarget", menuName = "CardEffect/CardEffectShieldTarget", order = -50)]
public class CardEffectShieldTarget : CardEffect
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
		FightController.main.AddShield(target, (uint)par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectShieldTarget>();
	}
}
