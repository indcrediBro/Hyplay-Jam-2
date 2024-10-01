public interface ICard
{
    string Name { get; }
    int EnergyCost { get; }

    void Execute(Player _player, Enemy _enemy);
}


