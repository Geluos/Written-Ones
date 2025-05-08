using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectBossFightEvent", menuName = "CardEffect/CardEffectBossFightEvent", order = -50)]
public class CardEffectBossFightEvent : CardEffect
{

    public override void Activate()
	{
		//FightController.main.StartBossFight();
    }

	public override CardEffect copy()
	{
		return new CardEffectBossFightEvent();
	}
}
