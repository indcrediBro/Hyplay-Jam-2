public class Player : Combatant
{
    public int ammo = 5;
    public int damageBoost = 0;
    public bool isDisarmed = false;

    public Player() : base(100, 20) { }

    public void Heal(int amount) { m_Health += amount; }
    public void Reload() { ammo += 1; }
}
