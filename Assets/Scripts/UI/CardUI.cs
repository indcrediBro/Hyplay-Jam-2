using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text cardNameText;
    public TMP_Text cardCostText;
    public TMP_Text cardDescriptionText;
    public TMP_Text cardTypeText;
    public Image cardIcon;
    public Image cardBackground;

    public Card card;
    private RectTransform rectTransform;
    private int originalSiblingIndex;

    private bool isSelected = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetCard(Card _card)
    {
        card = _card;
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();
        cardDescriptionText.text = card.description;
        cardTypeText.text = GetCardTypeName(card.cardType).ToString();
        cardBackground.color = GameDataReferences.Instance.cardColorTints[(int)card.cardRarity];
    }

    string GetCardTypeName(CardType _type)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected)
        {
            DeselectCard();
        }
        else
        {
            SelectCard();
        }
    }


    public void SelectCard()
    {
        isSelected = true;
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        GameController.Instance.player.SelectCard(this);
    }

    public void DeselectCard()
    {
        isSelected = false;
        if (GameController.Instance.player.SelectedCardUI == this)
        {
            transform.SetParent(GameDataReferences.Instance.cardSpawnParent.transform);
            rectTransform.SetSiblingIndex(originalSiblingIndex);
            GameController.Instance.player.DeselctCurrentCard();
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
