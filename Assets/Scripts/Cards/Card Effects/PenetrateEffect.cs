using System.Collections;
using UnityEngine;

[System.Serializable]
public class PenetrateEffect : IEffect
{
    private int damage;

    public PenetrateEffect(int damage)
    {
        this.damage = damage;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        user.TriggerAnimation("Attack");
        yield return new WaitForSeconds(.25f);
        target.TakePenetratingDamage(damage);
        target.TriggerAnimation("Hit");
    }
}