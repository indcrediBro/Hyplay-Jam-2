using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnergySlotManager : Singleton<EnergySlotManager>
{
    public List<SlotUI> energySlots;
    private GameFlowManager gameController => GameFlowManager.Instance;
    public void PlayAllAssignedCards()
    {
        foreach (var slot in energySlots)
        {
            if (slot.card != null)
            {
                gameController.player.UseCard(slot.card);
                slot.card.Play(gameController.player, gameController.enemy); // Passing self and target
                slot.ClearSlot(); // Ensure card is removed after play
            }
        }
        gameController.player.EndTurn();
    }
}
