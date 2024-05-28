using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectAddCard", menuName = "CardEffect/CardEffectAddCard", order = -50)]
public class CardEffectAddCard : CardEffect
{
	public override void Activate()
	{
		FightController.main.AddCard(1);
	}

	public override void Activate(int par)
	{
		FightController.main.AddCard((uint)par);
	}

	public override void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}
}
