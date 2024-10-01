using UnityEngine;

public class Disarm : ICard
{
    public string Name => "Disarm";
    public int EnergyCost => 2;

    public void Execute(Player _player, Enemy _enemy)
    {
        _enemy.ApplyDebuff(DebuffType.Disarm, 0);
        Debug.Log("The enemy's attack was disarmed.");
    }
}