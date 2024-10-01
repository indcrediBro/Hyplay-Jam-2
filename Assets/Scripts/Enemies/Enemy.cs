using UnityEngine;

public class Enemy : Combatant
{
    public int attackDamage = 10;

    public Enemy() : base(50, 10) { }

    public void ApplyDebuff(DebuffType _debuff, int _amount)
    {
        if (_debuff == DebuffType.Disarm)
            attackDamage = 0;
        if (_debuff == DebuffType.Weaken)
            attackDamage = Mathf.Max(0, attackDamage - _amount);
    }
}