using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Card
{
	public enum PlayType { TargetHero, TargetMonster, Global, TargetAll, Moment };
	public enum OwnerType { RedHead, Piper, TinWoodpeaker, Path, Other };
	public enum Rarity { Common, Rare, Gold };

	public uint manaPrice;
	public Sprite sprite;
    public Sprite crystalSprite;
    public Sprite ballSprite;
	public string name;
	public string description;
	[SerializeField]
	public List<CardEffectWrapper> effectsList;
    public PlayType type;
	public OwnerType otype;
	public Rarity rarity;
	[HideInInspector]
	public Character owner;

	public Card copy()
	{
		Card card = new Card();
		card.type = type;
		card.name = name;
		card.description = description;
		card.effectsList = new List<CardEffectWrapper>();
		for (int i = 0; i < effectsList.Count; i++)
		{
			card.effectsList.Add(effectsList[i].copy());
		}
		card.sprite = sprite;
        card.crystalSprite = crystalSprite;
        card.ballSprite = ballSprite;
		card.manaPrice = manaPrice;
		card.type = type;
		card.otype = otype;
		card.rarity = rarity;
		card.owner = owner;
		return card;
	}

	public void setOwnerForEffects()
	{
		for (int i = 0; i < effectsList.Count; i++)
			effectsList[i].effect.owner = owner;
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

	public CardEffectWrapper copy()
	{
		var wrapper = new CardEffectWrapper();
		wrapper.effect = effect.copy();
		wrapper.value = value;

		return wrapper;
	}
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
	public virtual CardEffect copy()
	{
		return new CardEffect();
	}
	//�� ������������� ��������������� ����������
	[HideInInspector]
	public Character owner;
}
