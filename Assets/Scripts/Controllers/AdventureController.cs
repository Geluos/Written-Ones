using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AdventureController : Controller<AdventureController>
{
	public Deck pathDeck;
	public GameObject hand;
	public GameObject pathCard;
    public GameObject notificationDialog;
    public GameObject chooseDialog;

    public void Start()
	{
		LoadPathDeck();
	}

	public void LoadPathDeck()
	{
        foreach (var card in pathDeck.cards)
        {
            var cardObject = Instantiate(pathCard, hand.transform);
            cardObject.GetComponent<PathCardScript>().card = card.copy();
            cardObject.GetComponent<PathCardScript>().UpdateView();
        }
    }

    public bool PlayCard(Card card)
    {
        foreach (var effect in card.effectsList)
            effect.Activate();
        return true;
    }
}
