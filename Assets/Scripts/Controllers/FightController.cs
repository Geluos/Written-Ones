using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FightController : Controller<FightController>
{
	public enum StageType { PlayerTurn, EnemyTurn };

	public StageType stage;
	public uint manaCnt = 0;

	public List<Hero> heroList;
	public List<Enemy> enemyList;

	private Deck heroesDeck;
	public List<Card> cards;
	public List<Card> deckCards;
	public GameObject hand;
	public GameObject baseCard;
	public TMPro.TextMeshProUGUI manaStr;
	public GameObject AdventureScene;
	public GameObject FightScene;
	public GameObject youLose;
	public Decorations decorations;
	public EnemySets enemySets;
	public EnemySets bossSets;
	[HideInInspector]
	public bool isDragCard = false;
	public bool isBossFight = false;
	public RewardsDialogScript rewardsDialog;
	[HideInInspector]
	public int actNum = 0;
	

	public void StartFight()
	{
		decorations.Decorate();
		AdventureScene.SetActive(false);
		FightScene.SetActive(true);
		createDeck();
		CreateMonsters();
		startHeroTurn();
	}

	public void StartBossFight()
	{
		isBossFight = true;
		StartFight();
	}

	private void CreateMonsters()
	{
		var eset = getMonsterSet();

		foreach (var monster in enemyList)
		{
			Destroy(monster.gameObject);
		}

		enemyList.Clear();

		List<GameObject> points = new List<GameObject>();

		if (eset.enemies.Count == 1)
		{
			points = CharacterController.main.oneMonsterPointer;
		}
		else if (eset.enemies.Count == 2)
		{
			points = CharacterController.main.twoMonsterPointers;
		}
		else if (eset.enemies.Count == 3)
		{
			points = CharacterController.main.threeMonsterPointers;
		}

		for(int i=0; i<eset.enemies.Count; ++i)
		{
			var monster = Instantiate(eset.enemies[i], points[i].transform);
			monster.transform.localPosition = new Vector3(0f, 0f, 0f);
			enemyList.Add(monster);
		}
	}

	private ESet getMonsterSet()
	{
		if (isBossFight)
			return bossSets.enemySets[actNum];
		return enemySets.enemySets[UnityEngine.Random.Range(0, enemySets.enemySets.Count)];
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

	private void createDeck()
	{
		heroesDeck = Deck.CreateInstance<Deck>();
		foreach (var hero in heroList)
		{
			foreach(var card in hero.startDeck.cards)
			{
				var cardCopy = card.copy();
				cardCopy.owner = hero;
				cardCopy.setOwnerForEffects();
				heroesDeck.cards.Add(cardCopy);
			}
		}
	}

	//private void startFight()
	//{
	//	deckCards = new List<Card>();
	//	foreach (Card card in heroesDeck.cards)
	//		deckCards.Add(card.copy());
	//	Shuffle(deckCards);
	//}

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
		manaStr.text = 0.ToString();
		manaCnt = 0;

		for (int i = 0; i < 6; ++i)
		{
			var card = getCard();
			cards.Add(card);
		}

		//ADD CARDS TO HAND
		//foreach (Card card in cards)
		//{
		//	//var cardObject = Instantiate(baseCard, hand.transform);
		//	//cardObject.GetComponent<CardBaseScript>().card = card.copy();
		//	//cardObject.GetComponent<CardBaseScript>().UpdateView();
		//}

		var cardsLayout = hand.GetComponent<CardsLayout>();
		Deck DeckCards = Deck.CreateInstance<Deck>();
		DeckCards.cards = cards;
		cardsLayout.Load(DeckCards);
		cardsLayout.FadeIn();


		foreach (Enemy enemy in enemyList)
		{
			EnemyGetCard(enemy);
		}

		//PlayMomentalCards();
	}

	private void UpdateUI()
	{
		manaStr.text = manaCnt.ToString();
	}

	public void addMana(uint value)
	{
		manaCnt += value;
		UpdateUI();
	}

	public void addCard(uint value)
	{
		for (int i = 0; i < value; ++i)
		{

			var card = getCard();
			cards.Add(card);

			var cardObject = Instantiate(baseCard, hand.transform);
			var cardLayout = hand.GetComponent<CardsLayout>();
			cardLayout.cardInstances.Add(cardObject);
			cardObject.GetComponent<CardBaseScript>().card = card.copy();
			cardObject.GetComponent<CardBaseScript>().UpdateView();
		}
	}

	private void checkLose()
	{
		bool flag = true;
		foreach (var hero in heroList)
		{
			if (hero.isAlive())
				flag = false;
		}
		if (flag)
			youLose.SetActive(true);
	}

	public void DamageAllEnemies(uint value)
	{
		foreach(var enemy in enemyList)
		{
			enemy.getDamage(value);
		}
		checkWin();
	}

	public void DamageEnemy(Character enemy, uint value)
	{
		enemy.getDamage(value);
		checkWin();
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

	private void checkWin()
	{
		bool hasAlive = false;
		foreach (var enemy in enemyList)
		{
			if (enemy.isAlive())
				hasAlive = true;
		}
		if (!hasAlive)
		{
			if (isBossFight)
			{
				++actNum;
				AdventureController.main.StartNewAct();
				foreach (var hero in heroList)
				{
					HealHp(hero, 1000);
				}
				isBossFight = false;
			}

			
			rewardsDialog.GiveReward();
			AdventureScene.SetActive(true);
			FightScene.SetActive(false);
		}
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

		checkLose();
	}

	public void DamageAllHeroes(uint value)
	{
		foreach(var hero in heroList)
		{
			hero.getDamage(value);
		}

		checkLose();
	}

	public void minusMana(uint value)
	{
		if (value > manaCnt)
			manaCnt = 0;
		else
			manaCnt -= value;
		UpdateUI();
	}

	public bool playCard(Card card, GameObject target = null)
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

			return true;
		}

		return false;
	}

	//HACK
	/*private void PlayMomentalCards()
	{

		var children = new List<GameObject>();
		foreach (Transform child in hand.transform)
		{
			var card = child.gameObject.GetComponent<CardBaseScript>().card;

			if (card.type != Card.PlayType.Moment)
				continue;

			child.gameObject.GetComponent<CardBaseScript>().CardActivate();
		}
		children.ForEach(child => Destroy(child));
	}*/

	private void clearCards()
	{
		cards.Clear();
		var children = new List<GameObject>();
		foreach (Transform child in hand.transform)
			children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		var cardsLayout = hand.GetComponent<CardsLayout>();
		cardsLayout.cardInstances.Clear();

	}

	public void endHeroTurn()
	{
		enemyTurn();
		startHeroTurn();
	}

	private void EnemyGetCard(Enemy enemy)
	{
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

	private Hero RandomAliveHero()
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

	private Enemy RandomAliveMonster()
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
}
