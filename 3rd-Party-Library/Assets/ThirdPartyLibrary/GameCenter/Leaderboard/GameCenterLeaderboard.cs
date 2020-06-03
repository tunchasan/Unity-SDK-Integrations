using System;

using UnityEngine.SocialPlatforms;

#if UNITY_IOS

namespace Library.GameCenter
{
    using UnityEngine;

    public class GameCenterLeaderboard
    {
        private static ILeaderboard leaderboard;

        // App Store leaderboardID
        public static string LeaderboardID { private set; get; }

        public static void InitializeLeaderboard(string leaderboardID, Action<bool> actionStatus)
        {
            // assign field
            LeaderboardID = leaderboardID;

            if (Social.localUser.authenticated == false)
            {
                // Authenticate user first
                Social.localUser.Authenticate(success => {
                    if (success)
                    {
                        Debug.Log("Authentication successful");

                        string userInfo = "Username: " + Social.localUser.userName +
                            "\nUser ID: " + Social.localUser.id +
                            "\nIsUnderage: " + Social.localUser.underage;

                        Debug.Log(userInfo);

                        // Callback action
                        actionStatus(true);
                    }
                    else
                    {
                        Debug.Log("Authentication failed");

                        // Callback action
                        actionStatus(false);
                    }
                });

                // if authentication to gamecenter succeed, we initialize leaderboard
                if (Social.localUser.authenticated)
                {
                    // create social leaderboard
                    leaderboard = Social.CreateLeaderboard();

                    leaderboard.id = LeaderboardID;

                    leaderboard.LoadScores(result =>
                    {
                        Debug.Log("Received " + leaderboard.scores.Length + " scores");

                        foreach (IScore score in leaderboard.scores)
                            Debug.Log(score);
                    });
                }
            }

            else
            {
                Debug.Log("You are already authenticated.");

                actionStatus(true);
            }
        }

        public static void ReportScore(long score)
        {
            Debug.Log("Reporting score " + score + " on leaderboard " + LeaderboardID);

            Social.ReportScore(score, LeaderboardID, success => {

                Debug.Log(success ? "Reported score successfully" : "Failed to report score");
            });
        }

        public static void ShowLeaderboard()
        {
            Social.ShowLeaderboardUI();
        }
    }
}

#endif