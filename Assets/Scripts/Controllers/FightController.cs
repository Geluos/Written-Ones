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

	public List<Card> cards;
	public GameObject hand;
	public GameObject baseCard;
	public TMPro.TextMeshProUGUI manaStr;
	public GameObject youWin;
	public GameObject youLose;

	public void Start()
	{
		startHeroTurn();
	}

	public void startHeroTurn()
	{
		clearCards();
		manaStr.text = 0.ToString();
		manaCnt = 0;

		foreach (var hero in heroList)
		{
			if (hero.isAlive())
			{
				//TODO Replace 3 with param
				for (int i = 0; i < 3; ++i)
				{
					var card = hero.getCard();
					cards.Add(card);
				}
			}
		}

		//ADD CARDS TO HAND
		foreach (Card card in cards)
		{
			var cardObject = Instantiate(baseCard, hand.transform);
			cardObject.GetComponent<CardBaseScript>().card = card.copy();
			cardObject.GetComponent<CardBaseScript>().UpdateView();
		}

		PlayMomentalCards();
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

	public void DamageEnemy(uint value)
	{
		enemyList[0].getDamage(value);

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

	public bool playCard(Card card)
	{
		if (card.manaPrice <= manaCnt)
		{
			minusMana(card.manaPrice);
			foreach(var effect in card.effectsList)
			{
				effect.Activate();
			}

			return true;
		}

		return false;
	}

	//HACK
	private void PlayMomentalCards()
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
	}

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
				effect.Activate();
			}
		}
	}
}
