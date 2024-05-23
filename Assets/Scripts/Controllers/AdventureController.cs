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

	public Deck shopDeckFight;
    public Deck shopDeckPath;

    private Deck currentShopDeck;
	public GameObject shop;
	public GameObject shopFightCard;
    public Transform[] cardSlots;

    public void Start()
	{
		LoadPathDeck();
        
        InitShopCards();
		LoadShopCards();
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

    public void InitShopCards()
    {
        currentShopDeck = new Deck();
        for (int i = 0; i < 8; i++)
        {
            currentShopDeck.cards.Add(shopDeckFight.cards[i]);
        }
        for (int i = 0; i < 2; i++)
        {
            currentShopDeck.cards.Add(shopDeckPath.cards[i]);
        }
    }

    public void LoadShopCards()
    {
        List<int> slotIndices = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            slotIndices.Add(i);
        }
        for (int i = 0; i < 8; i++)
        {
            int slotIndex = slotIndices[i];
            var cardObject = Instantiate(shopFightCard, shop.transform);
            cardObject.GetComponent<CardBaseScript>().card = currentShopDeck.cards[i].copy();
            cardObject.transform.localScale = Vector3.one * 0.27f;
            cardObject.transform.position = cardSlots[slotIndex].transform.position;
            cardObject.GetComponent<CardBaseScript>().UpdateView();
            cardObject.AddComponent<CardBuyer>();
        }
        for (int i = 8; i < 10; i++)
        {
            int slotIndex = slotIndices[i];
            var cardObject = Instantiate(pathCard, shop.transform);
            cardObject.GetComponent<CardBaseScript>().card = currentShopDeck.cards[i].copy();
            cardObject.transform.localScale = Vector3.one * 0.27f;
            cardObject.transform.position = cardSlots[slotIndex].transform.position;
            cardObject.GetComponent<CardBaseScript>().UpdateView();
            cardObject.AddComponent<CardBuyer>();
        }
    }

    public bool PlayCard(Card card)
    {
        return true;
    }
}
