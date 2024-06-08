using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectShieldAllMonsters", menuName = "CardEffect/CardEffectShieldAllMonsters", order = -50)]
public class CardEffectShieldAllMonsters : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		FightController.main.AddShieldAllEnemies((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		FightController.main.AddShield(target, (uint)par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectShieldAllMonsters>();
	}
}
