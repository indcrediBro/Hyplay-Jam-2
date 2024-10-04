using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public List<Card> cards;
    [SerializeField] private bool randomiseAttacks = true;
    private int currentAttackIndex;



    public override void PlayTurn()
    {
        StartCoroutine(PlayAction());
        TickEffects();  // Reduces duration of temporary effects
    }

    private IEnumerator PlayAction()
    {
        if (randomiseAttacks)
        {
            cards[Random.Range(0, cards.Count)].Play(this, GameFlowManager.Instance.player); // Passing self and target
        }
        else
        {
            cards[currentAttackIndex].Play(this, GameFlowManager.Instance.player); // Passing self and target
            currentAttackIndex++;
            if (currentAttackIndex >= cards.Count)
            {
                currentAttackIndex = 0;
            }
        }
        yield return new WaitForSeconds(.3f);
        TurnEnded = true;
    }
}
