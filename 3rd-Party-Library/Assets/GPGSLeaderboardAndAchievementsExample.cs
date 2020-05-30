using Library.GooglePlay;
using UnityEngine;

#if UNITY_ANDROID

public class GPGSLeaderboardAndAchievementsExample : MonoBehaviour
{
    static int score = 5;
    public void ReportScore()
    {
        GooglePlayGameService.PostToLeaderboard(50,

            (result) =>
            {
                if (result)
                {
                    //SUCCESS
                    Debug.Log("Posted new score to leaderboard");
                }
                else
                {
                    //FAIL
                    Debug.Log("Unable to post new score to leaderboard");
                }

            });
    }

    public void ShowLeaderboard()
    {
        GooglePlayGameService.ShowLeaderboardUI();
    }


    public void ShowAchievements()
    {
        GooglePlayGameService.ShowAchievementUI();
    }

    public void UpdateIncrementalAchievement()
    {
        GooglePlayGameService.UpdateIncrementalAchievement(score + 10, GPGSIds.achievement_incrementalxd, null);
    }
    public void UnlockReqularAchievement()
    {
        GooglePlayGameService.UnlockRegularAchievement(GPGSIds.achievement_ggwp, 100f, null);
    }

}

#endif