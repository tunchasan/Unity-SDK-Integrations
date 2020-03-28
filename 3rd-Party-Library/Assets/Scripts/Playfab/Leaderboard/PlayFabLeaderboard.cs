using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// </summary>
///
/// </summary>

public class PlayFabLeaderboard : MonoBehaviour
{
    // ResultPlayer represents the Leaderboard unique Player Subfields.
    public struct ResultPlayer
    {
        // Title-specific display name of the user for this leaderboard entry.
        public string DisplayName;

        // URL of the player's avatar image
        public string AvatarUrl;

        // User's overall position in the leaderboard.
        public int Position;

        // Specific value of the user's statistic.
        public int StatValue;

        // List of all authentication systems linked to this player account
        public List<LinkedPlatformAccountModel> LinkedAccounts;
    }

    //FOR TEST, It will be PRIVATE
    public string _statisticName;

    // Configrate unique player statistic info. for Leaderboard
    public void InitializeUser(string leaderboardFieldName) 
    {
        Debug.Log("{ PlayFab LeaderBoard } Service is initializing..." + " { " + " } ");

        //Assign the Leaderboard name to class field.
        this._statisticName = leaderboardFieldName;

        hasLeaderboardData(); // Initialization...
    }

    #region DATA SUBMISSION 

    /// </summary>
    /// The function that handles Player Statistic Request for Leaderboard Position
    /// </summary>
   
    public void SubmitScore(int score)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest // Update Request
        {
            Statistics = new List<StatisticUpdate> {

            new StatisticUpdate {

                StatisticName = _statisticName, //Statistic Field' Name

                Value = score //The Value
            }

        }

        }, result => OnStatisticsUpdated(result), FailureCallback);

    }

    // Statistic Update Success Request Handler
    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");

        RequestLeaderboardWorld();
    }

    // Statistic Update Request Failed Handler
    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region LEADERBOARD

    //Get Player Request based on the Specified Leadeboard Range
    public List<ResultPlayer> RequestLeaderboardWorld(int startPosition, int maxResultCount)
    {
        //The ResultPlayer List.
        List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

        //The ResultPlayer Elem.
        ResultPlayer userData;

        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = _statisticName, // Statistic used to rank players for this leaderboard.

            StartPosition = startPosition, // Position in the leaderboard to start this listing(defaults to the first entry).

            MaxResultsCount = maxResultCount  // Maximum number of entries to retrieve. Default 10, maximum 100.
        },
        
        result => { // Leaderboard access succeed.

            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                //If the player is banned from game.
                if (!player.Profile.BannedUntil.HasValue)
                {
                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.LinkedAccounts = player.Profile.LinkedAccounts; //List Of All Auth. System of Player

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    //Add Player to List
                    resultPlayers.Add(userData);

                }

            }

        },

            LeaderboardFailureCallbackk); // Leaderboard error callback

        return resultPlayers;
    }

    //Request Failed - Debug and Display the issue in the Console.
    private void LeaderboardFailureCallbackk(PlayFabError error)
    {
        Debug.LogWarning("LeaderBoard Data < GET REQUEST > is Failed: ");

        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion

    #region LEADERBOARD VALIDATION

    //Get Player Statistic Data for Validation Player
    public void hasLeaderboardData()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),

        OnGetStatisticSuccess, //Success Callback

        OnGetStatisticError //Error Callback

        );

    }

    //Request Failed - Debug and Display the issue in the Console.
    private void OnGetStatisticError(PlayFabError error)
    {
        Debug.LogError("LeaderBoard Error: " + error.GenerateErrorReport());
    }

    //Request Succeed - Get the Statistic Data
    private void OnGetStatisticSuccess(GetPlayerStatisticsResult result)
    {
        bool isValidUser = false; // Validation Value

        //Check each statistic in the list to see if it's the one you want
        foreach (var eachStat in result.Statistics)
        {
            if (eachStat.StatisticName.Equals(_statisticName))
            {
                Debug.Log("Value: " + eachStat.Value);

                isValidUser = true; // Validation succeed.

                break;
            }

        }

        if (!isValidUser) // Invalid User -> So we need to initialize "User Statistic Data" for Leadearboard Usage...
        {
            SubmitScore(0); // Assign 0 and Finalize the Validation

            Debug.Log("Validation Succeed.");
        }

    }

    #endregion
}