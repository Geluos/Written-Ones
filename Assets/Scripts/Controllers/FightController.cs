using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static UnityEngine.UI.GridLayoutGroup;


public struct CharacterDeck
{
	public Card.OwnerType ownerType;
	public Hero owner;
	public Deck deck;

	public CharacterDeck(Deck deck, Hero owner)
	{
		this.owner = owner;
		this.ownerType = owner.ownerTypeForCharacter;
		this.deck = Deck.CreateInstance<Deck>();

		foreach (var card in deck.cards)
			AddCard(card);
	}

	public readonly void AddCard(Card card)
	{
        var cardCopy = card.copy();
        cardCopy.owner = owner;
        cardCopy.setOwnerForEffects();
        this.deck.cards.Add(cardCopy);
    }
}
public class FightController : Controller<FightController>
{
	public enum StageType { PlayerTurn, EnemyTurn };

	public StageType stage;
	public uint manaCnt = 0;

	public List<Hero> heroList;
	public List<Enemy> enemyList;

	//FullDeck
	[HideInInspector]
	public Deck heroesDeck;
	//Hand
	public List<Card> cards;
	//Dobor
	public List<Card> deckCards;
	public EnemySets enemySets;

    [HideInInspector]
    public List<CharacterDeck> characterDecks;

	public void Start()
	{
		//StartCreateDecks();
	}

	private void StartCreateDecks()
	{
		//characterDecks = new List<CharacterDeck>();
		//foreach (var hero in heroList)
		//{
		//	characterDecks.Add(new CharacterDeck(hero.startDeck, hero));
		//}
	}

	public void StartFight()
	{
	//	foreach (var hero in heroList)
	//	{
	//		if (hero.current_hp <= 0)
	//			hero.current_hp = 1;
	//		hero.gameObject.SetActive(true);
	//	}
	//	startHeroTurn();
	}

    public void StartAIFight()
    {
        //Debug.Log("StartAIFight");
        foreach (var hero in heroList)
        {
            if (hero.current_hp <= 0)
                hero.current_hp = 1;
        }
        deckCards = new List<Card>();
        //startHeroTurn();
    }

    public void restorePartyHp(float percent)
	{
		foreach(var hero in heroList)
		{
			hero.current_hp = Math.Min(hero.max_hp, hero.current_hp + (uint)Math.Round(hero.max_hp * percent / 100));
		}
	}

	//TO UTILS
	public static void Shuffle<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			var temp = list[i];
			int randomIndex = UnityEngine.Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}

	public Card getCard()
	{
		Card res;
		if (deckCards.Count <= 0)
		{
			deckCards = new List<Card>();
			foreach (Card card in heroesDeck.cards)
				deckCards.Add(card.copy());
			Shuffle<Card>(deckCards);
		}

		res = deckCards[0];
		deckCards.RemoveAt(0);
		return res;
	}

	public void startHeroTurn()
	{
		clearCards();
		manaCnt = 0;

		foreach (var hero in heroList)
		{
			if (hero.ownerTypeForCharacter == Card.OwnerType.TinWoodpeaker)
				hero.shield /= 2;
			hero.shield = 0;
		}

		for (int i = 0; i < 6; ++i)
		{
			var card = getCard();
			cards.Add(card);
		}


		foreach (Enemy enemy in enemyList)
		{
			EnemyGetCard(enemy);
		}

	}

	public void addMana(uint value)
	{
		manaCnt += value;
	}

	public void addCard(uint value)
	{
		for (int i = 0; i < value; ++i)
		{

			var card = getCard();
			cards.Add(card);
		}
	}

	public void DamageAllEnemies(uint value)
	{
		foreach(var enemy in enemyList)
		{
			enemy.getDamage(value);
		}
	}

	public void DamageEnemy(Character enemy, uint value)
	{
		enemy.getDamage(value);
	}

	public void SetEffect(Character enemy, CharacterEffect effect, int value)
	{
		enemy.setEffect(effect, value);
	}

	public void AddShield(Character hero, uint value)
	{
		hero.shield += value;
	}

	public void AddShieldAllHeroes(uint value)
	{
		foreach(var hero in heroList)
			hero.shield += value;
	}

	public void AddShieldAllEnemies(uint value)
	{
		foreach (var enemy in enemyList)
			enemy.shield += value;
	}

	public void HealHp(Character hero, uint value)
	{
		hero.current_hp = Math.Min(hero.max_hp, hero.current_hp + value);
	}

	public void DamageRandomHero(uint value)
	{
		int next = UnityEngine.Random.Range(1, 10);

		var hero = heroList[0];
		for(int i = 0; i < next; ++i)
		{
			if (heroList[i%3].isAlive())
				hero = heroList[i%3];
		}

		hero.getDamage(value);

	}

	public void DamageAllHeroes(uint value)
	{
		foreach(var hero in heroList)
		{
			hero.getDamage(value);
		}
	}

	public void minusMana(uint value)
	{
		if (value > manaCnt)
			manaCnt = 0;
		else
			manaCnt -= value;
	}

	public bool playCard(Card card, Character target = null)
	{
		if (card.manaPrice <= manaCnt)
		{
			switch(card.type)
			{
				case Card.PlayType.TargetAll:
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate((int)effect.value);
					}
					break;
				case Card.PlayType.Global:
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate((int)effect.value);
					}
					break;
				case Card.PlayType.Moment:
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate((int)effect.value);
					}
					break;
				case Card.PlayType.TargetMonster:
					if (target == null || target.GetComponent<Enemy>() == null)
						return false;
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate(target.GetComponent<Enemy>(), (int)effect.value);
					}
					break;
				case Card.PlayType.TargetHero:
					if (target == null || target.GetComponent<Hero>() == null)
						return false;
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate(target.GetComponent<Hero>(), (int)effect.value);
					}
					break;
			}
			minusMana(card.manaPrice);
            cards.Remove(card);

            return true;
		}

		return false;
	}


	private void clearCards()
	{
		cards.Clear();
	}

	public void endHeroTurn()
	{
		enemyTurn();
		foreach (var enemy in enemyList)
		{
			enemy.popEffect(CharacterEffect.weakness);
		}
		startHeroTurn();
	}

	private void EnemyGetCard(Enemy enemy)
	{
		if (enemy.nextCard == null)
			enemy.nextCard = new CardHolder();
		enemy.nextCard.card = enemy.getCard();
		enemy.nextCard.card.owner = enemy;
		enemy.nextCard.card.setOwnerForEffects();

		if (enemy.nextCard.card.type == Card.PlayType.TargetHero)
		{
			enemy.nextTarget = RandomAliveHero();
		}

		if (enemy.nextCard.card.type == Card.PlayType.TargetMonster)
		{
			enemy.nextTarget = RandomAliveMonster();
		}
	}

	public Hero RandomAliveHero()
	{
		bool flag = false;
		foreach (var hero in heroList)
		{
			if (hero.current_hp > 0)
				flag = true;
		}

		if (!flag)
			return null;

		int t = UnityEngine.Random.Range(1, 15);

		int tar = 0;
		while (t>0)
		{

			tar = (tar + 1) % heroList.Count;
			if (heroList[tar].current_hp > 0)
			{
				--t;
			}

		}

		return heroList[tar];
	}

	public Enemy RandomAliveMonster()
	{
		bool flag = false;
		foreach (var enemy in enemyList)
		{
			if (enemy.current_hp > 0)
				flag = true;
		}

		if (!flag)
			return null;

		int t = UnityEngine.Random.Range(1, 15);

		int tar = 0;
		while (t > 0)
		{

			tar = (tar + 1) % enemyList.Count;
			if (enemyList[tar].current_hp > 0)
			{
				--t;
			}

		}

		return enemyList[tar];
	}

	public void enemyTurn()
	{
		foreach(var enemy in enemyList)
		{
			if (!enemy.isAlive())
			{
				continue;
			}

			enemy.shield = 0;
			if (enemy.nextCard == null)
			{
				EnemyGetCard(enemy);
			}

			foreach(var effect in enemy.nextCard.card.effectsList)
			{
				if (enemy.nextCard.card.type == Card.PlayType.TargetHero
					|| enemy.nextCard.card.type == Card.PlayType.TargetAll
					|| enemy.nextCard.card.type == Card.PlayType.TargetMonster)
					effect.effect.Activate(enemy.nextTarget, (int)effect.value);
				else
					effect.effect.Activate((int)effect.value);
			}

			enemy.nextTarget = null;
		}
	}

    public bool CheckEnd()
    {

		return isWin() || isLose();
    }

	public bool isWin()
	{
        bool hasAlive = false;
        foreach (var enemy in enemyList)
        {
            if (enemy.isAlive())
                hasAlive = true;
        }
		return !hasAlive;
    }

    public bool isLose()
    {
        bool hasAlive = false;
        foreach (var hero in heroList)
        {
            if (hero.isAlive())
                hasAlive = true;
        }
        return !hasAlive;
    }
}
