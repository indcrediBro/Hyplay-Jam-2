// Shield Effect (Lasts for a few turns)
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShieldEffect : IDurationalEffect
{
    public int shieldValue;
    public int duration;

    public ShieldEffect(int shieldValue, int duration)
    {
        this.shieldValue = shieldValue;
        this.duration = duration;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        user.UpdateUI();
        user.TriggerAnimation("Shield");
        yield return new WaitForSeconds(.25f);
        user.Shield += shieldValue;
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

