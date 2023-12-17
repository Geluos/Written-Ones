using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Card
{
	public enum PlayType { TARGET_ALLY, TARGET_ENEMY, GLOBAL, TARGET_ALL};

	public uint manaPrice;
	public Sprite sprite;
	public string name;
	public string description;
	[SerializeField]
	public List<CardEffect> effectsList;
	public PlayType type;
}

public abstract class CardEffect : MonoBehaviour
{
	protected abstract void activate();
}
