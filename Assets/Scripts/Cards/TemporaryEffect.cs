public enum EffectType
{
    Shield,
    Strength,
    Energy,
    SkipTurn,
    DrawCard
}

public class TemporaryEffect
{
    public EffectType EffectType { get; private set; }
    public int Value { get; private set; }
    public int Duration { get; private set; }
    public bool IsExpired => Duration <= 0;

    public TemporaryEffect(EffectType effectType, int value, int duration)
    {
        EffectType = effectType;
        Value = value;
        Duration = duration;
    }

    // Reduces the duration each turn
    public void Tick()
    {
        Duration--;
    }
}
