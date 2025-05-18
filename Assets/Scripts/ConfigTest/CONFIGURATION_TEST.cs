using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static AI;
using static UnityEngine.GraphicsBuffer;


public class ResultsOfConfiguration : ScriptableObject
{
    [SerializeField]
    public List<RunTestResult> results;
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

        //For shield cards
        List<Hero> underAttack = new List<Hero>();
        foreach (var hero in FightController.main.heroList)
        {
            foreach (var enemy in FightController.main.enemyList)
            {
                if (enemy.isAlive()
                    && enemy.nextCard != null
                    && (enemy.nextCard.card.effectsList.Exists(x => (x.effect is CardEffectDamageAllHeroes))
                    || (enemy.nextCard.card.effectsList.Exists(x => (x.effect is CardEffectDamageEnemy)) && enemy.nextTarget == hero)))
                {
                    underAttack.Add(hero);
                }
            }
        }

        foreach (var card in cards)
        {
            if (card.manaPrice <= FightController.main.manaCnt)
            {
                res.selectCard = card;

                switch (card.type)
                {
                    case Card.PlayType.TargetHero:
                        if (card.effectsList.Exists(x => (x.effect is CardEffectShieldTarget)) && underAttack.Count > 0)
                        {
                            int next = UnityEngine.Random.Range(0, underAttack.Count);
                            res.target = underAttack[next];
                        }
                        else
                        {
                            res.target = FightController.main.RandomAliveHero();
                        }
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
    public virtual void Test()
    {

        Debug.Log("Start of test");
        config.init();
        double wins = 0;
        int N = 100;
        for (int i = 0; i < N; ++i)
        {
            var result = config.play();
            if (result.isWin)
                wins++;
        }
        Debug.Log("Win percent:" + (wins * 100.0 / N).ToString());
        AssetDatabase.CreateAsset(config.resultsOfConfiguration, "Assets/TestConfig/Results/result.asset");
        Debug.Log("Test is over");
    }


   
}
