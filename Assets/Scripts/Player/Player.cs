using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : Character
{
    public int Energy;
    public int MaxEnergy;
    public Deck playerDeck;
    public List<Card> Hand;

    private Coroutine updateHandRoutine;

    protected override void Awake()
    {
        base.Awake();
        MaxHealth = 100;
        Health = MaxHealth;
        Shield = 0;
        Strength = 0;
        MaxEnergy = 6; // Starting energy
        Energy = MaxEnergy; // Starting energy

        Hand = new List<Card>();
        InitializeDeck(GameFlowManager.Instance.initialDeck);
    }

    private void Update()
    {
        TurnEnded = GameFlowManager.Instance.isPlayerTurn == false;
    }

    public void InitializeDeck(List<Card> initialDeck)
    {
        playerDeck.AddCards(initialDeck);
        playerDeck.ShuffleDeck();
    }

    public void StartTurn()
    {
        Energy = MaxEnergy;
        TickEffects(); // Apply effects like buffs/debuffs
        DrawHand();
        TurnEnded = false;
    }

    public void UseCard(Card card)
    {
        playerDeck.DiscardCard(card);
    }

    public void EndTurn()
    {
        TurnEnded = true;
        ClearHand();
    }

    private void ClearHand()
    {
        for (int i = 0; i < GameFlowManager.Instance.cardSpawnParent.transform.childCount; i++)
        {
            Destroy(GameFlowManager.Instance.cardSpawnParent.transform.GetChild(i).gameObject);
        }
        Hand.Clear();
    }

    public void DrawHand()
    {
        DrawCard(5);
    }

    public void DrawCard(int amount)
    {
        if (updateHandRoutine != null)
        {
            StopCoroutine(updateHandRoutine);
        }
        updateHandRoutine = StartCoroutine(DrawCardCO(amount));
    }

    public void GainEnergy(int amount)
    {
        Energy = Mathf.Min(MaxEnergy, Energy + amount);
    }

    public void UseEnergy(int amount)
    {
        if (Energy >= amount)
        {
            Energy -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough energy!");
        }
    }

    public override void PlayTurn()
    {
        StartTurn();
    }

    private IEnumerator DrawCardCO(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (playerDeck.deckCards.Count > 0)
            {
                Card drawnCard = playerDeck.DrawCard();
                yield return StartCoroutine(SpawnCardCO(drawnCard));
                playerDeck.DiscardCard(drawnCard);
            }
            yield return new WaitForSeconds(.3f);
        }
    }

    public IEnumerator SpawnCardCO(Card c)
    {
        // Instantiate card
        GameObject cardObject = Instantiate(GameFlowManager.Instance.cardPrefab, GameFlowManager.Instance.cardSpawnParent.transform);
        CardUI cardUI = cardObject.GetComponent<CardUI>();
        cardUI.SetCard(c);
        cardObject.transform.DOShakeScale(0.3f);
        yield return new WaitForSeconds(0.3f);
        GameFlowManager.Instance.cardSpawnParent.AlignChildrenInArc();
        Hand.Add(c);
    }
}
