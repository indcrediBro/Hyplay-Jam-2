using UnityEngine;
using System.Collections.Generic;

public class GameFlowManager : Singleton<GameFlowManager>
{
    [SerializeField] private GameObject playerPrefab;
    public GameObject cardPrefab;
    public ArcLayoutGroup cardSpawnParent;
    [SerializeField] GameObject gameCanvas;
    public List<Card> allCards;
    public Player player;
    public Enemy enemy;
    public bool isPlayerTurn;
    public List<Card> initialDeck;

    private void Start()
    {
        InitializeGame();
    }


    private void InitializeGame()
    {
        // Initialize player deck with your cards (assumed to be instantiated)
        isPlayerTurn = true;
        UpdateGameState();
        FindObjectOfType<TurnManager>().StartTurnLoop();
    }

    private void UpdateGameState()
    {
        if (isPlayerTurn)
        {
            gameCanvas.SetActive(true);
        }
        else
        {
            gameCanvas.SetActive(false);
        }
    }

    public void EndEnemyTurn()
    {
        isPlayerTurn = true;
        UpdateGameState();
    }

    public void EndPlayerTurn()
    {

        isPlayerTurn = false; // Switch to enemy turn
        UpdateGameState();
    }
}
