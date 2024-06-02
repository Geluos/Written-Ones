using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EffectObject
{
    public Sprite image;
    public string text;
    public CardEffect effect;
}

[Serializable]
public class EffectsCombination
{
    public List<EffectObject> effects;
}

[CreateAssetMenu(fileName = "CardEffectRandomEvent", menuName = "CardEffect/CardEffectRandomEvent", order = -50)]
public class CardEffectRandomEvent : CardEffect
{
    public List<EffectsCombination> effectsCombinations;

    private System.Random r = new System.Random();

    public override void Activate()
	{
        Instantiate(AdventureController.main.chooseDialog).GetComponent<RandomEventScript>().
            Activate(effectsCombinations[r.Next(effectsCombinations.Count)]);
    }

	public override CardEffect copy()
	{
		return new CardEffectRandomEvent();
	}
}
