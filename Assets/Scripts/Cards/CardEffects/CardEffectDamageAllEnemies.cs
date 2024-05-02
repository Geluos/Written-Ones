using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageAllEnemies", menuName = "CardEffect/CardEffectDamageAllEnemies", order = -50)]
public class CardEffectDamageAllEnemies : CardEffect
{
	public override void Activate()
	{
		Activate(0);
	}

	public override void Activate(int par)
	{
		FightController.main.DamageAllEnemies((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}
}
