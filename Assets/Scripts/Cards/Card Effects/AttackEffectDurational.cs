using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackEffectDurational : IDurationalEffect
{
    private int damage;
    private int duration;

    public AttackEffectDurational(int damage, int duration)
    {
        this.damage = damage;
        this.duration = duration;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        user.TriggerAnimation("Attack");
        yield return new WaitForSeconds(.25f);

        target.TakeDamage(damage);
        target.TriggerAnimation("Hit");
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