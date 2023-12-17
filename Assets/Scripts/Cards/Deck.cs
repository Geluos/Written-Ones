using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Decks/Deck", order = -1)]
public class Deck : ScriptableObject
{
	[SerializeField]
	List<Card> cards;
}
