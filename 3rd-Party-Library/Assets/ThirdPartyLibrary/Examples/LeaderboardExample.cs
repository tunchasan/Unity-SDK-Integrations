using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.Social;
using Library.Social.Leaderboard;
using System;

public class LeaderboardExample : MonoBehaviour
{
    /*******************************************************************************************************************/

    public void InitializeLeaderboard() // Initialize Leaderboards
    {
        Dictionary<string, int> setupList = new Dictionary<string, int>();

        setupList.Add("LEADERBOARD_01", 0);

        setupList.Add("LEADERBOARD_02", 0);

        setupList.Add("LEADERBOARD_03", 0);

        PlayFabLeaderboard.SubmitScores(setupList);
    }

    /*******************************************************************************************************************/

    public void SubmitManyScores() // Submit Scores
    {
        Dictionary<string, int> scoreList = new Dictionary<string, int>();

        scoreList.Add("LEADERBOARD_01", 50);

        scoreList.Add("LEADERBOARD_02", 100);

        scoreList.Add("LEADERBOARD_03", 200);

        PlayFabLeaderboard.SubmitScores(scoreList);
    }

    /*******************************************************************************************************************/

    public void SubmitSingleScore() // Submit Score
    {
        PlayFabLeaderboard.SubmitScore("LEADERBOARD_01",500);
    }

    /*******************************************************************************************************************/

    public void GetScores() // Get Scores
    {
        PlayFabLeaderboard.GetScores(

            (result) =>
            {
                foreach(var score in result)
                {
                    Debug.Log(score.Key + " : " + score.Value);
                }
            },

            (errorMessage) =>
            {
                Debug.LogError(errorMessage);
            });

    }

    /*******************************************************************************************************************/

    public void GetGlobalLeaderboard() // Get Global Leaderboard
    {
        PlayFabLeaderboard.GetLeaderboardGlobal(50, "LEADERBOARD_01", GlobalLeaderboardResultCallback, GlobalLeaderboardErrorCallback);
    }

    private void GlobalLeaderboardResultCallback(List<ResultPlayer> resultPlayers) // Success Callback
    {
        foreach (var player in resultPlayers)
        {
            Debug.Log("Display Name: { " + player.DisplayName + " }" + " Avatar URL: { " + player.AvatarUrl + " }" + " Position: { " + player.Position + " }" + " StatValue: { " + player.StatValue + " }");
        }
    }

    private void GlobalLeaderboardErrorCallback(string error) // Error Callback
    {
        Debug.LogError(error);
    }

    /*******************************************************************************************************************/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) // Initialize Leaderboard
        {
            InitializeLeaderboard();
        }

        else if (Input.GetKey("s")) // Submit Many Scores
        {
            SubmitManyScores();
        }

        else if (Input.GetKey("d")) // Submit Scores
        {
            SubmitSingleScore();
        }

        else if (Input.GetKey("a")) // Submit Scores
        {
            GetScores();
        }

        else if (Input.GetKey("e")) // Get Global Leaderboard
        {
            GetGlobalLeaderboard();
        }
    }
}
