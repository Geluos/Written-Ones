using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageSelf", menuName = "CardEffect/CardEffectDamageSelf", order = -50)]
public class CardEffectDamageSelf : CardEffect
{
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}

	public override void Activate(int par)
	{
		if (owner?.getEffect(CharacterEffect.vulnerability) > 0)
		{
			owner.popEffect(CharacterEffect.vulnerability);
			par = (int)Mathf.Round(par * 1.5f);
		}
		if (owner?.getEffect(CharacterEffect.weakness) > 0)
		{
			int cnt = System.Math.Min(5, owner.getEffect(CharacterEffect.weakness));
			par = (int)Mathf.Round(par * (10 - cnt) / 10f);
		}
		FightController.main.DamageEnemy(owner, (uint)par);
	}

	public override void Activate(Character target, int par)
	{
		if (owner?.getEffect(CharacterEffect.vulnerability) > 0)
		{
			owner.popEffect(CharacterEffect.vulnerability);
			par =  (int)Mathf.Round(par * 1.5f);
		}
		if (owner?.getEffect(CharacterEffect.weakness) > 0)
		{
			int cnt = System.Math.Min(5, owner.getEffect(CharacterEffect.weakness));
			par = (int)Mathf.Round(par * (10 - cnt) / 10f);
		}
		FightController.main.DamageEnemy(owner, (uint)par);
	}

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectDamageSelf>();
	}
}
