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
		if (owner?.getEffect(CharacterEffect.weakness) > 0)
		{
			int cnt = System.Math.Min(5, owner.getEffect(CharacterEffect.weakness));
			par = (int)Mathf.Round(par * (10 - cnt) / 10f);
		}
		FightController.main.DamageAllEnemies((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		if (owner.getEffect(CharacterEffect.weakness) > 0)
		{
			int cnt = System.Math.Min(5, owner.getEffect(CharacterEffect.weakness));
			par = (int)Mathf.Round(par * (10 - cnt) / 10f);
		}
		FightController.main.DamageAllEnemies((uint)par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectDamageAllEnemies>();
	}
}
