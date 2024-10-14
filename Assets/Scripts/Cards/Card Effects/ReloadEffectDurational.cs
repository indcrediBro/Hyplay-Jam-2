using System.Collections;
using UnityEngine;

[System.Serializable]
public class ReloadEffectDurational : IDurationalEffect
{
    private int reloadValue;
    private int duration;

    public ReloadEffectDurational(int amount, int duration)
    {
        this.reloadValue = amount;
        this.duration = duration;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        yield return null;
        user.Ammo = Mathf.Min(user.MaxAmmo, user.Ammo + reloadValue);
        user.TriggerAnimation("Reload");
        if (user is Player player) player.UpdateAmmoDisplay(true);
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
