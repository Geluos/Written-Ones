using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Card
{
	public enum PlayType { TargetAlly, TargetEnemy, Global, TargetAll, Moment };
	public enum Owner { RedHead, Piper, TinWoodpeaker, Path };
	public enum Rarity { Common, Rare, Gold };

	public uint manaPrice;
	public Sprite sprite;
	public Sprite ballSprite;
	public string name;
	public string description;
	[SerializeField]
	public List<CardEffect> effectsList;
    public PlayType type;
	public Owner owner;
	public Rarity rarity;

	public Card copy()
	{
		Card card = new Card();
		card.type = type;
		card.name = name;
		card.description = description;
		card.effectsList = effectsList;
		card.sprite = sprite;
		card.ballSprite = ballSprite;
		card.manaPrice = manaPrice;
		card.type = type;
		card.owner = owner;
		card.rarity = rarity;
		return card;
	}
}

public abstract class CardEffect : ScriptableObject
{
	public abstract void Activate();
}
