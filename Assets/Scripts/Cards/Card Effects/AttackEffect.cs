using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackEffect : IEffect
{
    private int damage;

    public AttackEffect(int damage)
    {
        this.damage = damage;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        user.TriggerAnimation("Attack");
        yield return new WaitForSeconds(1f);
        target.TakeDamage(damage);
        target.TriggerAnimation("Hit");
    }
}