using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectNothing", menuName = "CardEffect/CardEffectNothing", order = -50)]
public class CardEffectNothing : CardEffect
{
    public override void Activate() { }

	public override CardEffect copy()
	{
		return new CardEffectNothing();
	}
}
