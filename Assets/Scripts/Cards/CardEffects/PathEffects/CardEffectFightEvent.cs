using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectFightEvent", menuName = "CardEffect/CardEffectFightEvent", order = -50)]
public class CardEffectFightEvent : CardEffect
{

    public override void Activate()
	{
		FightController.main.StartFight();
    }

	public override CardEffect copy()
	{
		return new CardEffectFightEvent();
	}
}
