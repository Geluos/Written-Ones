using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectAddMana", menuName = "CardEffect/CardEffectAddMana", order = -50)]
public class CardEffectAddMana : CardEffect
{
	public override void Activate()
	{
		FightController.main.AddMana(5);
	}

	public override void Activate(int par)
	{
		FightController.main.AddMana((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}

	public override CardEffect copy()
	{
		return new CardEffectAddMana();
	}
}
