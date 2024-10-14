using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : Character
{
    public Deck playerDeck;
    public List<CardUI> HandUI;
    public CardUI SelectedCardUI;
    private List<GameObject> bulletImages;
    private Transform bulletParent;
    private Coroutine updateHandRoutine;

    protected override void Awake()
    {
        base.Awake();
        MaxHealth = 100;
        Health = MaxHealth;
        Shield = 0;
        Ammo = MaxAmmo;
        HandUI = new List<CardUI>();
        bulletParent = GameDataReferences.Instance.bulletParent;
        bulletImages = GameDataReferences.Instance.bulletImages;
        InitializeDeck(GameDataReferences.Instance.initialDeck);
        UpdateUI();
        UpdateAmmoDisplay();
    }

    public void InitializeDeck(List<Card> initialDeck)
    {
        playerDeck.AddCards(initialDeck);
        playerDeck.ShuffleDeck();
    }

    public void StartTurn()
    {
        DrawHand();
        TurnEnded = false;
    }

    public void SelectCard(CardUI cardUI)
    {
        foreach (var c in HandUI)
        {
            if (c != cardUI)
            {
                if (c != null)
                {
                    c.transform.SetParent(GameDataReferences.Instance.cardSpawnParent.transform);
                    c.DeselectCard();
                }
            }
            else
            {
                SelectedCardUI = cardUI;
                SelectedCardUI.transform.SetParent(GameDataReferences.Instance.cardSelectionParent);
                SelectedCardUI.transform.DOKill();
                SelectedCardUI.transform.DOLocalMove(Vector3.zero, .15f).OnComplete(() =>
                {
                    SelectedCardUI.transform.localRotation = Quaternion.identity;
                });
            }
        }
        GameDataReferences.Instance.cardSpawnParent.AlignChildrenInArc();

        AudioManager.Instance.PlaySound("SFX_PanelOpen");
    }

    public void DeselctCurrentCard()
    {
        SelectedCardUI = null;
        GameDataReferences.Instance.cardSpawnParent.AlignChildrenInArc();
    }

    public void UseCard()
    {
        if (SelectedCardUI != null)
        {
            // Discard the card (game logic)
            playerDeck.DiscardCard(SelectedCardUI.card);

            // Remove the card from HandUI list before destroying it
            if (HandUI.Contains(SelectedCardUI))
            {
                HandUI.Remove(SelectedCardUI);
            }

            // Destroy the card UI GameObject
            Destroy(SelectedCardUI.gameObject);

            // Clear the selected card reference
            SelectedCardUI = null;
        }
        else
        {
            Debug.LogWarning("No card is selected to use.");
        }
    }


    public void EndTurn()
    {
        TurnEnded = true;
        ClearHand();
    }

    private void ClearHand()
    {
        foreach (var cardUI in HandUI)
        {
            if (cardUI != null)
            {
                Destroy(cardUI.gameObject);
            }
        }
        HandUI.Clear(); // Clear the list after destroying all cards
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

    public bool UseAmmo(int amount)
    {
        if (Ammo >= amount)
        {
            Ammo -= amount;
            UpdateAmmoDisplay();
            return true;
        }
        else
        {
            AudioManager.Instance.PlaySound("SFX_Empty");
            Debug.LogWarning("Not enough bullets!");
            return false;
        }
    }

    public void UpdateAmmoDisplay(bool gainedAmmo = false)
    {
        StartCoroutine(UpdateAmmoDisplayCO(gainedAmmo));
    }

    private IEnumerator UpdateAmmoDisplayCO(bool gainedAmmo = false)
    {
        for (int i = bulletImages.Count - 1; i >= 0; i--)
        {
            if (i < Ammo)
            {
                if (gainedAmmo)
                {
                    bulletImages[i].SetActive(true);
                    AudioManager.Instance.PlaySound("SFX_Reload");
                    // Spin effect when gaining ammo
                    bulletParent.localRotation = Quaternion.Euler(0, 0, -360); // Start fully rotated
                    bulletParent.DORotate(Vector3.forward * 0, 0.5f).SetEase(Ease.OutCubic); // Spin back to default
                }
            }
            else
            {
                if (bulletImages[i].activeSelf) // If already inactive, skip rotation
                {
                    bulletImages[i].SetActive(false);

                    // Spin effect when losing ammo
                    yield return new WaitForSeconds(0.5f);
                    AudioManager.Instance.PlaySound("SFX_Cock");
                    bulletParent.DORotate(new Vector3(0, 0, -60), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InCubic); // Rotate by 60 degrees
                }
            }
        }
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
            yield return new WaitForSeconds(.2f);
        }
    }

    public IEnumerator SpawnCardCO(Card c)
    {
        // Instantiate card
        AudioManager.Instance.PlaySound("SFX_PanelOpen");

        GameObject cardObject = Instantiate(GameDataReferences.Instance.cardPrefab, GameDataReferences.Instance.cardSpawnParent.transform);
        CardUI cardUI = cardObject.GetComponent<CardUI>();
        cardUI.SetCard(c);
        cardObject.transform.DOShakeScale(0.2f);
        yield return new WaitForSeconds(0.2f);
        GameDataReferences.Instance.cardSpawnParent.AlignChildrenInArc();
        AudioManager.Instance.PlaySound("SFX_PanelClose");
        HandUI.Add(cardUI);
    }
}
