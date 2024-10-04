using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public TMP_Text cardNameText;
    public TMP_Text cardCostText;
    public TMP_Text cardDescriptionText;
    public TMP_Text cardTypeText;
    public Image cardIcon;

    public EnergySlotManager energySlotManager;

    private Card card;
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    private int originalSiblingIndex;
    private CanvasGroup canvasGroup;

    private bool isSelected = false;
    private bool isDragging = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        energySlotManager = EnergySlotManager.Instance;
    }

    public void SetCard(Card _card)
    {
        card = _card;
        cardNameText.text = card.cardName;
        cardCostText.text = card.cost.ToString();
        cardDescriptionText.text = card.description;
        cardTypeText.text = GetCardTypeName(card.cardType).ToString();
    }

    string GetCardTypeName(CardType _type)
    {
        switch (_type)
        {
            case CardType.Attack:
                return "Attack";
            case CardType.Shield:
                return "Shield";
            case CardType.Heal:
                return "Heal";
            case CardType.Buff:
                return "Buff";
            case CardType.Debuff:
                return "Debuff";
            default:
                return "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging) // Prevent selection if dragging
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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        initialPosition = rectTransform.anchoredPosition;
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        canvasGroup.blocksRaycasts = false;
        rectTransform.DOScale(1.1f, 0.2f);
        rectTransform.localRotation = Quaternion.identity;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out mousePosition);
        rectTransform.anchoredPosition = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;
        GameObject target = eventData.pointerEnter?.gameObject;
        if (IsValidTarget(target))
        {
            Debug.Log(card.cardName + " Card assigned!");
            SlotUI slot = target.GetComponent<SlotUI>();
            if (slot.card == null)
            {
                slot.AddCardToSlot(card);
                transform.SetParent(null);
                GameFlowManager.Instance.cardSpawnParent.AlignChildrenInArc();
                Destroy(gameObject);
            }
            else
            {
                ResetCardPosition();
            }
        }
        else
        {
            ResetCardPosition();
        }
    }

    private void SelectCard()
    {
        isSelected = true;
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        // Animate the card to "stick out" and reset rotation
        rectTransform.DOLocalMoveY(initialPosition.y + 200f, 0.3f).OnComplete(() =>
        {
            rectTransform.localRotation = Quaternion.identity; // Reset rotation to zero
        });
    }

    private void DeselectCard()
    {
        isSelected = false;
        // Animate the card back to its original position
        rectTransform.DOAnchorPosY(initialPosition.y, 0.3f);
        rectTransform.SetSiblingIndex(originalSiblingIndex);
        GameFlowManager.Instance.cardSpawnParent.AlignChildrenInArc();
    }

    private bool IsValidTarget(GameObject target)
    {
        // Implement your logic for checking valid targets
        return target != null && target.CompareTag("EnergySlot");
    }

    private void ResetCardPosition()
    {
        // Reset card to original position and sibling index if drag is invalid
        rectTransform.DOAnchorPos(initialPosition, 0.5f).OnComplete(() =>
        {
            rectTransform.SetSiblingIndex(originalSiblingIndex);
        });
        DeselectCard();
    }
}
