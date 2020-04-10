using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Library.Authentication.GooglePlay
{
    using GooglePlayGames;

    using GooglePlayGames.BasicApi;

    using UnityEngine;

    public class PlayFabGPGS
    {
        // Store Recover PopUp Menu Ref.
        private GameObject _recoverPopUpMenu;

        // Popup Text
        private string _popUpText;

        // Store loggedIn status
        private bool LoggedIn;

        public PlayFabGPGS(GameObject _PopUpMenu)
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

            // Initialize Game Object
            _recoverPopUpMenu = _PopUpMenu;

            //Remember User
            RememberGoogleAccount();
        }

        #region AUTHENTICATE

        public void LoginPlayGameService(bool linkAction)
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
                        LinkWithGooglePlayAccount(serverAuthCode);
                    }

                    else
                    {
                        // Login in with PlayFab
                        LoginWithGoogleAccout(serverAuthCode);
                    }

                }

                else
                {
                    Debug.Log("Google Failed to Authorize your login");
                }

            });
        }

        // GPGS login data with PlayFab Integration.
        public void LoginWithGoogleAccout(string authCode)
        {
            //PlayFab Google Account Integration
            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,

                ServerAuthCode = authCode,

                CreateAccount = true

            }, (result) =>

            {
                Debug.Log("Signed In as " + result.PlayFabId);

                string entityID = result.EntityToken.Entity.Id;

                string entityType = result.EntityToken.Entity.Type;

                LoggedIn = true; // Logged in user

                PlayerPrefs.SetString("GPGSAUTH", "success"); // PlayFab GPGS Auth succeed.

                /********************************************************************************************/
                /*CLOUD SAVE*/

                //Determinete the cloud Service Type in { "FILE", "STATISTIC", "OBJECT" }
                /*string cloudMethodType = (CSPlayFabMaster.cloudType.DATATABLE)
                                          .ToString();

                //CloudSave Instance
                CSPlayFabMaster cloud = new CSPlayFabMaster(entityID, entityType, cloudMethodType);*/

                /********************************************************************************************/

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
        private void RememberGoogleAccount()
        {
            if (LoggedInBefore())
            {
                LoginPlayGameService(false); // Login Google Acc. automaticly
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
        private void LinkWithGooglePlayAccount(string authCode)
        {
            PlayFabClientAPI.LinkGoogleAccount(new LinkGoogleAccountRequest()
            {
                ServerAuthCode = authCode

            }, (result) =>

            {
                Debug.Log("Account Linked With Google Play Succeed.");

                PlayerPrefs.SetString("GPGSAUTH", "success"); // PlayFab GPGS Auth succeed.

                LoggedIn = true; // Logged in user

                //Request PlayFab DisplayName
                this.SetDisplayName(Social.localUser.userName);
            },

            OnPlayFabLinkError); // Error Callback

        }

        private void OnPlayFabLinkError(PlayFabError error)
        {
            // Specified Error Code
            if (error.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed) // GPGS Acc. is already used by another user.
            {
                Debug.LogError("The Google Play Account is already used by another user.");

                AccountRecoverWithGPGS(); // Account Recover with GPGS Account.
            }

            else
            {
                Debug.LogError(error.GenerateErrorReport());
            }
        }

        // UnLink user account with GPGS
        public void UnLinkWithGooglePlayAccount()
        {
            PlayFabClientAPI.UnlinkGoogleAccount(new UnlinkGoogleAccountRequest()
            {

            }, (result) =>

            {
                PlayGamesPlatform.Instance.SignOut();

                LoggedIn = false; // Logged in user -> false

                //Reset Display Name
                this.ResetDisplayName();

                Debug.Log("Account UnLinked With Google Play Succeed.");
            },

            OnPlayFabError); // Error Callback

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

        // Initialize Recover Acc. Action
        private void AccountRecoverWithGPGS()
        {
            Debug.Log("Do you want to load " + Social.localUser.userName + "'s game ?");

            // Debug.LogWarning("Warning: progress in the current game will be saved. You can load the current game by next login.");

            _popUpText = "Do you want to load " + Social.localUser.userName + "'s game ?";

            // Create PopUp
            RecoverPopUpMenu(true);
        }

        // Handle PopUpMenu Visibility
        private void RecoverPopUpMenu(bool Visibilty)
        {
            // Enable PopUp Menu
            _recoverPopUpMenu.SetActive(Visibilty);
        }

        // User wants to recover account, Recover it.  *** Yes Click Event ***
        public void RecoverAccount()
        {
            // Hidden PopUp Menu
            RecoverPopUpMenu(false);

            PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, (string serverAuthCode) =>
            {
                Debug.Log("New Server Auth Code: " + serverAuthCode);

                //Login in with PlayFab
                LoginWithGoogleAccout(serverAuthCode);

            });
        }

        // User dont want to recover accout, Keep current.  *** No Click Event ***
        public void DontRecoverAccount()
        {
            // Hidden PopUp Menu
            RecoverPopUpMenu(false);

            // Logout from PlayGames
            PlayGamesPlatform.Instance.SignOut();

            LoggedIn = false; // Logged in user -> false

        }

        // PopUp Text
        public string GetRecoverPopUpText()
        {
            return _popUpText;
        }

        #endregion

        //Return user Display Name
        public string GetUserName()
        {
            return PlayGamesPlatform.Instance.GetUserDisplayName();
        }

        //Return Auth. Status
        public bool GetLoggedIn()
        {
            return LoggedIn;
        }
    }
}

