using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectSetWeaknessEnemy", menuName = "CardEffect/CardEffectSetWeaknessEnemy", order = -50)]
public class CardEffectSetWeaknessEnemy : CardEffect
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
		FightController.main.SetEffect(target, CharacterEffect.weakness, par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectSetWeaknessEnemy>();
	}
}
