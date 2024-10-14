using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance;
    public List<Card> deckCards = new List<Card>();
    private List<Card> shuffledDeck = new List<Card>();
    private List<Card> discardPile = new List<Card>();

    private void Awake()
    {
        Instance = this; // Singleton pattern for easy access
    }

    // Add cards to the deck
    public void AddCard(Card card)
    {
        deckCards.Add(card);
    }

    public void AddCards(List<Card> cards)
    {
        deckCards.AddRange(cards);
    }

    // Shuffle the deck (Fisher-Yates shuffle algorithm)
    public void ShuffleDeck()
    {
        List<Card> cardsToShuffle = new List<Card>(deckCards);
        shuffledDeck.Clear();
        discardPile.Clear();

        for (int i = 0; i < cardsToShuffle.Count; i++)
        {
            int randomIndex = Random.Range(i, cardsToShuffle.Count);
            Card temp = cardsToShuffle[i];
            cardsToShuffle[i] = cardsToShuffle[randomIndex];
            cardsToShuffle[randomIndex] = temp;
        }

        // Push cards into the stack
        foreach (Card card in cardsToShuffle)
        {
            shuffledDeck.Add(card);
        }
    }
    public void ShuffleDiscardPile()
    {
        List<Card> cardsToShuffle = new List<Card>(discardPile);
        shuffledDeck.Clear();
        discardPile.Clear();

        for (int i = 0; i < cardsToShuffle.Count; i++)
        {
            int randomIndex = Random.Range(i, cardsToShuffle.Count);
            Card temp = cardsToShuffle[i];
            cardsToShuffle[i] = cardsToShuffle[randomIndex];
            cardsToShuffle[randomIndex] = temp;
        }

        // Push cards into the stack
        foreach (Card card in cardsToShuffle)
        {
            shuffledDeck.Add(card);
        }
    }
    // Draw a card from the deck
    public Card DrawCard()
    {
        if (shuffledDeck.Count == 0)
        {
            ResetDeckFromDiscardPile();
        }
        Card c = shuffledDeck[0];
        shuffledDeck.Remove(c);
        return c;
    }

    // Discard a card
    public void DiscardCard(Card card)
    {
        discardPile.Add(card);
    }

    // Reset the deck from the discard pile when the deck is empty
    private void ResetDeckFromDiscardPile()
    {
        //foreach (Card card in discardPile)
        //{
        //    shuffledDeck.Add(card);
        //}
        //discardPile.Clear();
        ShuffleDiscardPile();
    }

    // Add all scriptable cards into the deck
    public void LoadDeckFromScriptableObjects(List<Card> scriptableCards)
    {
        deckCards.Clear();
        deckCards.AddRange(scriptableCards);
        ShuffleDeck();
    }
}
