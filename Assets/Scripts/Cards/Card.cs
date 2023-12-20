using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Card
{
	public enum PlayType { TargetAlly, TargetEnemy, Global, TargetAll, Moment };

	public uint manaPrice;
	public Sprite sprite;
	public string name;
	public string description;
	[SerializeField]
	public List<CardEffect> effectsList;
	public PlayType type;

	public Card copy()
	{
		Card card = new Card();
		card.type = type;
		card.name = name;
		card.description = description;
		card.effectsList = effectsList;
		card.sprite = sprite;
		card.manaPrice = manaPrice;
		return card;
	}
}

public abstract class CardEffect : MonoBehaviour
{
	protected abstract void Activate();
}
