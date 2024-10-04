using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Card card;
    public Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        image.color = Color.grey;
    }

    public void ClearSlot()
    {
        image.color = Color.grey;
        button.onClick.RemoveListener(ReturnToHand);
        card = null;
    }

    public void ReturnToHand()
    {
        StartCoroutine(GameFlowManager.Instance.player.SpawnCardCO(card));
        card = null;
        image.color = Color.grey;
        button.onClick.RemoveListener(ReturnToHand);
    }

    public void AddCardToSlot(Card c)
    {
        card = c;
        image.color = Color.yellow;
        button.onClick.AddListener(ReturnToHand);
    }
}
