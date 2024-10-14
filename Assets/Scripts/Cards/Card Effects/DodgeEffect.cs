using System.Collections;
using UnityEngine;

[System.Serializable]
public class DodgeEffect : IEffect
{
    private int dodgeChance; // Percentage chance to dodge

    public DodgeEffect(int dodgeChance)
    {
        this.dodgeChance = dodgeChance;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        yield return null;
        if (Random.Range(0, 100) < dodgeChance)
        {
            user.TriggerAnimation("Dodge");
            // Implement dodge logic if needed
        }
        else
        {
            user.TriggerAnimation("DodgeFailed");
            // Implement what happens when dodge fails
        }
    }
}