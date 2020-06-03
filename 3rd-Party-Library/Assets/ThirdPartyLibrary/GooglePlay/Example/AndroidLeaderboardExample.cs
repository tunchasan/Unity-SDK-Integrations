using UnityEngine;

#if UNITY_ANDROID

using Library.GooglePlay;

public class AndroidLeaderboardExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GooglePlayLeaderboard.InitializeLeaderboard((success) =>
        {
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
        GooglePlayLeaderboard.ReportScore(25);
    }

    public void ShowLeaderboard()
    {
        GooglePlayLeaderboard.ShowLeaderboard();
    }
}

#endif