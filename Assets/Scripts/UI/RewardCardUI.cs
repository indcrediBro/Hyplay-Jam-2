using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RewardCardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardCostText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private Image cardIcon;
    [SerializeField] private Image cardBackground;

    public Card card;

    public void SetupCard(Card _card)
    {
        card = _card;
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();
        cardDescriptionText.text = card.description;
        cardTypeText.text = getCardTypeName(card.cardType).ToString();
        cardBackground.color = GameDataReferences.Instance.cardColorTints[(int)card.cardRarity];

        string getCardTypeName(CardType _type)
        {
            switch (_type)
            {
                case CardType.Attack:
                    return "Attack";
                case CardType.Defend:
                    return "Defence";
                case CardType.Heal:
                    return "Health";
                case CardType.Dodge:
                    return "Dodge";
                case CardType.Reload:
                    return "Reload";
                default:
                    return "";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameDataReferences.Instance.SelectRewardCard(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(2f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameDataReferences.Instance.selectedRewardCard != card)
            transform.DOScale(1.5f, 0.25f);
    }
}