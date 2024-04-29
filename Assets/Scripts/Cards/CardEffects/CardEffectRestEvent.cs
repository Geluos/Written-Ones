using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardEffectRestEvent", menuName = "CardEffect/CardEffectRestEvent", order = -50)]
public class CardEffectRestEvent : CardEffect
{
    public int healthAddition = 40;

    public override void Activate()
	{
		Instantiate(AdventureController.main.notificationDialog).GetComponent<RestEventScript>().Activate(healthAddition);
    }
}
