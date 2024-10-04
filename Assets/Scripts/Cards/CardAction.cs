using UnityEngine;
using System;

public enum ActionType
{
    DealDamage,
    GainShield,
    ReduceStrength,
    DrawCard,
    IncreaseStrength,
    GainHealth,
    GainEnergy,
    UseEnergy,
    SkipTurn
}

public enum TargetType
{
    Self, Other
}

[Serializable]
public class CardAction
{
    public ActionType actionType;
    public TargetType targetType;
    public int value;      // Can be used for damage, shield, strength, energy, etc.
    public int duration;   // For actions with a duration (e.g., shield, debuffs)

    public void Execute(Character self, Character target)
    {
        Character actualTarget = targetType == TargetType.Self ? self : target;

        switch (actionType)
        {
            case ActionType.DealDamage:
                actualTarget.TakeDamage(value);
                break;
            case ActionType.GainShield:
                actualTarget.GainShield(value, duration);
                break;
            case ActionType.ReduceStrength:
                actualTarget.ModifyStrength(-value, duration);
                break;
            case ActionType.IncreaseStrength:
                actualTarget.ModifyStrength(value, duration);
                break;
            case ActionType.GainHealth:
                actualTarget.GainHealth(value);
                break;
            case ActionType.GainEnergy:
                if (actualTarget is Player playerEnergy)
                {
                    playerEnergy.GainEnergy(value);
                }
                break;
            case ActionType.UseEnergy:
                if (actualTarget is Player playerUseEnergy)
                {
                    playerUseEnergy.UseEnergy(value);
                }
                break;
            case ActionType.SkipTurn:
                if (actualTarget is Enemy enemy)
                {
                    enemy.AddTemporaryEffect(new TemporaryEffect(EffectType.SkipTurn, 1, duration));
                }
                break;
            case ActionType.DrawCard:
                if (actualTarget is Player playerCard)
                {
                    playerCard.DrawCard(value);
                }
                break;
            default:
                Debug.LogWarning("Action type not implemented.");
                break;
        }
    }
}
