using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int cost;
    public string description;
    public CardType cardType;
    public CardRarity cardRarity;
    public CardAction[] actions;

    public void Play(Character self, Character target)
    {
        foreach (CardAction action in actions)
        {
            action.Execute(self, target);
        }
    }
}

