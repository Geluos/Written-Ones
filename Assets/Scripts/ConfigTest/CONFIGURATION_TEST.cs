using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AI;
using static UnityEngine.GraphicsBuffer;


[Serializable]
public class RunTestResult
{

}

[Serializable]
public struct EnemySet
{
    [SerializeField]
    public List<Enemy> enemies;
}

public abstract class AI
{
    public class TurnDecription
    {
        public Card selectCard;
        public Character target;
    }
    public abstract void getInfo(TestConfig config);
    public abstract TurnDecription turn();
}

public class SimpleAutomatAI : AI
{
    public override void getInfo(TestConfig config)
    {
        throw new NotImplementedException();
    }

    public override TurnDecription turn()
    {
        var res = new TurnDecription();
        var cards = FightController.main.cards;


        foreach (var card in cards)
        {
            if (card.manaPrice == 0
                && card.effectsList.Exists(x => (x.effect is CardEffectAddCard) || (x.effect is CardEffectAddMana)) 
                && (card.type == Card.PlayType.Moment || card.type == Card.PlayType.Global))
            {
                res.selectCard = card;
                return res;
            }
        }

        FightController.Shuffle(cards);

        foreach (var card in cards)
        {
            if ( card.manaPrice <= FightController.main.manaCnt)
            {
                res.selectCard = card;

                switch(card.type)
                {
                    case Card.PlayType.TargetHero:
                        res.target = FightController.main.RandomAliveHero();
                        break;
                    case Card.PlayType.TargetMonster:
                        res.target = FightController.main.RandomAliveMonster();
                        break;
                }
                return res;
            }    
        }
        return null;

    }
}

[CreateAssetMenu(fileName = "TestConfig", menuName = "TestConfig/TestConfig", order = -1)]
public class TestConfig : ScriptableObject
{
    [SerializeField]
    public List<Hero> heroesSet;
    [SerializeField]
    public List<EnemySet> enemiesSet;
    [SerializeField]
    public List<Deck> rewardAfter;
    [SerializeField]
    public Deck deck;


    //Несериализуемое поле
    private RunTestResult result;
    private AI ai;

    public void Fight(EnemySet enemySet)
    {
        FightController.main.enemyList = new List<Enemy>();

        foreach (Enemy enemy in enemySet.enemies)
            FightController.main.enemyList.Add(new Enemy(enemy));

        FightController.main.heroList = heroesSet;

        FightController.main.heroesDeck = new Deck(deck);

        FightController.main.StartAIFight();

        AI.TurnDecription turnDecription;
        while (FightController.main.CheckEnd())
        {
            FightController.main.startHeroTurn();
            while ( (turnDecription = ai.turn()) != null && FightController.main.CheckEnd())
            {
                FightController.main.playCard(turnDecription.selectCard, turnDecription.target);
            }

            FightController.main.enemyTurn();
        }
    }

    public RunTestResult play()
    {
        result = new RunTestResult();

        Deck deck = new Deck();

        foreach (Hero hero in heroesSet)
        {
            if (hero != null)
            {
                foreach (Card card in hero.startDeck.cards)
                {
                    deck.cards.Add(card.copy());
                }
            }
        }

        for(int i = 0; i < enemiesSet.Count; ++i)
        {
            Fight(enemiesSet[i]);

            if (FightController.main.isLose())
                break;

            if (rewardAfter.Count > i)
            {
                int next = UnityEngine.Random.Range(0, rewardAfter[i].cards.Count);
                deck.cards.Add(rewardAfter[i].cards[next].copy());
            }
        }

        return result;
    }
}



public class CONFIGURATION_TEST : MonoBehaviour
{
    public TestConfig config;
    [SerializeField]
    public void Test()
    {
        Debug.Log("Start of test");
        config.play();
        Debug.Log("Test is over");
    }
}

public class TestSimulator
{

}
