using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library.Social
{
    public class PlayFabFriends : MonoBehaviour
    {
        // ResultPlayer represents the friend unique Subfields.
        public struct ResultUser
        {
            // Title-specific display name of the user for this leaderboard entry.
            public string DisplayName;

            // URL of the player's avatar image
            public string AvatarUrl;

            //*//

            //*//

            //*//
        }

        //Determinate the Frind List data based on the ListType's elements
        public enum ListType { WITHFACEBOOK, WITHOUTFACEBOOK };

        //Determinate the "AddSubs" Method.
        public enum FriendIdType { PlayFabId, Username, Email, DisplayName };

        #region PLAYFAB LIST - FRIENDS

        //Get Player's Friends
        public List<ResultUser> GetFriends(ListType listType)
        {
            List<FriendInfo> friendList = null; // Store Friend List Data

            //The ResultPlayer List.
            List<ResultUser> resultFriends = null;

            bool withFacebook;

            // Facebook Users Handler
            switch (listType)
            {
                case ListType.WITHFACEBOOK: // List will include Player's facebook friends.
                    {
                        withFacebook = true;

                        break;
                    }

                case ListType.WITHOUTFACEBOOK: // List won't include Player's facebook friends.
                    {
                        withFacebook = false;

                        break;
                    }

                default:
                    {
                        withFacebook = false;

                        break;
                    }

            }

            // Get Friend List Request
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
            {
                IncludeSteamFriends = false,

                IncludeFacebookFriends = withFacebook,

                XboxToken = null,

                ProfileConstraints = GetProfileViewObject() // Determines which properties of the resulting player profiles to return

            }, result => {

                friendList = result.Friends;

                resultFriends = DisplayUsers(friendList); // Display friend list with this data

            }, DisplayPlayFabError);

            return resultFriends;

        }

        // Display the Friend List
        private List<ResultUser> DisplayUsers(List<FriendInfo> friendsCache)
        {
            //The ResultPlayer List.
            List<ResultUser> resultFriends = new List<ResultUser>();

            //The ResultPlayer Elem.
            ResultUser userData;

            foreach (FriendInfo friend in friendsCache)
            {
                userData.DisplayName = friend.Profile.DisplayName; //Display Name

                userData.AvatarUrl = friend.Profile.AvatarUrl; //Player URL

                Debug.Log(friend.TitleDisplayName);

                // Add Friend to List
                resultFriends.Add(userData);

            }

            return resultFriends;
        }

        // Request Error Callback
        public void DisplayPlayFabError(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }

        // Request Error Callback
        public void DisplayError(string error)
        {
            Debug.LogError(error);
        }

        #endregion

        // PlayerProfileViewConstraints Initializer for Leaderboard User Datas.
        private PlayerProfileViewConstraints GetProfileViewObject()
        {
            PlayerProfileViewConstraints profile = new PlayerProfileViewConstraints();

            profile.ShowAvatarUrl = true; // Avatar Url

            profile.ShowDisplayName = true; // Avatar Display Name

            return profile; // Return Profile
        }

    }

}
