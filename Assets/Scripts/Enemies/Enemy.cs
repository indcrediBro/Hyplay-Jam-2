using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : Character
{
    public List<Card> cards;
    [SerializeField] private bool randomiseAttacks = true;
    private int currentAttackIndex = 0;
    public Card SelectedCard;
    [SerializeField] private GameObject nextAttackIcon, nextDefenceIcon, nextDodgeIcon, nextHealIcon;
    [SerializeField] private TMP_Text nextValueText;
    public int stars;
    public List<Card> cardRewards;

    public void Start()
    {
        InitializeEnemy();
    }

    public void InitializeEnemy()
    {
        cards = EnemyManager.Instance.GetEnemyCards(stars);
        NoAttackCardCheck();

        cardRewards = GameDataReferences.Instance.GetRewardCards(stars);
        GameDataReferences.Instance.SetUpRewardCards(cardRewards);

        if (!IsPlayer) Health = MaxHealth;

        UpdateUI();
        UpdateNextAttack();
    }

    public void HideNextAttack()
    {
        nextAttackIcon.SetActive(false);
        nextDefenceIcon.SetActive(false);
        nextDodgeIcon.SetActive(false);
        nextHealIcon.SetActive(false);
        nextValueText.text = "";
    }

    public void UpdateNextAttack()
    {
        SelectedCard = GetSelectedCard();

        if (SelectedCard.cardType == CardType.Attack)
        {
            nextAttackIcon.SetActive(true);
            nextDefenceIcon.SetActive(false);
            nextDodgeIcon.SetActive(false);
            nextHealIcon.SetActive(false);
        }
        else if (SelectedCard.cardType == CardType.Defend)
        {
            nextAttackIcon.SetActive(false);
            nextDefenceIcon.SetActive(true);
            nextDodgeIcon.SetActive(false);
            nextHealIcon.SetActive(false);
        }
        else if (SelectedCard.cardType == CardType.Dodge)
        {
            nextAttackIcon.SetActive(false);
            nextDefenceIcon.SetActive(false);
            nextDodgeIcon.SetActive(true);
            nextHealIcon.SetActive(false);
        }
        else if (SelectedCard.cardType == CardType.Heal)
        {
            nextAttackIcon.SetActive(false);
            nextDefenceIcon.SetActive(false);
            nextDodgeIcon.SetActive(false);
            nextHealIcon.SetActive(true);
        }
        if (SelectedCard.cardType == CardType.Defend)
        {
            nextValueText.text = SelectedCard.extraValue.ToString();
        }
        else
        {
            nextValueText.text = SelectedCard.mainValue.ToString();
        }
    }

    private Card GetSelectedCard()
    {
        if (randomiseAttacks)
        {
            return cards[Random.Range(0, cards.Count)];
        }
        else
        {
            Card selection;
            if (currentAttackIndex == cards.Count)
            {
                currentAttackIndex = 0;
            }
            selection = cards[currentAttackIndex];
            currentAttackIndex++;

            return selection;
        }
    }

    private void NoAttackCardCheck()
    {
        foreach (var card in cards)
        {
            if (card.cardType == CardType.Attack)
            {
                return;
            }
        }
        cards.Add(EnemyManager.Instance.GetEnemyAttackCard(stars));
    }
}
