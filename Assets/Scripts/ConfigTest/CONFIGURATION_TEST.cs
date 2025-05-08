using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        throw new NotImplementedException();
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


    //Несериализуемое поле
    private RunTestResult result;
    private AI ai;

    public void Fight(EnemySet enemySet)
    {
        while (true)
        {
            if (ai.turn() != null)
            {

            }
            //Check End

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
        
    }
}

public class TestSimulator
{

}
