using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectShieldAllHeroes", menuName = "CardEffect/CardEffectShieldAllHeroes", order = -50)]
public class CardEffectShieldAllHeroes : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		FightController.main.AddShieldAllHeroes((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectShieldAllHeroes>();
	}
}
