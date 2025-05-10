using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


[CreateAssetMenu(fileName = "TestConfig", menuName = "TestConfig/TestConfig", order = -1)]
public class TestConfig : ScriptableObject
{
    [SerializeField]
    public List<Hero> heroesSet;
    [SerializeField]
    public List<EnemySet> enemiesSet;
    [SerializeField]
    public List<RewardSet> rewardAfter;

    [DoNotSerialize]
    [HideInInspector]
    public Deck deck; 
    [DoNotSerialize]
    [HideInInspector]
    public List<Hero> heroes;


    //Несериализуемое поле
    private RunTestResult result;
    [HideInInspector]
    public AI ai;

    public void Fight(EnemySet enemySet)
    {
        Debug.Log("StartAIFight");
        FightController.main.enemyList = new List<Enemy>();

        foreach (Enemy enemy in enemySet.enemies)
            FightController.main.enemyList.Add(Instantiate(enemy));

        FightController.main.heroList = heroes;

        FightController.main.heroesDeck = deck;

        FightController.main.StartAIFight();

        AI.TurnDecription turnDecription;
        bool test = FightController.main.CheckEnd();

        //TODO: Replace
        ai = new SimpleAutomatAI();

        while (!FightController.main.CheckEnd())
        {
            FightController.main.startHeroTurn();
            for(int i=0; i<1000 && ((turnDecription = ai.turn()) != null && !FightController.main.CheckEnd()); ++i)
            {
                FightController.main.playCard(turnDecription.selectCard, turnDecription.target);
            }

            FightController.main.enemyTurn();
        }
        foreach (var hero in FightController.main.heroList)
        {
            Debug.Log(hero.name + ": " + hero.current_hp);
        }
        foreach (var enemy in FightController.main.enemyList)
        {
            Debug.Log(enemy.name + ": " + enemy.current_hp);
        }
    }

    public RunTestResult play()
    {
        result = new RunTestResult();

        heroes = new List<Hero>();

        foreach (Hero hero in heroesSet)
            heroes.Add(Instantiate(hero));

        deck = ScriptableObject.CreateInstance<Deck>();

        foreach (Hero hero in heroesSet)
        {
            if (hero != null)
            {
                foreach (Card card in hero.startDeck.cards)
                {
                    var cardCopy = card.copy();
                    cardCopy.owner = hero;
                    cardCopy.setOwnerForEffects();
                    deck.cards.Add(cardCopy);
                }
            }
        }

        for (int i = 0; i < enemiesSet.Count; ++i)
        {
            Fight(enemiesSet[i]);

            if (FightController.main.isLose())
                break;

            if (rewardAfter.Count > i)
            {
                int next = UnityEngine.Random.Range(0, rewardAfter[i].cards.Count);
                var rewardCard = rewardAfter[i].cards[next].copy();
                deck.cards.Add(rewardCard);
            }
        }

        return result;
    }
}