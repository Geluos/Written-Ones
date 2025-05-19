using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public ResultsOfConfiguration resultsOfConfiguration;
    //Несериализуемое поле
    private RunTestResult result;

    [HideInInspector]
    public AI ai = new SimpleAutomatAI();

    public TestConfig Copy()
    {
        var child = ScriptableObject.CreateInstance<TestConfig>();
        child.init();

        child.heroesSet = new List<Hero>();
        child.enemiesSet = new List<EnemySet>();
        child.rewardAfter = new List<RewardSet>();

        foreach (var hero in heroesSet)
        {
            var newHero = Instantiate(hero);
            newHero.startDeck = ScriptableObject.CreateInstance<Deck>();
            newHero.startDeck.cards = new List<Card>();
            foreach (var card in hero.startDeck.cards)
            {
                var newCard = card.copy(newHero);
                newCard.setOwnerForEffects();
                newHero.startDeck.cards.Add(newCard);
            }
            child.heroesSet.Add(newHero);
        }

        foreach (var enemySet in enemiesSet)
        {
            child.enemiesSet.Add(new EnemySet(new List<Enemy>()));
            foreach (var enemy in enemySet.enemies)
            {
                child.enemiesSet.Last().enemies.Add(Instantiate(enemy));
            }
        }

        foreach (var reward in rewardAfter)
        {
            child.rewardAfter.Add(reward);

        }

        return child;
    }

    public bool Fight(EnemySet enemySet)
    {
        //Debug.Log("StartAIFight");
        FightController.main.enemyList = new List<Enemy>();

        foreach (Enemy enemy in enemySet.enemies)
            FightController.main.enemyList.Add(Instantiate(enemy));

        FightController.main.heroList = heroes;

        FightController.main.heroesDeck = deck;

        FightController.main.StartAIFight();

        AI.TurnDecription turnDecription;
        bool test = FightController.main.CheckEnd();

        while (!FightController.main.CheckEnd())
        {
            FightController.main.startHeroTurn();
            for(int i=0; i<1000 && ((turnDecription = ai.turn()) != null && !FightController.main.CheckEnd()); ++i)
            {
                FightController.main.playCard(turnDecription.selectCard, turnDecription.target);
            }

            FightController.main.enemyTurn();
        }

        bool isWin = FightController.main.isWin();
        FightController.main.enemyList.ForEach(x => DestroyImmediate(x.gameObject));
        FightController.main.enemyList.Clear();

        return isWin;

    }

    public void init()
    {
        resultsOfConfiguration = new ResultsOfConfiguration();
        resultsOfConfiguration.results = new List<RunTestResult>();
        ai = new SimpleAutomatAI();
    }

    public RunTestResult play(int t = 1)
    {

        for (int q = 0; q < t; ++q)
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

            bool win = true;

            for (int i = 0; i < enemiesSet.Count; ++i)
            {
                win = win && Fight(enemiesSet[i]);

                if (FightController.main.isLose())
                {
                    break;
                }

                if (rewardAfter.Count > i)
                {
                    int next = UnityEngine.Random.Range(0, rewardAfter[i].cards.Count);
                    var rewardCard = rewardAfter[i].cards[next].copy();
                    deck.cards.Add(rewardCard);
                }
            }

            heroes.ForEach(x => DestroyImmediate(x.gameObject));
            heroes.Clear();


            result.isWin = win;

            resultsOfConfiguration.results.Add(result);

        }
        return result;
    }
}