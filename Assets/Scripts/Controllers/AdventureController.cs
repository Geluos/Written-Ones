using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;
using static UnityEngine.GraphicsBuffer;

public class AdventureController : Controller<AdventureController>
{
	public Deck pathDeckAct1;
	public Deck pathDeckAct2;
	public Deck pathDeckAct3;
	public GameObject hand;
	public GameObject notificationDialog;
	public GameObject chooseDialog;
    public GameObject ritualDialog;

	public Deck shopDeckFight;
    public Deck shopDeckPath;

    public GameObject shop;
    public GameObject shopFightCard;
	public GameObject shopPathCard;
	public GameObject buyButtonPrefab;
    public Transform[] cardSlots;
    public Button updateButton;
    public const int AVG_MONEY = 20;

    private Deck currentShopDeck;
    private List<Card> shopFightCards;
    private List<Card> shopPathCards;
	private CardsLayout cardsLayout;

	public void Start()
	{
		
	}

	public void ForceStart()
	{
		//LoadPathDeck();

		FightController.main.AdventureScene.gameObject.SetActive(true);
		hand.GetComponent<CardsLayout>().ForceStart();

		shopFightCards = new List<Card>(shopDeckFight.cards);
		shopPathCards = new List<Card>(shopDeckPath.cards);
		InitShopCards();
		LoadShopCards();

		//shop.SetActive(!shop.activeSelf);

		updateButton.GetComponentInChildren<TMP_Text>().text = $"Обновить за {0.3 * AVG_MONEY}";
		updateButton.GetComponentInChildren<TMP_Text>().fontSize = 19;
		updateButton.onClick.AddListener(UdpateShop);
		StartNewAct();
		SoundController.main.PlayAdventureMusic();
	}

	public void StartNewAct()
	{
		if (FightController.main.actNum == 0)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct1);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 1)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct2);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 2)
		{
			cardsLayout = hand.GetComponent<CardsLayout>();
			cardsLayout.Load(pathDeckAct2);
			cardsLayout.FadeIn();
		}
		else if (FightController.main.actNum == 3)
		{
			//WIN
		}
	}

    public bool PlayCard(Card card)
	{
		foreach (var effect in card.effectsList)
			effect.effect.Activate();
		return true;
	}

    //public void Update()
    //{
    //    if (CameraMoveOnClick.showShop) shop.SetActive(true);
    //    else shop.SetActive(false);
    //}

 //   public void LoadPathDeck()
	//{
 //       foreach (var card in pathDeck.cards)
 //       {
 //           var cardObject = Instantiate(pathCard, hand.transform);
 //           cardObject.GetComponent<PathCardScript>().card = card.copy();
 //           cardObject.GetComponent<PathCardScript>().UpdateView();
 //       }
 //   }

    public void InitShopCards()
    {
        currentShopDeck = new Deck();
        int currentPathCardsCount = 0;
        int currentFightCardsCount = 0;

        while (currentFightCardsCount < 8 && shopFightCards.Count > 0)
        {
            int num = Random.Range(1, 11);
            Rarity rarity;
            if (num <= 6) rarity = Rarity.Common;
            else if (num <= 9) rarity = Rarity.Rare;
            else rarity = Rarity.Gold;

            List<Card> cards = shopFightCards.Where(card => card.rarity == rarity).ToList();
            if (cards.Count == 0) continue;

            Card currentCard = cards[Random.Range(0, cards.Count)];
            shopFightCards.Remove(currentCard);
            currentShopDeck.cards.Add(currentCard);
            currentFightCardsCount++;
        }

        while (currentPathCardsCount < 2 && shopPathCards.Count > 0)
        {
            Card currentCard = shopPathCards[Random.Range(0, shopPathCards.Count)];
            currentShopDeck.cards.Add(currentCard);
            shopPathCards.Remove(currentCard);
            currentPathCardsCount++;
        }
    }

    public void LoadShopCards()
    {
        int currentPathCardsCount = currentShopDeck.cards.Where(card => card.otype == OwnerType.Path).Count();
        int currentFightCardsCount = currentShopDeck.cards.Count - currentPathCardsCount;
        List<int> slotIndices = new List<int>();
        for (int i = 0; i < currentShopDeck.cards.Count; i++)
        {
            slotIndices.Add(i);
        }
        for (int i = 0; i < currentFightCardsCount; i++)
        {
            int slotIndex = slotIndices[i];
            var cardObject = Instantiate(shopFightCard, shop.transform);
            cardObject.GetComponent<CardBaseScript>().card = currentShopDeck.cards[i].copy();
            cardObject.transform.localScale = Vector3.one * 0.27f;
            cardObject.transform.position = cardSlots[slotIndex].transform.position;
            cardObject.GetComponent<CardBaseScript>().UpdateView();
            AddBuyButton(cardObject);
            cardObject.AddComponent<CardBuyer>();
        }
        for (int i = currentFightCardsCount; i < currentShopDeck.cards.Count; i++)
        {
            int slotIndex = slotIndices[i];
            var cardObject = Instantiate(shopPathCard, shop.transform);
            cardObject.GetComponent<CardBaseScript>().card = currentShopDeck.cards[i].copy();
            cardObject.transform.localScale = Vector3.one * 0.27f;
            cardObject.transform.position = cardSlots[slotIndex].transform.position;
            cardObject.GetComponent<CardBaseScript>().UpdateView();
            AddBuyButton(cardObject);
            cardObject.AddComponent<CardBuyer>();
        }
    }

    public void UndrawShopCards()
    {
        var pathCards = new List<Card>();
        var fightCards = new List<Card>();
        foreach (var card in FindObjectsOfType<CardBaseScript>())
        {
            if (card.GetComponent<CardBuyer>() == null) continue;
            if (card.card.otype == OwnerType.Path) pathCards.Add(card.card);
            else fightCards.Add(card.card);
            Destroy(card.gameObject);
        }

        shopFightCards.AddRange(fightCards);
        shopPathCards.AddRange(pathCards);
    }

    public void UdpateShop()
    {
        if (GoldController.main.Substract((int)(AVG_MONEY * 0.3)))
        {
            UndrawShopCards();
            InitShopCards();
            LoadShopCards();
        }
    }

    void AddBuyButton(GameObject card)
    {
        GameObject buyButton = Instantiate(buyButtonPrefab, card.transform);
        RectTransform buttonTransform = buyButton.GetComponent<RectTransform>();
        buttonTransform.anchoredPosition = new Vector2(0, -570);
        buttonTransform.localScale = new Vector3(4.3f, 4.3f, 1.0f);

        Rarity rarity = card.GetComponent<CardBaseScript>().card.rarity;
        buyButton.GetComponentInChildren<TMP_Text>().text = $"Купить за {((int)rarity+1) * AVG_MONEY}";
        buyButton.GetComponentInChildren<TMP_Text>().fontSize = 10;
        buyButton.GetComponent<Button>().onClick.AddListener(() => BuyCard(card, ((int)rarity+1) * AVG_MONEY));
    }

    void BuyCard(GameObject cardObj, int price)
    {
        if (GoldController.main.Substract(price))
        {
            var card = cardObj.GetComponent<CardBaseScript>().card;

            if (card.otype == OwnerType.Path)
            {
                cardsLayout = hand.GetComponent<CardsLayout>();
                cardsLayout.deck.cards.Add(card);
                cardsLayout.Load(cardsLayout.deck);

                shopPathCards.Add(card);
            }
            else
            {
                FightController.main.characterDecks.Find(d => d.ownerType == card.otype)
                    .AddCard(card);
                shopFightCards.Add(card);
            }

            Destroy(cardObj);
        }
    }
}
