using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text turnsTakenBonusText;
    [SerializeField] private TMP_Text healthPreservationBonusText;
    [SerializeField] private TMP_Text enemyDifficultyMultiplierText;
    [SerializeField] private TMP_Text victoryBonusText;
    [SerializeField] private TMP_Text totalScoreText;

    [SerializeField] private TMP_InputField usernameInputText;
    [SerializeField] private GameObject submissionPanel, submittedText;
    [SerializeField] private TMP_Text finalScoreText;

    public Player player;
    public Enemy enemy;

    private bool isExecutingTurn;
    private GameDataReferences gameDataRef;
    private MapPlayerTracker mapTracker;
    private MapManager mapManager;
    private int turnsTaken;
    private int initialPlayerHealth;

    private void Start()
    {
        gameDataRef = GameDataReferences.Instance;
        mapTracker = FindObjectOfType<MapPlayerTracker>();
        mapManager = FindObjectOfType<MapManager>();

        //gameDataRef.mapCanvas.SetActive(false);
        //gameDataRef.rewardsCanvas.SetActive(false);
        //gameDataRef.scoreCanvas.SetActive(false);
        //submissionPanel.SetActive(true);
        //gameDataRef.gameoverCanvas.SetActive(false);
    }

    public void StartNewGame()
    {
        AudioManager.Instance.StopSound("BGM_MainMenu");
        AudioManager.Instance.PlaySound("BGM_Game");
        gameDataRef.continueGame = false;
        SceneManager.LoadScene(1);

        gameDataRef.mapCanvas.SetActive(true);
        gameDataRef.rewardsCanvas.SetActive(false);
        gameDataRef.scoreCanvas.SetActive(false);
        submissionPanel.SetActive(true);
        gameDataRef.gameoverCanvas.SetActive(false);

    }

    public void ContinueGame()
    {
        gameDataRef.continueGame = true;
        SceneManager.LoadScene(1);

        gameDataRef.mapCanvas.SetActive(true);
        gameDataRef.rewardsCanvas.SetActive(false);
        gameDataRef.scoreCanvas.SetActive(false);
        submissionPanel.SetActive(true);
        gameDataRef.gameoverCanvas.SetActive(false);
        player.Score = PlayerPrefs.GetInt("Score");
    }

    public void StartRound(GameObject enemyPrefab)
    {
        if (!player)
        {
            player = Instantiate(gameDataRef.playerPrefab, gameDataRef.playerSpawn).GetComponent<Player>();
        }
        enemy = Instantiate(enemyPrefab, gameDataRef.enemySpawn).GetComponent<Enemy>();
        if (!PlayerPrefs.HasKey("Score"))
        {
            player.Score = 0;
            PlayerPrefs.SetInt("Score", 0);
        }

        gameDataRef.mapCanvas.SetActive(false);
        gameDataRef.rewardsCanvas.SetActive(false);

        turnsTaken = 0;
        initialPlayerHealth = player.Health;
        player.playerDeck.ShuffleDeck();
        gameCanvas.SetActive(true);
        ResetPlayer();
        player.StartTurn();
    }

    private void ResetPlayer()
    {
        player.transform.position = gameDataRef.playerSpawn.position;
        player.Ammo = player.MaxAmmo;
        player.UpdateAmmoDisplay(true);
    }

    private bool IsGameOver()
    {
        return player.Health <= 0 || enemy.Health <= 0;
    }

    public void ExecuteTurns()
    {
        if (isExecutingTurn) return;

        StartCoroutine(ExecuteTurnsCO());
    }

    private IEnumerator ExecuteTurnsCO()
    {
        isExecutingTurn = true;
        gameCanvas.SetActive(false);

        //player.UpdateUI();
        //enemy.UpdateUI();

        Card playerCard = player.SelectedCardUI?.card;
        Card enemyCard = enemy.SelectedCard;


        // Resolve player's card
        if (playerCard != null && player.Ammo >= playerCard.cost && player.UseAmmo(playerCard.cost))
        {
            yield return StartCoroutine(playerCard.UseCard(player, enemy));
        }
        yield return new WaitForSeconds(1f);
        player.ApplyDurationalEffects();
        turnsTaken++;

        yield return new WaitForSeconds(0.5f);

        if (!IsGameOver())
        {
            // Resolve enemy's card
            if (enemyCard != null)
            {
                enemy.HideNextAttack();

                yield return StartCoroutine(enemyCard.UseCard(enemy, player));
            }
            yield return new WaitForSeconds(1f);
            enemy.ApplyDurationalEffects();
            yield return new WaitForSeconds(1f);
        }

        if (!IsGameOver())
        {
            player.UpdateUI();
            enemy.UpdateUI();

            player.StartTurn();
            enemy.UpdateNextAttack();
            player.UpdateUI();
            enemy.UpdateUI();
            gameCanvas.SetActive(true);
            isExecutingTurn = false;
        }
        else
        {
            yield return StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        isExecutingTurn = false;

        if (player.Health <= 0)
        {
            player.TriggerAnimation("Dead");
            enemy.TriggerAnimation("Victory");
            gameDataRef.gameoverCanvas.SetActive(true);
            //yield return StartCoroutine(CalculateScore());
            if (PlayerPrefs.HasKey("Map"))
            {
                PlayerPrefs.DeleteKey("Map");
            }
            if (PlayerPrefs.HasKey("Score"))
            {
                PlayerPrefs.DeleteKey("Score");
            }
        }
        else if (enemy.Health <= 0)
        {
            enemy.TriggerAnimation("Dead");
            player.TriggerAnimation("Victory");

            yield return StartCoroutine(CalculateScore());
            mapTracker.Locked = false;
        }
    }

    private IEnumerator CalculateScore()
    {
        int baseScore = player.Score; // Base score to start from
        int turnsTakenBonus = 100 - (turnsTaken * 5); // Bonus based on turns taken
        float healthPreservationBonus = (player.Health / initialPlayerHealth) * 100; // Bonus based on health preservation
        float enemyDifficultyMultiplier = enemy.stars * 1.5f; // Multiplier based on enemy difficulty
        int victoryBonus = enemy.stars * 100; // Bonus for winning

        // Calculate total score
        float totalScore = baseScore + turnsTakenBonus + healthPreservationBonus + enemyDifficultyMultiplier + victoryBonus;

        baseScoreText.text = "0";
        turnsTakenBonusText.text = "0";
        healthPreservationBonusText.text = "0";
        enemyDifficultyMultiplierText.text = "0";
        victoryBonusText.text = "0";
        totalScoreText.text = "0";

        gameDataRef.rewardsCanvas.SetActive(true);
        gameDataRef.scoreCanvas.SetActive(true);

        yield return new WaitForSeconds(.2f);
        // Update individual score fields
        baseScoreText.text = baseScore.ToString();
        yield return new WaitForSeconds(.2f);
        turnsTakenBonusText.text = turnsTakenBonus.ToString();
        yield return new WaitForSeconds(.2f);
        healthPreservationBonusText.text = Mathf.FloorToInt(healthPreservationBonus).ToString();
        yield return new WaitForSeconds(.2f);
        enemyDifficultyMultiplierText.text = enemyDifficultyMultiplier.ToString();
        yield return new WaitForSeconds(.2f);
        victoryBonusText.text = victoryBonus.ToString();
        yield return new WaitForSeconds(.2f);

        // Animate total score display
        float displayedScore = 0; // Current displayed score
        float duration = 1.5f; // Duration for the score animation

        // Animate total score
        while (displayedScore < totalScore)
        {
            displayedScore += totalScore / (duration / Time.deltaTime); // Increment score
            displayedScore = Mathf.Min(displayedScore, totalScore); // Clamp to totalScore
            totalScoreText.text = Mathf.FloorToInt(displayedScore).ToString(); // Update UI
            yield return null; // Wait for the next frame
        }


        player.Score += Mathf.FloorToInt(totalScore);
        PlayerPrefs.SetInt("Score", player.Score);
        // Optional: Tween to emphasize final score
        totalScoreText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            totalScoreText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBounce);
        });

        finalScoreText.text = Mathf.FloorToInt(player.Score).ToString();
        finalScoreText.transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            finalScoreText.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBounce);
        });
        Debug.Log("Final Score: " + player.Score);
        // Here you can save the score to the leaderboard as needed
        mapManager.SaveMap();
    }

    public void SubmitLeaderboardEntry()
    {
        submissionPanel.SetActive(false);
        submittedText.SetActive(true);
        //Submit Entry
        LeaderboardController.Instance.SubmitScore(usernameInputText.text, player.Score);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        AudioManager.Instance.StopSound("BGM_Game");
        AudioManager.Instance.PlaySound("BGM_MainMenu");
    }
}
