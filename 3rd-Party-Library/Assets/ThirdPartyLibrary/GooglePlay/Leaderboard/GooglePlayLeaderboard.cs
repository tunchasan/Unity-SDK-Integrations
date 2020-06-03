using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

#if UNITY_ANDROID

namespace Library.GooglePlay
{
    using UnityEngine;

    public class GooglePlayLeaderboard : MonoBehaviour
    {
        public static void InitializeLeaderboard(Action<bool> actionStatus)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((success) => {

                if (success)
                {
                    Debug.Log("Google Play Authentication succeed.");

                    actionStatus(true);
                }
                else
                {
                    Debug.LogError("Google Play Authentication failed.");

                    actionStatus(false);
                }
            });
        }

        public static void ReportScore(long score)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_leaderboard, (success) =>
            {
                if (success)
                {
                    Debug.Log("Posted new score to leaderboard");
                }
                else
                {
                    Debug.LogError("Unable to post new to score to leaderboard");
                }
            });
        }

        public static void ShowLeaderboard()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_leaderboard);
        }
    }
}

#endif