using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Card;

public class ConfigScriptBaseDecks : CONFIGURATION_TEST
{
    public Deck additionCards;

    public override void Test()
    {
        //base.Test();

    }



    double grade(TestConfig con)
    {
        double grade = 0.0;
        foreach (var item in con.resultsOfConfiguration.results)
        {
            if (item.isWin)
                grade += 1.0;
        }

        if (grade > 85.0)
            grade -= 30;

        foreach (var hero in con.heroesSet)
        {
            if (hero.startDeck.cards.Count < 3 && hero.startDeck.cards.Count > 5)
                grade -= 2 * Math.Min(Math.Abs(hero.startDeck.cards.Count - 3), Math.Abs(hero.startDeck.cards.Count - 5));

            foreach (var card in hero.startDeck.cards)
                if (card.rarity != Card.Rarity.Common)
                {
                    grade -= 3.0;
                }
        }


        return grade;


    }

    private struct Gen
    {
        public TestConfig conf;
        public double grade;
    }

    private TestConfig crossover(TestConfig con1, TestConfig con2)
    {
       

    }


    private void mutate(TestConfig con)
    {
        var rnd = UnityEngine.Random.value;
        rnd *= 3;
        OwnerType owner;
        Deck deck;
        if(rnd < 1.0)
        {
            deck = con.heroesSet[0].startDeck;
            owner = con.heroesSet[0].ownerTypeForCharacter;
        }
        else if (rnd < 2.0)
        {
            deck = con.heroesSet[1].startDeck;
            owner = con.heroesSet[1].ownerTypeForCharacter;
        }
        else
        {
            deck = con.heroesSet[2].startDeck;
            owner = con.heroesSet[2].ownerTypeForCharacter;
        }

        rnd = UnityEngine.Random.value;
        List<Card> cardByOwner = new List<Card>();
        foreach (var card in additionCards.cards)
        {
            if (card.otype == owner || card.otype == OwnerType.Other)
            {
                cardByOwner.Add(card);
            }
        }

        //Delete
        if (rnd < 0.3)
        {
            if (deck.cards.Count > 0)
            {
                int next = UnityEngine.Random.Range(0, deck.cards.Count);
                deck.cards.RemoveAt(next);
            }
        }
        //Add
        else if (rnd < 0.6)
        {
            int next = UnityEngine.Random.Range(0, cardByOwner.Count);
            deck.cards.Add(cardByOwner[next].copy());
        }
        //Replace
        else
        {
            if (deck.cards.Count > 0)
            {
                int next1 = UnityEngine.Random.Range(0, deck.cards.Count);
                int next2 = UnityEngine.Random.Range(0, cardByOwner.Count);
                deck.cards[next1] = cardByOwner[next2].copy();
            }
            else
            {
                int next = UnityEngine.Random.Range(0, cardByOwner.Count);
                deck.cards.Add(cardByOwner[next].copy());
            }
        }

    }


    void genetic_algo()
    {
        int popCount = 50;
        int generations = 5;


    }
}
