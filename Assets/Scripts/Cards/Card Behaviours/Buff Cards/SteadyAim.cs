public class SteadyAim : ICard
{
    public string Name => "Steady Aim";
    public int EnergyCost => 2;

    public void Execute(Player _player, Enemy _enemy)
    {
        _player.m_DamageBoost += 5;
    }
}
