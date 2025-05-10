using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

[Serializable]
public struct RewardSet
{
    [SerializeField]
    public List<Card> cards;
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
