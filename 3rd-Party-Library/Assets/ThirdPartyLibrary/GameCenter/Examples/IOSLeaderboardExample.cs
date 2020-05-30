using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Library.GameCenter;

public class IOSLeaderboardExample : MonoBehaviour
{
    public string leaderboardID;

    // Start is called before the first frame update
    void Start()
    {
        Leaderboard.InitializeLeaderboard(leaderboardID,
            (success) => {

                if (success)
                {
                    // Everything is good to go
                }
                else
                {
                    // Something went wrong
                }
            });
    }

    public void ReportScore()
    {
        Leaderboard.ReportScore(25);
    }

    public void ShowLeaderboard()
    {
        Leaderboard.ShowLeaderboard();
    }
}
