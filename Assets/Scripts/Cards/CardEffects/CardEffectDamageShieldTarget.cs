using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageShieldTarget", menuName = "CardEffect/CardEffectDamageShieldTarget", order = -50)]
public class CardEffectDamageShieldTarget : CardEffect
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
}
