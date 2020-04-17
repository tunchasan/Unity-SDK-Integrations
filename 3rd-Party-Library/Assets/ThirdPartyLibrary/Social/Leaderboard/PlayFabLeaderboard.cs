using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

namespace Library.Social.Leaderboard
{
    /// </summary>
    /// Class that handles PlayFab Leaderboard Configurations and also provides many features like "GLOBAL LEADERBOARD"
    /// "LOCAL LEADERBOARD - Based on Friends" and "LOCAL LEADERBOARD - Based on Friends + Facebook Friends"
    /// </summary>

    public class PlayFabLeaderboard
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
        }

        ///FOR TEST, It will be PRIVATE
        private static List <StatisticUpdate> _statisticField;

        // Configrate unique player statistic info. for Leaderboard
        public static void InitializeLeaderBoards(List <StatisticUpdate> leaderboardFieldNames)
        {
            Debug.Log("{ PlayFab LeaderBoard } Service is initializing..." + " { " + " } ");

            //Assign the Leaderboard name to class field.
            _statisticField = leaderboardFieldNames;

            AddStatisticFieldsToPlayer(); // Initialization...
        }

        #region DATA SUBMISSION 

        /// </summary>
        /// The function that handles Player Statistic Request for Leaderboard Position
        /// </summary>

        public static void SubmitScore(int score, string leaderboardName)
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

        public static void SubmitScores(List<StatisticUpdate> leaderboards)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest // Update Request
            {
                Statistics = leaderboards

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
        public static List<ResultPlayer> RequestLeaderboardGlobal(int startPosition, int maxResultCount, string leaderboardName)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            //The ResultPlayer Elem.
            ResultPlayer userData;

            PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                StartPosition = startPosition, // Position in the leaderboard to start this listing(defaults to the first entry).

                MaxResultsCount = maxResultCount,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return
            },

            result => { // Leaderboard access succeed.

            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                //If the player is banned from game.
                //if (!player.Profile.BannedUntil.HasValue) -> THERE IS AN ISSUE HERE.
                //{
                    userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                //Add Player to List
                resultPlayers.Add(userData);

                //}

            }

            },

                LeaderboardFailureCallbackk); // Leaderboard error callback

            return resultPlayers;
        }

        //Request Failed - Debug and Display the issue in the Console.
        private static void LeaderboardFailureCallbackk(PlayFabError error)
        {
            Debug.LogWarning("LeaderBoard Data < GET REQUEST > is Failed: ");

            Debug.LogError(error.GenerateErrorReport());
        }

        /*******************************************************************************************************************************/

        public static void RequestLeaderboardWorld(string leaderboardName) // FOR TEST
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            PlayerProfileViewConstraints rofileConstraints = new PlayerProfileViewConstraints();

            rofileConstraints.ShowAvatarUrl = true;

            rofileConstraints.ShowBannedUntil = true;

            rofileConstraints.ShowDisplayName = true;

            //The ResultPlayer Elem.
            ResultPlayer userData;

            PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
            {
                StatisticName = leaderboardName, // Statistic used to rank players for this leaderboard.

                StartPosition = 0, // Position in the leaderboard to start this listing(defaults to the first entry).

                MaxResultsCount = 5,  // Maximum number of entries to retrieve. Default 10, maximum 100.

                ProfileConstraints = rofileConstraints  // Determines which properties of the resulting player profiles to return
            },

            result => { // Leaderboard access succeed.

            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                //If the player is banned from game.
                //if (!player.Profile.BannedUntil.HasValue)
                //{
                userData.DisplayName = player.DisplayName; //Display Name

                    userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                    userData.Position = player.Position; //Player Overall Position

                    userData.StatValue = player.StatValue; //Player Score Value

                    Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                //Add Player to List
                resultPlayers.Add(userData);
                //}

            }

            },

                LeaderboardFailureCallbackk); // Leaderboard error callback
        }

        /*******************************************************************************************************************************/

        #endregion

        #region LOCAL

        // Returns a List that stores player Leaderboard information based on specified " maxResultCount "
        // LOCAL - ONLY FRIENDS //
        public static List<ResultPlayer> RequestLeaderboardOnlyFriends(int maxResultCount, string leaderboardName)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            //The ResultPlayer Elem.
            ResultPlayer userData;

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
                //If the player is banned from game.
                //if (!player.Profile.BannedUntil.HasValue)
                //{
                userData.DisplayName = player.DisplayName; // Display Name

                userData.AvatarUrl = player.Profile.AvatarUrl; // Avatar Url

                userData.Position = player.Position; // Player Overall Position

                userData.StatValue = player.StatValue; // Player Score Value

                Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                //Add Player to List
                resultPlayers.Add(userData);
                //}

            }

            },

                LeaderboardFailureCallbackk); // Leaderboard error callback

            return resultPlayers;
        }

        // Returns a List that stores player Leaderboard information based on specified " maxResultCount "
        // LOCAL - INGAME FRIENDS + FACEBOOK FRIENDS //
        public static List<ResultPlayer> RequestLeaderboardFacebookandFriends(int maxResultCount, string leaderboardName)
        {
            //The ResultPlayer List.
            List<ResultPlayer> resultPlayers = new List<ResultPlayer>();

            //The ResultPlayer Elem.
            ResultPlayer userData;

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
                //If the player is banned from game.
                //if (!player.Profile.BannedUntil.HasValue)
                //{
                userData.DisplayName = player.DisplayName; //Display Name

                userData.AvatarUrl = player.Profile.AvatarUrl; //Player URL

                userData.Position = player.Position; //Player Overall Position

                userData.StatValue = player.StatValue; //Player Score Value

                Debug.Log("Display Name: { " + userData.DisplayName + " }" + " Avatar URL: { " + userData.AvatarUrl + " }" + " Position: { " + userData.Position + " }" + " StatValue: { " + userData.StatValue + " }");

                //Add Player to List
                resultPlayers.Add(userData);
                //}

            }

            },

                LeaderboardFailureCallbackk); // Leaderboard error callback

            return resultPlayers;
        }

        #endregion

        #endregion

        #region LEADERBOARD VALIDATION

        //Get Player Statistic Data for Validation Player
        private static void AddStatisticFieldsToPlayer()
        {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),

            OnGetStatisticSuccess, //Success Callback

            OnGetStatisticError //Error Callback

            );

        }

        //Request Failed - Debug and Display the issue in the Console.
        private static void OnGetStatisticError(PlayFabError error)
        {
            Debug.LogError("LeaderBoard Error: " + error.GenerateErrorReport());
        }

        //Request Succeed - Get the Statistic Data
        private static void OnGetStatisticSuccess(GetPlayerStatisticsResult statisticsResult)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest // Update Request
            {
                Statistics = _statisticField // Statictic Fields

            }, result => OnStatisticsUpdated(result), FailureCallback);

            Debug.Log("Validation Succeed.");
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

