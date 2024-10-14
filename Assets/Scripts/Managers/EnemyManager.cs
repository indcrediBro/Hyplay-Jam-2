using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<Card> enemyCards;

    private List<Card> commonCards;
    private List<Card> uncommonCards;
    private List<Card> rareCards;
    private List<Card> epicCards;

    private List<Card> attackCards;

    [SerializeField] private List<GameObject> commonEnemies;
    [SerializeField] private List<GameObject> eliteEnemies;
    [SerializeField] private List<GameObject> bossEnemies;

    public void InitializeCards()
    {
        enemyCards = GameDataReferences.Instance.allEnemyCards;
        commonCards = new List<Card>();
        uncommonCards = new List<Card>();
        rareCards = new List<Card>();
        epicCards = new List<Card>();
        attackCards = new List<Card>();

        foreach (Card card in enemyCards)
        {
            if (card.cardRarity == CardRarity.Common)
            {
                commonCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Uncommon)
            {
                uncommonCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Rare)
            {
                rareCards.Add(card);
            }
            if (card.cardRarity == CardRarity.Epic)
            {
                epicCards.Add(card);
            }
            if (card.cardType == CardType.Attack)
            {
                attackCards.Add(card);
            }
        }
    }

    public GameObject GetCommonEnemy() { return commonEnemies[Random.Range(0, commonEnemies.Count)]; }
    public GameObject GetEliteEnemy() { return eliteEnemies[Random.Range(0, eliteEnemies.Count)]; }
    public GameObject GetBossEnemy() { return bossEnemies[Random.Range(0, bossEnemies.Count)]; }

    public List<Card> GetEnemyCards(int stars)
    {
        List<Card> cards = new List<Card>();

        switch (stars)
        {
            case 1:
                // get 3 random common cards //3
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                return cards;
            case 2:
                // get 1 uncommon cards and 2 common cards //3
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Uncommon));

                return cards;
            case 3:
                // get 1 rare and 2 uncommon cards and 2 common cards //5
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Rare));
                return cards;
            case 4:
                // get 2 rare cards and 2 uncommon cards and 2 common cards //5
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Rare));
                cards.Add(GetRandomCard(CardRarity.Rare));
                return cards;
            case 5:
                // get 2 epic cards and 1 rare cards and 1 uncommon cards and 2 common cards //6
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Rare));
                cards.Add(GetRandomCard(CardRarity.Rare));
                cards.Add(GetRandomCard(CardRarity.Epic));
                return cards;
            case 6:
                // get 3 epic cards and 2 rare cards and 3 uncommon cards //8
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Common));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Uncommon));
                cards.Add(GetRandomCard(CardRarity.Rare));
                cards.Add(GetRandomCard(CardRarity.Rare));
                cards.Add(GetRandomCard(CardRarity.Epic));
                cards.Add(GetRandomCard(CardRarity.Epic));
                return cards;
            default:
                return cards;
        }
    }
    public Card GetEnemyAttackCard(int stars)
    {
        Card card;

        switch (stars)
        {
            case 1:
                card = GetAttackCard(CardRarity.Common);
                return card;
            case 2:
                card = GetAttackCard(CardRarity.Uncommon);
                return card;
            case 3:
                card = GetAttackCard(CardRarity.Uncommon);
                return card;
            case 4:
                card = GetAttackCard(CardRarity.Rare);
                return card;
            case 5:
                card = GetAttackCard(CardRarity.Rare);
                return card;
            case 6:
                card = GetAttackCard(CardRarity.Epic);
                return card;
            default:
                return attackCards[0];
        }
    }

    private Card GetAttackCard(CardRarity rarity)
    {
        List<Card> cards = new List<Card>();
        foreach (var c in attackCards)
        {
            if (c.cardRarity == rarity)
            {
                cards.Add(c);
            }
        }
        return cards[Random.Range(0, cards.Count)];
    }
    private Card GetRandomCard(CardRarity rarity)
    {
        Card card;
        switch (rarity)
        {
            case CardRarity.Common:
                card = commonCards[Random.Range(0, commonCards.Count)];
                return card;

            case CardRarity.Uncommon:
                card = uncommonCards[Random.Range(0, uncommonCards.Count)];
                return card;

            case CardRarity.Rare:
                card = rareCards[Random.Range(0, rareCards.Count)];
                return card;

            case CardRarity.Epic:
                card = epicCards[Random.Range(0, epicCards.Count)];
                return card;
            default:
                return null;
        }
    }
}
