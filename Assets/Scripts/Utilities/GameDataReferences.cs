using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class GameDataReferences : Singleton<GameDataReferences>
{
    public bool continueGame;
    public GameObject playerPrefab;
    public Transform playerSpawn, enemySpawn;
    public GameObject mapCanvas;
    public Transform bulletParent;
    public List<GameObject> bulletImages;
    public GameObject rewardsCanvas, scoreCanvas, gameoverCanvas;
    public List<RewardCardUI> rewardCardUIs;

    public Card selectedRewardCard;

    public ArcLayoutGroup cardSpawnParent;
    public List<Card> allCards;
    public List<Card> initialDeck;
    public List<Color> cardColorTints;

    public List<Sprite> allEnemySprites;
    public Transform cardSelectionParent;

    public GameObject cardPrefab;
    public List<Card> allEnemyCards;

    private List<Card> commonPlayerCards;
    private List<Card> uncommonPlayerCards;
    private List<Card> rarePlayerCards;
    private List<Card> epicPlayerCards;

    private void Start()
    {
        InitializeCards();
        EnemyManager.Instance.InitializeCards();
    }

    private void InitializeCards()
    {
        commonPlayerCards = new List<Card>();
        uncommonPlayerCards = new List<Card>();
        rarePlayerCards = new List<Card>();
        epicPlayerCards = new List<Card>();

        foreach (var card in allCards)
        {
            if (card.cardRarity == CardRarity.Common)
            {
                commonPlayerCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Uncommon)
            {
                uncommonPlayerCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Rare)
            {
                rarePlayerCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Epic)
            {
                epicPlayerCards.Add(card);
            }
        }

        initialDeck.Add(GetRandomCard(CardRarity.Common));
    }

    public List<Card> GetRewardCards(int stars)
    {
        List<Card> rewardCards = new List<Card>();

        // Weighted probabilities based on star rating
        Dictionary<CardRarity, float> rarityWeights = GetRarityWeights(stars);

        // Determine how many cards to reward
        int rewardCount = 3;

        // Randomly choose cards based on weighted rarity chances
        for (int i = 0; i < rewardCount; i++)
        {
            CardRarity selectedRarity = GetRandomRarity(rarityWeights);
            Card randomCard = GetRandomCard(selectedRarity);
            rewardCards.Add(randomCard);
        }

        // Shuffle the list to add more variety and randomness
        rewardCards = ShuffleCards(rewardCards);

        return rewardCards;
    }

    // Helper method to get weighted rarity chances based on stars
    private Dictionary<CardRarity, float> GetRarityWeights(int stars)
    {
        Dictionary<CardRarity, float> weights = new Dictionary<CardRarity, float>();

        switch (stars)
        {
            case 1:
                weights[CardRarity.Common] = 80f;
                weights[CardRarity.Uncommon] = 15f;
                weights[CardRarity.Rare] = 5f;
                weights[CardRarity.Epic] = 0f;
                break;
            case 2:
                weights[CardRarity.Common] = 70f;
                weights[CardRarity.Uncommon] = 20f;
                weights[CardRarity.Rare] = 10f;
                weights[CardRarity.Epic] = 0f;
                break;
            case 3:
                weights[CardRarity.Common] = 60f;
                weights[CardRarity.Uncommon] = 25f;
                weights[CardRarity.Rare] = 10f;
                weights[CardRarity.Epic] = 5f;
                break;
            case 4:
                weights[CardRarity.Common] = 50f;
                weights[CardRarity.Uncommon] = 30f;
                weights[CardRarity.Rare] = 15f;
                weights[CardRarity.Epic] = 5f;
                break;
            case 5:
                weights[CardRarity.Common] = 40f;
                weights[CardRarity.Uncommon] = 30f;
                weights[CardRarity.Rare] = 20f;
                weights[CardRarity.Epic] = 10f;
                break;
            case 6:
                weights[CardRarity.Common] = 30f;
                weights[CardRarity.Uncommon] = 30f;
                weights[CardRarity.Rare] = 25f;
                weights[CardRarity.Epic] = 15f;
                break;
        }

        return weights;
    }

    // Method to get a random rarity based on weighted chances
    private CardRarity GetRandomRarity(Dictionary<CardRarity, float> rarityWeights)
    {
        float totalWeight = 0;
        foreach (var weight in rarityWeights.Values)
            totalWeight += weight;

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (var rarity in rarityWeights)
        {
            cumulativeWeight += rarity.Value;
            if (randomValue < cumulativeWeight)
            {
                return rarity.Key;
            }
        }

        // Default to Common if no rarity selected (shouldn't happen)
        return CardRarity.Common;
    }

    // Helper method to shuffle the reward cards for added randomness
    private List<Card> ShuffleCards(List<Card> cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
        return cards;
    }


    private Card GetRandomCard(CardRarity rarity)
    {
        Card card;
        switch (rarity)
        {
            case CardRarity.Common:
                card = commonPlayerCards[Random.Range(0, commonPlayerCards.Count)];
                break;
            case CardRarity.Uncommon:
                card = uncommonPlayerCards[Random.Range(0, uncommonPlayerCards.Count)];
                break;
            case CardRarity.Rare:
                card = rarePlayerCards[Random.Range(0, rarePlayerCards.Count)];
                break;
            case CardRarity.Epic:
                card = epicPlayerCards[Random.Range(0, epicPlayerCards.Count)];
                break;
            default:
                card = null;
                break;
        }
        return card;
    }

    public void SetUpRewardCards(List<Card> _cards)
    {
        for (int i = 0; i < rewardCardUIs.Count; i++)
        {
            rewardCardUIs[i].SetupCard(_cards[i]);
        }
    }

    public void SelectRewardCard(RewardCardUI _rewardCard)
    {
        foreach (var rc in rewardCardUIs)
        {
            if (rc == _rewardCard)
            {
                rc.transform.DOScale(1.75f, .25f);
                selectedRewardCard = rc.card;
            }
            else
            {
                rc.transform.DOScale(1.5f, .25f);
            }
        }
    }

    public void AddRewardCardToDeck()
    {
        GameController.Instance.player.playerDeck.AddCard(selectedRewardCard);
        rewardsCanvas.SetActive(false);
        mapCanvas.SetActive(true);
        selectedRewardCard = null;
        Destroy(GameController.Instance.enemy.gameObject);
    }
}