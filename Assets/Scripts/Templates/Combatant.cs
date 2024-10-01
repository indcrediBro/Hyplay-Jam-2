using UnityEngine;

public abstract class Combatant : MonoBehaviour
{
    public int m_Health;
    public int m_Shield;

    public Combatant(int _health, int _shield)
    {
        this.m_Health = _health;
        this.m_Shield = _shield;
    }

    public void TakeDamage(int _amount)
    {
        if (m_Shield > 0)
        {
            int damageAfterShield = _amount - m_Shield;
            m_Shield = Mathf.Max(0, m_Shield - _amount);
            if (damageAfterShield > 0)
                m_Health -= damageAfterShield;
        }
        else
        {
            m_Health -= _amount;
        }
    }
}
