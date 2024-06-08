using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectAddGoldEvent", menuName = "CardEffect/CardEffectAddGoldEvent", order = -50)]
public class CardEffectAddGoldEvent : CardEffect
{
	public int gold_count = 100;

    public override void Activate()
	{
		GoldController.main.Add(gold_count);
    }

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectAddGoldEvent>();
	}
}
