using Library.GooglePlay;
using UnityEngine;

public class GPGSLeaderboardExample : MonoBehaviour
{
    private static long score = 0;

    public void ReportScore()
    {
        score += 50;

        GooglePlayGameService.PostToLeaderboard(score);
    }

    public void ShowLeaderboard()
    {
        GooglePlayGameService.ShowLeaderboardUI();
    }
}
