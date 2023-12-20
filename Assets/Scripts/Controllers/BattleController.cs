using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : Controller<BattleController>
{
	public enum StageType { PlayerTurn, EnemyTurn };

	public StageType stage;
	public uint manaCnt;

	public List<Hero> heroList;
	public List<Enemy> enemyList;

	public List<Card> cards;
	public GameObject hand;

	public void Start()
	{
		startHeroTurn();
	}

	public void startHeroTurn()
	{
		clearCards();

		foreach (var hero in heroList)
		{
			//TODO Replace 3 with param
			for (int i = 0; i < 3; ++i)
			{
				var card = hero.getCard();
				cards.Add(card);
			}
		}

		//ADD CARDS TO HAND
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

	}
}
