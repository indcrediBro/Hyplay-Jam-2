using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HYPLAY.Leaderboards.Runtime;
using HYPLAY.Core.Runtime;

public class LeaderboardController : Singleton<LeaderboardController>
{
    [SerializeField] private HyplayLeaderboard leaderboard;
    [SerializeField] private GameObject scoreTemplate;
    [SerializeField] private Transform contentParent;

    void Start()
    {
        SignInGuest();
    }

    public async void SignInGuest()
    {
        var res = await HyplayBridge.GuestLoginAndReturnUserAsync();
        if (res.Success)
        {
            Debug.Log($"signing in: {res.Data.Username}");
            if (HyplayBridge.IsLoggedIn)
                GetScores();
        }
        else
            Debug.LogError($"Error signing in: {res.Error}");

    }

    public async void GetScores()
    {
        LeaderboardScoreUI[] pastEntries = contentParent.GetComponentsInChildren<LeaderboardScoreUI>();
        if (pastEntries.Length > 0)
            foreach (var item in pastEntries)
            {
                Destroy(item.gameObject);
            }

        var scores = await leaderboard.GetScores();
        for (int i = 0; i < scores.Data.scores.Length; i++)
        {
            LeaderboardScore score = scores.Data.scores[i];
            LeaderboardScoreUI scoreUI = Instantiate(scoreTemplate, contentParent).GetComponent<LeaderboardScoreUI>();
            scoreUI.UpdateScoreUI((i + 1).ToString(), score.username, score.score.ToString());
            Debug.Log($"{score.username} got {score.score}");
        }
    }

    public async void SubmitScore(string username, int score)
    {
        HyplayBridge.CurrentUser.Id = username;
        var res = await leaderboard.PostScore(score);
        if (!res.Success)
            Debug.LogError($"Error Submitting Leaderboard: {res.Error}");
        //HyplayBridge.CurrentUser.Id = username;
        //await leaderboard.PostScore(score);
    }
}
