public interface IObserver
{
    void OnHealthChanged(int newHealth);
    void OnShieldChanged(int newShield);
    void OnEnemyHealthChanged(int newHealth);
}
