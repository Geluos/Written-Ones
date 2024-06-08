using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectDamageAllHeroes", menuName = "CardEffect/CardEffectDamageAllHeroes", order = -50)]
public class CardEffectDamageAllHeroes : CardEffect
{
	public override void Activate()
	{
		FightController.main.DamageAllHeroes(10);
	}

	public override void Activate(int par)
	{
		if (owner.getEffect(CharacterEffect.weakness) > 0)
		{
			int cnt = System.Math.Min(5, owner.getEffect(CharacterEffect.weakness));
			par = (int)Mathf.Round(par * (10 - cnt) / 10f);
		}
		FightController.main.DamageAllHeroes((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectDamageAllHeroes>();
	}
}
