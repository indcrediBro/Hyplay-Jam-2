using System.Collections;
using UnityEngine;

[System.Serializable]
public class HealEffect : IEffect
{
    private int healValue;

    public HealEffect(int healValue)
    {
        this.healValue = healValue;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        user.Health = Mathf.Min(user.MaxHealth, user.Health + healValue);
        user.TriggerAnimation("Heal");
        yield return new WaitForSeconds(0.25f);
        user.UpdateUI();
    }
}