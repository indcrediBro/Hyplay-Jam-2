using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEditor.ShaderGraph;

public class PlayerHand : MonoBehaviour
{
    public Player player;
    public GameObject cardPrefab;
    public Transform spawnPoint;
    private Coroutine updateHandRoutine;

    private void UpdateHandUI()
    {
        if (updateHandRoutine != null)
        {
            StopCoroutine(updateHandRoutine);
        }
        updateHandRoutine = StartCoroutine(UpdateRadialHand());
    }

    private IEnumerator UpdateRadialHand()
    {
        ClearHand();
        List<Card> hand = player.Hand;
        for (int i = 0; i < hand.Count; i++)
        {
            // Instantiate card
            GameObject cardObject = Instantiate(cardPrefab, spawnPoint);
            CardUI cardUI = cardObject.GetComponent<CardUI>();
            cardUI.SetCard(hand[i]); // Set card data
            cardObject.transform.DOShakeScale(0.3f);
            //cardObject.transform.SetParent(spawnPoint);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ClearHand()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
