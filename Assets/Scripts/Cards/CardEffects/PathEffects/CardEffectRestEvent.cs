using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectRestEvent", menuName = "CardEffect/CardEffectRestEvent", order = -50)]
public class CardEffectRestEvent : CardEffect
{
    public int minHealthAddition = 30;
    public int maxHealthAddition = 50;

	private readonly System.Random rand = new System.Random();

    public override void Activate()
	{
		var healthAddition = rand.Next(minHealthAddition, maxHealthAddition);
		Instantiate(AdventureController.main.notificationDialog).GetComponent<RestEventScript>().Activate(healthAddition);
    }

	public override CardEffect copy()
	{
		return new CardEffectRestEvent();
	}
}
