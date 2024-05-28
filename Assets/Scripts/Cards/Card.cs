using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Card
{
	public enum PlayType { TargetAlly, TargetEnemy, Global, TargetAll, Moment };

	public uint manaPrice;
	public Sprite sprite;
	public Sprite ballSprite;
	public string name;
	public string description;
	[SerializeField]
	public List<CardEffectWrapper> effectsList;
    public PlayType type;

	public Card Copy()
	{
        Card card = new()
        {
            type = type,
            name = name,
            description = description,
            effectsList = effectsList,
            sprite = sprite,
            ballSprite = ballSprite,
            manaPrice = manaPrice
        };
        return card;
	}
}

public class CardHolder
{
	public Card card;
}

[System.Serializable]
public struct CardEffectWrapper
{
	public CardEffect effect;
	public uint value;
}

public class CardEffect : ScriptableObject
{
	public virtual void Activate()
	{
		throw new System.NotImplementedException();
	}
	public virtual void Activate(int par)
	{
		throw new System.NotImplementedException();
	}
	public virtual void Activate(Character target, int par)
	{
		throw new System.NotImplementedException();
	}
}
