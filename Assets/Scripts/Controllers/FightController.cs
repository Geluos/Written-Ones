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

	private Deck heroesDeck;
	public List<Card> cards;
	public List<Card> deckCards;
	public GameObject hand;
	public GameObject baseCard;
	public TMPro.TextMeshProUGUI manaStr;
	public GameObject AdventureScene;
	public GameObject FightScene;
	public GameObject youLose;
	public List<Decorations> decorations;
	public EnemySets enemySets;
	public EnemySets bossSets;
	[HideInInspector]
	public bool isDragCard = false;
	public bool isBossFight = false;
	public RewardsDialogScript rewardsDialog;
	[HideInInspector]
	public int actNum = 0;

	public List<CharacterDeck> characterDecks;

	public void Start()
	{
		StartCreateDecks();
	}

	private void StartCreateDecks()
	{
		characterDecks = new List<CharacterDeck>();
		foreach (var hero in heroList)
		{
			characterDecks.Add(new CharacterDeck(hero.startDeck, hero));
		}
	}

	public void StartFight()
	{
		foreach (var dec in decorations)
			dec.gameObject.SetActive(false);
		decorations[actNum].gameObject.SetActive(true);
		SoundController.main.PlayBattleMusic();
		foreach (var hero in heroList)
		{
			if (hero.current_hp <= 0)
				hero.current_hp = 1;
			hero.gameObject.SetActive(true);
		}
		decorations[actNum].Decorate();
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

		switch (actNum)
		{
			case 0:
				var sets = enemySets.enemySets.Where(s => s.cost <= 30).ToList();
				return sets[UnityEngine.Random.Range(0, sets.Count)];
			case 1:
				var sets2 = enemySets.enemySets.Where(s => s.cost >= 30 && s.cost <= 45).ToList();
				return sets2[UnityEngine.Random.Range(0, sets2.Count)];
			case 2:
				var sets3 = enemySets.enemySets.Where(s => s.cost >= 50).ToList();
				return sets3[UnityEngine.Random.Range(0, sets3.Count)];
		}
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
		foreach (var charDeck in characterDecks)
		{
			foreach(var card in charDeck.deck.cards)
			{
				var cardCopy = card.copy();
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
			else
				hero.gameObject.SetActive(false);
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
		checkLose();
		checkWin();
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

	private void checkWin()
	{
        bool hasAlive = false;
		foreach (var enemy in enemyList)
		{
			enemy.DestroyTargetIcon();
			if (enemy.isAlive())
				hasAlive = true;
			else
				enemy.gameObject.SetActive(false);
		}
		if (!hasAlive)
		{


			rewardsDialog.GiveReward();
			AdventureScene.SetActive(true);
			FightScene.SetActive(false);

			if (isBossFight)
			{
				++actNum;
				if (actNum == 3)
				{
					SoundController.main.PlayAdventureMusic();
					MenuController.main.StartEndGame();
					return;
				}
				AdventureController.main.StartNewAct();
				foreach (var hero in heroList)
				{
					HealHp(hero, 1000);
				}
				isBossFight = false;
			}
			SoundController.main.PlayAdventureMusic();
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
			switch (card.otype)
			{
				case Card.OwnerType.RedHead:
					SoundController.main.PlaySound(SoundController.main.KnifeSound);
					break;
				case Card.OwnerType.TinWoodpeaker:
					SoundController.main.PlaySound(SoundController.main.AxeSound);
					break;
				case Card.OwnerType.Piper:
					SoundController.main.PlaySound(SoundController.main.FluteSound);
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
		foreach (var enemy in enemyList)
		{
			enemy.popEffect(CharacterEffect.weakness);
		}
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
		SoundController.main.PlaySound(SoundController.main.MonsterSound);
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
}
