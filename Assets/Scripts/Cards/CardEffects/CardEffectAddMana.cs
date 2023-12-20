using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectAddMana", menuName = "CardEffect/CardEffectAddMana", order = -50)]
public class CardEffectAddMana : CardEffect
{
	public uint value = 1;
	public override void Activate()
	{
		FightController.main.addMana(value);
	}
}
