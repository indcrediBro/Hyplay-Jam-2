using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    private Player player => GameFlowManager.Instance.player;
    public Enemy[] enemies;

    public GameFlowManager gameFlowManager;

    public void StartTurnLoop()
    {
        StartCoroutine(StartTurnLoopCO());
    }

    public void StopTurnLoop()
    {
        StopCoroutine(StartTurnLoopCO());
    }

    // Start the turn loop
    private IEnumerator StartTurnLoopCO()
    {
        while (true)
        {
            if (gameFlowManager.isPlayerTurn)
            {
                // Start player's turn
                Debug.Log("Player's Turn");
                StartTurn();
                // Wait until player's turn ends
                yield return new WaitUntil(() => player.TurnEnded);

                // Process end of player turn
                EndTurn();

            }
            else
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    Enemy enemy = enemies[i];

                    Debug.Log("Enemy's Turn");
                    enemy.TurnEnded = false;
                    // Process enemy turn
                    enemy.PlayTurn();

                    // Wait until enemy's turn ends
                    yield return new WaitUntil(() => enemy.TurnEnded);
                    gameFlowManager.EndEnemyTurn();

                }
                // Start enemy's turn

                gameFlowManager.isPlayerTurn = true;
            }

            // Check if the game has ended
            //gameFlowManager.CheckGameOver();

            // Wait for a short delay before switching turns
            yield return new WaitForSeconds(1f);
        }
    }

    public void StartTurn()
    {
        // At the start of the player's turn, tick all active effects
        player.TickEffects();

        foreach (var enemy in enemies)
        {
            enemy.TickEffects();
        }

        player.StartTurn();
        player.TurnEnded = false;
    }

    public void EndTurn()
    {
        EnergySlotManager.Instance.PlayAllAssignedCards();
        player.EndTurn();
        GameFlowManager.Instance.EndPlayerTurn();
        gameFlowManager.isPlayerTurn = false;
        player.TurnEnded = false;
    }
}
