using System;
using UnityEngine;

public class QuickDraw : ICard
{
    public string Name => "Quick Draw";
    public int EnergyCost => 1;

    public void Execute(Player _player, Enemy _enemy)
    {
        _enemy.TakeDamage(7 + _player.m_DamageBoost);
    }
}