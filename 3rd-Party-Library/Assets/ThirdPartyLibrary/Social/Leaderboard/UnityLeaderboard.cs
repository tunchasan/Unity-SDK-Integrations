using UnityEngine;
using UnityEngine.SocialPlatforms;

public class UnityLeaderboard : MonoBehaviour
{
    private ILeaderboard leaderboard;
    private static string leaderboard_id = "yourleaderboardid"; // setup on appstore connect

    void Start()
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
            }
            else
                Debug.Log("Authentication failed");
        });

        // create social leaderboard
        leaderboard = Social.CreateLeaderboard();
        leaderboard.id = leaderboard_id;
        leaderboard.LoadScores(result =>
        {
            Debug.Log("Received " + leaderboard.scores.Length + " scores");
            foreach (IScore score in leaderboard.scores)
                Debug.Log(score);
        });
    }

    public void ReportScore()
    {
        Debug.Log("Reporting score " + 50 + " on leaderboard " + leaderboard_id);
        Social.ReportScore(50, leaderboard_id, success => {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
}
