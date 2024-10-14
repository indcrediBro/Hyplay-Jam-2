using System.Collections;

[System.Serializable]
public class DefendEffect : IEffect
{
    private int shieldValue;

    public DefendEffect(int shieldValue)
    {
        this.shieldValue = shieldValue;
    }

    public IEnumerator Apply(Character user, Character target)
    {
        yield return null;
        user.TriggerAnimation("Defend");
        user.Shield += shieldValue;
        user.UpdateUI();
    }
}