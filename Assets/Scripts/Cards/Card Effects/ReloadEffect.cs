using System.Collections;
using UnityEngine;

[System.Serializable]
public class ReloadEffect : IEffect
{
    private int reloadValue;

    public ReloadEffect(int amount)
    {
        this.reloadValue = amount;
    }
    public IEnumerator Apply(Character user, Character target)
    {
        yield return null;
        user.Ammo = Mathf.Min(user.MaxAmmo, user.Ammo + reloadValue);
        user.TriggerAnimation("Reload");
        if (user is Player player) player.UpdateAmmoDisplay(true);
    }
}
