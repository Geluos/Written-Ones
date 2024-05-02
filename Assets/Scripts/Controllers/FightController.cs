using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public GameObject youWin;
	public GameObject youLose;

	public void Start()
	{
		createDeck();
		startHeroTurn();
	}

	//TO UTILS
	public static void Shuffle<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			var temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}

	private void createDeck()
	{
		heroesDeck = new Deck();
		foreach (var hero in heroList)
		{
			foreach(var card in hero.startDeck.cards)
			{
				heroesDeck.cards.Add(card.copy());
			}
		}
	}

	private void startFight()
	{
		deckCards = new List<Card>();
		foreach (Card card in heroesDeck.cards)
			deckCards.Add(card.copy());
		Shuffle(deckCards);
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
		manaStr.text = 0.ToString();
		manaCnt = 0;

		for (int i = 0; i < 6; ++i)
		{
			var card = getCard();
			cards.Add(card);
		}

		//ADD CARDS TO HAND
		foreach (Card card in cards)
		{
			var cardObject = Instantiate(baseCard, hand.transform);
			cardObject.GetComponent<CardBaseScript>().card = card.copy();
			cardObject.GetComponent<CardBaseScript>().UpdateView();
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

	private void checkWin()
	{
		if (!enemyList[0].isAlive())
		{
			youWin.SetActive(true);
		}
	}

	public void DamageRandomHero(uint value)
	{
		int next = Random.Range(1, 10);

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
				case Card.PlayType.TargetEnemy:
					if (target == null || target.GetComponent<Enemy>() == null)
						return false;
					foreach (var effect in card.effectsList)
					{
						effect.effect.Activate(target.GetComponent<Enemy>(), (int)effect.value);
					}
					break;
				case Card.PlayType.TargetAlly:
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

	}

	public void endHeroTurn()
	{
		enemyTurn();
		startHeroTurn();
	}

	public void enemyTurn()
	{
		foreach(var enemy in enemyList)
		{
			var card = enemy.getCard();
			foreach(var effect in card.effectsList)
			{
				effect.effect.Activate((int)effect.value);
			}
		}
	}
}
