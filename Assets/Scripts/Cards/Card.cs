using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ExtraTargetType
{
    self,
    other
}
[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int cost;
    public string description;
    public CardType cardType; // enum
    public CardRarity cardRarity; // enum
    public CardEffect mainEffect; // enum  
    public int mainValue;
    public AdditionalEffect extraEffect; // enum
    public ExtraTargetType extraTargetType;
    public int extraValue;
    public int extraDuration;

    // Method to use the card
    public IEnumerator UseCard(Character user, Character target)
    {
        // Apply main effect
        IEffect main = EffectFactory.CreateMainEffect(mainEffect, mainValue);
        yield return main?.Apply(user, target);

        user.UpdateUI();
        target.UpdateUI();
        yield return new WaitForSeconds(2f);
        if (extraEffect == AdditionalEffect.Attack)
        {
            IEffect extra = EffectFactory.CreateExtraEffect(extraEffect, extraValue, extraDuration);
            yield return extra?.Apply(user, target);
        }
        // Apply extra effect if any
        else if (extraEffect != AdditionalEffect.None && extraEffect != AdditionalEffect.Attack)
        {
            IEffect extra = EffectFactory.CreateExtraEffect(extraEffect, extraValue, extraDuration);
            // If it's a durational effect, add it to the target's active effects
            if (extra is IDurationalEffect durational)
            {
                if (extraTargetType == ExtraTargetType.self)
                    user.AddDurationalEffect(durational);
                else
                    target.AddDurationalEffect(durational);
            }
        }

        user.UpdateUI();
        target.UpdateUI();
    }
}
