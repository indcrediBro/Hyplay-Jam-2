public class TakeCover : ICard
{
    public string Name => "Take Cover";
    public int EnergyCost => 1;

    public void Execute(Player _player, Enemy _enemy)
    {
        _player.m_Shield += 10;
    }
}