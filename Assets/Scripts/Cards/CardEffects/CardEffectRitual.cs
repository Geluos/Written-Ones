using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(fileName = "CardEffectRitual", menuName = "CardEffect/CardEffectRitual", order = -50)]
public class CardEffectRitual : CardEffect
{
    public override void Activate()
    {
        Instantiate(AdventureController.main.ritualDialog).GetComponent<RitualScript>().
            Activate(FightController.main.characterDecks.Select(h => h.deck.cards).SelectMany(c => c).ToList());
    }

	public override CardEffect copy()
	{
		return CreateInstance<CardEffectRitual>();
	}
}
