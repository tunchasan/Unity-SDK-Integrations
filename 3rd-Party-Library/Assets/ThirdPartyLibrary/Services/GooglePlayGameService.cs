using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Library.Authentication;

namespace Library.GooglePlay
{
    using GooglePlayGames;

    using GooglePlayGames.BasicApi;
    using System;
    using UnityEngine;

    public class GooglePlayGameService
    {
        public static bool LoggedIn { get; private set; }

        public GooglePlayGameService()
        {
            // The following grants profile access to the Google Play Games SDK.
            // Note: If you also want to capture the player's Google email, be sure to add
            // .RequestEmail() to the PlayGamesClientConfiguration
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();

            PlayGamesPlatform.InitializeInstance(config);

            // Recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();

            //Initialize the Auth. status
            LoggedIn = false;

            //RememberGoogleAccount();
        }

        #region AUTHENTICATE

        public void LoginPlayGameService(bool linkAction, Action<bool, string, bool> actionStatus)
        {
            //Authenticate the user request
            Social.localUser.Authenticate((bool success) => {

                if (success) // If "Auth" is success
                {
                    //Get "ServerAuthCode" and assing it.
                    string serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                    Debug.Log("Server Auth Code: " + serverAuthCode);

                    if (linkAction) // is GPGS Linking Action ?
                    {
                        LinkWithGooglePlayAccount(serverAuthCode, actionStatus);
                    }

                    else
                    {
                        // Login in with PlayFab
                        LoginWithGoogleAccout(serverAuthCode);
                    }

                }

                else
                {
                    //Debug.Log("Google Failed to Authorize your login");

                    actionStatus(false, "Google Failed to Authorize your login", false);
                }

            });
        }

        // GPGS login data with PlayFab Integration.
        public void LoginWithGoogleAccout(string authCode)
        {
            GetPlayerCombinedInfoRequestParams requestParams = PlayfabCustomAuth.LoginPayloadRequestSetter();

            //PlayFab Google Account Integration
            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,

                ServerAuthCode = authCode,

                CreateAccount = true,

                InfoRequestParameters = requestParams

            }, (result) =>

            {   
                string entityID = result.EntityToken.Entity.Id;

                string entityType = result.EntityToken.Entity.Type;

                Debug.Log("[6] Logged in as " + Social.localUser.userName);

                LoggedIn = true; // Logged in user

                PlayfabCustomAuth.PlayFabID = result.PlayFabId; // Update PlayFabID

                PlayfabCustomAuth.UserDisplayName = result.InfoResultPayload.PlayerProfile.DisplayName; // Update DisplayName

                PlayerPrefs.SetString("GPGSAUTH", "success"); // PlayFab GPGS Auth succeed.

            },

            OnPlayFabError); // Error Callback

        }

        //Error Callback
        private void OnPlayFabError(PlayFabError error)
        {
            PlayerPrefs.SetString("GPGSAUTH", "failed"); // PlayFab GPGS Auth failed.

            Debug.LogError(error.Error);

            Debug.LogError("GPGS PlayFab - Error Report: " + error.GenerateErrorReport());
        }

        #endregion

        #region RECOVER

        //Remember User whose signed in with Google Acc.
        public void RememberGoogleAccount()
        {
            if (LoggedInBefore())
            {
                LoginPlayGameService(false, null); // Login Google Acc. automaticly
            }
        }

        // Player loggenIn with GPGS before ?
        public bool LoggedInBefore()
        {
            if (PlayerPrefs.HasKey("GPGSAUTH"))
            {
                if (PlayerPrefs.GetString("GPGSAUTH").Equals("success"))
                    return true;

                else
                    return false;
            }

            else
                return false;
        }

        #endregion

        #region LINK - UNLINK

        // Link user account with GPGS
        private void LinkWithGooglePlayAccount(string authCode, Action<bool, string, bool> actionStatus)
        {
            PlayFabClientAPI.LinkGoogleAccount(new LinkGoogleAccountRequest()
            {
                ServerAuthCode = authCode

            }, (result) =>

            {
                //Debug.Log("Account Linked With Google Play Succeed.");

                PlayerPrefs.SetString("GPGSAUTH", "success"); // PlayFab GPGS Auth succeed.

                LoggedIn = true; // Logged in user

                //Request PlayFab DisplayName
                this.SetDisplayName(Social.localUser.userName);

                actionStatus(true, "Account Linked With Google Play Succeed.", false);
            },

            (error) =>
            {
                OnPlayFabLinkError(error, actionStatus);
            });

        }

        private void OnPlayFabLinkError(PlayFabError error, Action<bool, string, bool> actionStatus)
        {
            // Specified Error Code
            if (error.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed) // GPGS Acc. is already used by another user.
            {
                if (PlayfabCustomAuth.ISGuestAccount())
                {
                    Debug.LogError("The Google Play Account is already used by another user.");

                    actionStatus(false, "Do you want to load " + Social.localUser.userName + "'s game ?", true);
                }

                else
                {
                    actionStatus(false, error.GenerateErrorReport(), false);

                    // Logout from PlayGames
                    PlayGamesPlatform.Instance.SignOut();

                    LoggedIn = false; // Logged in user -> false
                }

            }

            else
            {
                //Debug.LogError(error.GenerateErrorReport());

                actionStatus(false, error.GenerateErrorReport(), false);

                // Logout from PlayGames
                PlayGamesPlatform.Instance.SignOut();

                LoggedIn = false; // Logged in user -> false
            }
        }

        // UnLink user account with GPGS
        public void UnLinkWithGooglePlayAccount(Action<bool,string> actionStatus)
        {
            PlayFabClientAPI.UnlinkGoogleAccount(new UnlinkGoogleAccountRequest()
            {

            }, (result) =>

            {
                PlayGamesPlatform.Instance.SignOut();

                LoggedIn = false; // Logged in user -> false

                //Reset Display Name
                this.ResetDisplayName();

                actionStatus(true, "Account UnLinked With Google Play Succeed.");
            },

            (error) =>
            {
                actionStatus(false, error.GenerateErrorReport());

            }); // Error Callback

        }

        #endregion

        #region DISPLAY NAME

        /*********************************************SET*******************************************/

        private void SetDisplayName(string displayName)
        {
            //Display Name Request
            var requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = displayName };

            PlayFabClientAPI.UpdateUserTitleDisplayName(requestDisplayName, OnDisplayNameSuccess, OnDisplayNameFailure);
        }

        //Display Name Error Callback
        private void OnDisplayNameFailure(PlayFabError error)
        {
            Debug.LogError("Display Name Change Error: " + error.GenerateErrorReport());
        }

        //Display Name Succeed Callback
        private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Display Name Changed: " + result.DisplayName);

            PlayerPrefs.SetString("DISPLAYNAME_GPGS", result.DisplayName);

            PlayfabCustomAuth.UserDisplayName = result.DisplayName;
        }

        /******************************************RESET**********************************************/

        private void ResetDisplayName(string displayName)
        {
            //Display Name Request
            var requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = displayName };

            PlayFabClientAPI.UpdateUserTitleDisplayName(requestDisplayName, OnResetDisplayNameSuccess, OnResetDisplayNameFailure);
        }

        // Reset DisplayName Error Callback
        private void OnResetDisplayNameFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }

        // Reset DisplayName Callback
        private void OnResetDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Display Name Changed: " + result.DisplayName);

            PlayfabCustomAuth.UserDisplayName = result.DisplayName;
        }

        private void ResetDisplayName()
        {
            PlayerPrefs.DeleteKey("DISPLAYNAME_GPGS");

            PlayerPrefs.DeleteKey("GPGSAUTH");

            // Reset as Facebook DisplayName
            if (PlayerPrefs.HasKey("DISPLAYNAME_FACEBOOK"))
            {
                ResetDisplayName(PlayerPrefs.GetString("DISPLAYNAME_FACEBOOK"));
            }

            // Reset as Guest DisplayName
            else
            {
                ResetDisplayName(PlayerPrefs.GetString("DISPLAYNAME_GUEST"));
            }
        }

        /************************************************************************************************/

        #endregion

        #region ACCOUNT RECOVER

        // User wants to recover account, Recover it.  *** Yes Click Event ***
        public void RecoverAccount()
        {
            PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, (string serverAuthCode) =>
            {
                // Login Accout
                Debug.Log("Account Recovered.");

                Debug.Log("New Server Auth Code: " + serverAuthCode);

                //Login in with PlayFab
                LoginWithGoogleAccout(serverAuthCode);

            });
        }

        // User dont want to recover accout, Keep current.  *** No Click Event ***
        public void DontRecoverAccount()
        {
            // Logout from PlayGames
            PlayGamesPlatform.Instance.SignOut();

            LoggedIn = false; // Logged in user -> false
        }

        #endregion

        //Return user Display Name
        public string GetUserName()
        {
            return PlayGamesPlatform.Instance.GetUserDisplayName();
        }

    }

}

