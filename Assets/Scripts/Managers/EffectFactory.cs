public static class EffectFactory
{
    public static IEffect CreateMainEffect(CardEffect effectType, int value)
    {
        switch (effectType)
        {
            case CardEffect.Attack:
                return new AttackEffect(value);
            case CardEffect.Defend:
                return new DefendEffect(value);
            case CardEffect.Heal:
                return new HealEffect(value);
            case CardEffect.Dodge:
                return new DodgeEffect(value);
            case CardEffect.Reload:
                return new ReloadEffect(value);
            case CardEffect.Penetrate:
                return new PenetrateEffect(value);
            default:
                return null;
        }
    }

    public static IEffect CreateExtraEffect(AdditionalEffect effectType, int value, int duration = 0)
    {
        switch (effectType)
        {
            case AdditionalEffect.Bleed:
                return new BleedEffect(value, duration);
            case AdditionalEffect.Shield:
                return new ShieldEffect(value, duration);
            case AdditionalEffect.Reload:
                return new ReloadEffectDurational(value, duration);
            case AdditionalEffect.Attack:
                return new AttackEffectDurational(value, duration);
            // Add other extra effects here
            case AdditionalEffect.None:
            default:
                return null;
        }
    }
}
