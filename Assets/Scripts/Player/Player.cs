public class Player : Combatant
{
    public int m_Ammo = 5;
    public int m_DamageBoost = 0;
    public bool m_IsDisarmed = false;

    public Player() : base(100, 20) { }

    public void Heal(int _amount) { m_Health += _amount; }
    public void Reload() { m_Ammo += 1; }
}
