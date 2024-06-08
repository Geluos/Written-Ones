using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectLoseGoldEvent", menuName = "CardEffect/CardEffectLoseGoldEvent", order = -50)]
public class CardEffectLoseGoldEvent : CardEffect
{


    public override void Activate()
	{
		GoldController.main.goldData.goldValue = 0;
    }

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectLoseGoldEvent>();
	}
}
