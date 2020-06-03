using UnityEngine;

using Library.GameCenter;

#if UNITY_IOS

public class IOSLeaderboardExample : MonoBehaviour
{
    public string leaderboardID;

    // Start is called before the first frame update
    void Start()
    {
        GameCenterLeaderboard.InitializeLeaderboard(leaderboardID,
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
        GameCenterLeaderboard.ReportScore(25);
    }

   public void ShowLeaderboard()
    {
        GameCenterLeaderboard.ShowLeaderboard();
    }
}

#endif