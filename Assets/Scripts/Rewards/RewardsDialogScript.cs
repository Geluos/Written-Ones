using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RewardsDialogScript : MonoBehaviour
{
    protected static readonly System.Random random = new();
    public int minGold;
    public int maxGold;
    public List<GameObject> cardObjects;
    public GameObject goldButton;
    private List<RewardCardScript> cardScripts;
    private RewardButtonScript goldButtonScript;

    private void Awake()
    {
        goldButtonScript = goldButton.GetComponent<RewardButtonScript>();
        goldButtonScript.onSelectionEnd = OnRewardSelected;

        cardScripts = new();
        foreach (var cardObj in cardObjects)
        {
            var script = cardObj.GetComponent<RewardCardScript>();
            script.onSelectionEnd = OnRewardSelected;
            cardScripts.Add(script);
        }
    }

    public void GiveReward()
    {
        gameObject.SetActive(true);
        var cards = new List<Tuple<Card.OwnerType, Card>>();
        foreach(var hero in FightController.main.heroList)
        {
            foreach(var card in hero.startDeck.cards)
            {
                cards.Add(Tuple.Create(hero.ownerTypeForCharacter, card));
            }
        }

        for (int i = 0; i < cardObjects.Count; i++)
        {
            LoadRandomCard(i, cards);
        }

        goldButtonScript.GoldAmount = random.Next(minGold, maxGold + 1);
    }

    private void OnRewardSelected()
    {
        gameObject.SetActive(false);
        FightController.main.AdventureScene.SetActive(true);
        FightController.main.FightScene.SetActive(false);
    }

    private void LoadRandomCard(int idx, List<Tuple<Card.OwnerType, Card>> cards)
    {
        var script = cardScripts[idx];
        var randomCard = RandomListPop(cards);
        script.ownerType = randomCard.Item1;
        script.card = randomCard.Item2;
        script.UpdateView();
    }

    private static T RandomListPop<T>(List<T> lst)
    {
        int idx = random.Next(0, lst.Count);
        T value = lst[idx];
        lst.RemoveAt(idx);
        return value;
    }
}
