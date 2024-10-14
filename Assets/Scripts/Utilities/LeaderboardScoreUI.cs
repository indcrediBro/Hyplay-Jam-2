using UnityEngine;
using System.Collections;
using TMPro;

public class LeaderboardScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rank, username, score;

    public void UpdateScoreUI(string _rank, string _username, string _score)
    {
        rank.text = _rank;
        username.text = _username;
        score.text = _score;
    }
}
