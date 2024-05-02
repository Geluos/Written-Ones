using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectRandomEvent_1", menuName = "CardEffect/CardEffectRandomEvent", order = -50)]
public class CardEffectRandomEvent_1 : CardEffect
{
	public override void Activate()
	{
		FightController.main.DamageAllHeroes(10);
	}

	public override void Activate(int par)
	{
		FightController.main.DamageAllHeroes((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}
}
