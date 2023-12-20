using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEffectDamageEnemy", menuName = "CardEffect/CardEffectDamageEnemy", order = -50)]
public class CardEffectDamageEnemy : CardEffect
{
	public uint value = 10;
	public override void Activate()
	{
		FightController.main.DamageEnemy(value);
	}
}
