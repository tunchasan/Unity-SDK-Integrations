using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library.Social.Leaderboard
{
    /// </summary>
    /// Class that handles PlayFab Leaderboard Configurations and also provides many features like "GLOBAL LEADERBOARD"
    /// "LOCAL LEADERBOARD - Based on Friends" and "LOCAL LEADERBOARD - Based on Friends + Facebook Friends"
    /// </summary>
    // ResultPlayer represents the Leaderboard unique Player Subfields.
    public class ResultPlayer
    {
        // Title-specific display name of the user for this leaderboard entry.
        public string DisplayName;

        // URL of the player's avatar image
        public string AvatarUrl;

        // User's overall position in the leaderboard.
        public int Position;

        // Specific value of the user's statistic.
        public int StatValue;
    }

    public class PlayFabLeaderboard
    {
       
        #region SET STATISTICS

        /// </summary>
        /// The function that handles Player Statistic Request for Leaderboard Position
        /// </summary>

        public static void SubmitScore(string leaderboardName, int score)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest // Update Request
            {
                Statistics = new List<StatisticUpdate> {

                    new StatisticUpdate {

                        StatisticName = leaderboardName, // Statistic Field' Name

                        Value = score //The Value
                    }

                }
    
            }, result => OnStatisticsUpdated(result), FailureCallback);

        }

        /// </summary>
        /// The function that handles Player Statistics Request
        /// </summary>
        public static void SubmitScores(Dictionary<string,int> leaderboardList)
        {
            List<StatisticUpdate> statisticList = new List<StatisticUpdate>();

            foreach (var data in leaderboardList)
            {
                StatisticUpdate elem = new StatisticUpdate();

                elem.StatisticName = data.Key;

                elem.Value = data.Value;

                statisticList.Add(elem);
            }

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest // Update Request
            {
                Statistics = statisticList

            }, result => OnStatisticsUpdated(result), FailureCallback);

        }

        // Statistic Update Success Request Handler
        private static void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
        {
            Debug.Log("Successfully submitted high scores");
        }

        // Statistic Update Request Failed Handler
        private static void FailureCallback(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");

            Debug.LogError(error.GenerateErrorReport());
        }

        #endregion

        #region LEADERBOARD

        #region GLOBAL

        // Returns a List that stores player Leaderboard information based on specified range ( startPosition - maxResultCount )
        // GLOBAL //
        public static void GetLeaderboardGlobal(int maxResultCount, string leaderboardName, Action<List<ResultPlayer>> resultCallback, Action<string> errorCallback)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            (result) => { // Leaderboard access succeed.

                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    //The ResultPlayer Elem.
                    ResultPlayer userData = new ResultPlayer();

                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    //Add Player to List
                    resultPlayers.Add(userData);
                }

                resultCallback(resultPlayers);
            },

            (error) =>
            {
                errorCallback("LeaderBoard Data < GET REQUEST > is Failed: " + error.GenerateErrorReport().ToString());

            }); // Leaderboard error callback

        }

        // Returns a List that stores player Leaderboard information based on specified range ( startPosition - maxResultCount )
        // GLOBAL //
        public static void GetLeaderboardGlobal(int startPosition, int maxResultCount, string leaderboardName, Action<List<ResultPlayer>> resultCallback, Action<string> errorCallback)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                StartPosition = startPosition,

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            (result) => { // Leaderboard access succeed.

                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    //The ResultPlayer Elem.
                    ResultPlayer userData = new ResultPlayer();

                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    //Add Player to List
                    resultPlayers.Add(userData);
                }

                resultCallback(resultPlayers);
            },

            (error) =>
            {
                errorCallback("LeaderBoard Data < GET REQUEST > is Failed: " + error.GenerateErrorReport().ToString());

            }); // Leaderboard error callback

        }

        #endregion

        #region LOCAL

        // Returns a List that stores player Leaderboard information based on specified " maxResultCount "
        // LOCAL - ONLY FRIENDS //
        public static void GetLeaderboardOnlyFriends(int maxResultCount, string leaderboardName, Action<List<ResultPlayer>> resultCallback, Action<string> errorCallback)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayFabClientAPI.GetFriendLeaderboardAroundPlayer(new GetFriendLeaderboardAroundPlayerRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                IncludeFacebookFriends = false, // Indicates whether Facebook friends should be included in the response.

                IncludeSteamFriends = false,  // Indicates whether Steam friends should be included in the response.

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            result => { // Leaderboard access succeed.

                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    //The ResultPlayer Elem.
                    ResultPlayer userData = new ResultPlayer();

                    userData.DisplayName = player.DisplayName; // Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; // Avatar Url

                    userData.Position = player.Position; // Player Overall Position

                    userData.StatValue = player.StatValue; // Player Score Value

                    //Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                    //Add Player to List
                    resultPlayers.Add(userData);

                }

                resultCallback(resultPlayers);
            }, 
            
            (error) =>
            {
                errorCallback("LeaderBoard Data < GET REQUEST > is Failed: " + error.GenerateErrorReport().ToString());

            }); // Leaderboard error callback

        }

        // Returns a List that stores player Leaderboard information based on specified " maxResultCount "
        // LOCAL - INGAME FRIENDS + FACEBOOK FRIENDS //
        public static void GetLeaderboardFacebookandFriends(int maxResultCount, string leaderboardName, Action<List<ResultPlayer>> resultCallback, Action<string> errorCallback)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayFabClientAPI.GetFriendLeaderboardAroundPlayer(new GetFriendLeaderboardAroundPlayerRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                IncludeFacebookFriends = true, // Indicates whether Facebook friends should be included in the response.

                IncludeSteamFriends = false, // Indicates whether Steam friends should be included in the response.

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            result => { // Leaderboard access succeed.

                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    //The ResultPlayer Elem.
                    ResultPlayer userData = new ResultPlayer();

                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    //Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                    //Add Player to List
                    resultPlayers.Add(userData);
                }

                resultCallback(resultPlayers);

            },

            (error) =>
            {
                errorCallback("LeaderBoard Data < GET REQUEST > is Failed: " + error.GenerateErrorReport().ToString());

            }); // Leaderboard error callback

        }

        // Returns a List that stores player Leaderboard information based on specified " maxResultCount "
        // LOCAL - INGAME FRIENDS + FACEBOOK FRIENDS //
        public static void GetLeaderboardFacebookandFriends(int startPosition, int maxResultCount, string leaderboardName, Action<List<ResultPlayer>> resultCallback, Action<string> errorCallback)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayFabClientAPI.GetFriendLeaderboard(new GetFriendLeaderboardRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                StartPosition = startPosition, // Start position on the leaderboard

                IncludeFacebookFriends = true, // Indicates whether Facebook friends should be included in the response.

                IncludeSteamFriends = false, // Indicates whether Steam friends should be included in the response.

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            result => { // Leaderboard access succeed.

                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    //The ResultPlayer Elem.
                    ResultPlayer userData = new ResultPlayer();

                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    //Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                    //Add Player to List
                    resultPlayers.Add(userData);
                }

                resultCallback(resultPlayers);

            },

            (error) =>
            {
                errorCallback("LeaderBoard Data < GET REQUEST > is Failed: " + error.GenerateErrorReport().ToString());

            }); // Leaderboard error callback

        }

        #endregion

        #endregion

        #region GET STATISTICS

        public static void GetScores(Action<Dictionary<string,int>> resultCallback, Action<string> errorCallback)
        {
            Dictionary<string, int> resultData = new Dictionary<string, int>();

            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),

            (result) =>
            {
                foreach (var data in result.Statistics)
                {
                    resultData.Add(data.StatisticName,data.Value);
                }

                resultCallback(resultData);
            },

            (error) =>
            {
                errorCallback(error.GenerateErrorReport());
            });

        }

        #endregion

        // PlayerProfileViewConstraints Initializer for Leaderboard User Datas.
        private static PlayerProfileViewConstraints GetProfileViewObject()
        {
            PlayerProfileViewConstraints profile = new PlayerProfileViewConstraints();

            profile.ShowAvatarUrl = true; // Avatar Url

            profile.ShowBannedUntil = true; // Ban Information

            profile.ShowDisplayName = true; // Avatar Display Name

            return profile; // Return Profile
        }
    }
}

