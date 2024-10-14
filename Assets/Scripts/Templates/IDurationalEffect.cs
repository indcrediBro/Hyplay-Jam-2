using System;

public interface IDurationalEffect : IEffect
{
    int GetDuration();
    void DecrementDuration();
    bool IsExpired();
}
