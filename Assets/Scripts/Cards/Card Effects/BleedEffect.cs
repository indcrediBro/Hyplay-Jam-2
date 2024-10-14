// Bleed Effect (Inflicts damage over turns)
using System.Collections;
using UnityEngine;

[System.Serializable]
public class BleedEffect : IDurationalEffect
{
    private int damagePerTurn;
    private int duration;

    public BleedEffect(int damagePerTurn, int duration)
    {
        this.damagePerTurn = damagePerTurn;
        this.duration = duration;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        yield return new WaitForSeconds(.25f);

        target.TakeDamage(damagePerTurn);
        target.TriggerAnimation("Hit");
        target.UpdateUI();
    }

    public void DecrementDuration()
    {
        duration--;
    }

    public int GetDuration()
    {
        return duration;
    }

    public bool IsExpired()
    {
        return duration <= 0;
    }
}
