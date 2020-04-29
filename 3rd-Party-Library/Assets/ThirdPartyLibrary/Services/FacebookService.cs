// Import statements introduce all the necessary classes for this example.

using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;
using System;
using Library.Authentication;

namespace Library.FaceBook
{
    public class FacebookService
    {
        // Holds the latest message to be displayed on the screen.
        private string _debugMessage;

        #region AUTHENTICATON

        public FacebookService()
        {
            //DebugLogHandler("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method

            // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
            FB.Init();

            // In the "OnFacebookInitialized" function didn't invoke by FB.Init, we invoke it here.
            OnFacebookInitialized();
        }

        //Control User's Auth. Status
        public static bool GetLoggedIn()
        {
            return FB.IsLoggedIn;
        }

        private void OnFacebookInitialized()
        {
            // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
            if (GetLoggedIn())
                FB.LogOut();
        }

        //Facebook Auth. Handler
        public void AuthLogin(bool linkAction, Action<bool, string, bool> actionStatus)
        {
            DebugLogHandler("Logging into Facebook...");

            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(

                null,

                (result) =>

                {
                    // If result has no errors, it means we have authenticated in Facebook successfully
                    if ((result == null || string.IsNullOrEmpty(result.Error)) && !result.Cancelled)
                    {
                        DebugLogHandler("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");

                        if (linkAction) // is this Facebook Linking Action ?
                        {
                            LinkWithFacebook(AccessToken.CurrentAccessToken.TokenString, actionStatus); // Link Accout with Facebook
                        }

                        else
                        {
                            PlayFabFacebookLogin(AccessToken.CurrentAccessToken.TokenString); // Just Login with Facebook
                        }
                    }

                    else
                    {
                        if(actionStatus != null)
                        { 
                            actionStatus(false, "Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, false);
                        }

                        else
                        {
                            //If Facebook authentication failed, we stop the cycle with the message
                            DebugLogHandler("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
                        }
                        
                    }

                }

            );

        }

        //Logout from Facebook and reset Facebook Auth. procedure.
        public void AuthLogout()
        {
            DebugLogHandler("Logout from Facebook...");

            FB.LogOut();
        }

        //Login with Facebook
        public void PlayFabFacebookLogin(string token)
        {
            /* We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
            and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results*/

            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = token, InfoRequestParameters = InfoRequest() },
                OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        }

        // When processing both results, we just set the message, explaining what's going on.
        private void OnPlayfabFacebookAuthComplete(LoginResult result)
        {
            DebugLogHandler("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        }

        private void OnPlayfabFacebookAuthFailed(PlayFabError error)
        {
            DebugLogHandler("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport(), true);
        }

        //Fuction that handles the console display MESSAGE based on Auth Process's Action's Output.
        private void DebugLogHandler(string message, bool error = false)
        {
            //The Log Message
            _debugMessage = message;

            //If "error" flag is true, Print the error message in Console.
            if (error)
                Debug.LogError(_debugMessage);

            //If "error" flag is false, Print the error message in Console.
            else
                Debug.Log(_debugMessage);
        }

        // Set-Up PayloadData
        private GetPlayerCombinedInfoRequestParams InfoRequest()
        {
            GetPlayerCombinedInfoRequestParams request = new GetPlayerCombinedInfoRequestParams();

            request.GetPlayerProfile = true;

            return request;
        }

        #endregion

        #region SHARE

        //Function that manages Facebook Share Service
        public void Share(string URL)
        {
            //If the user loggedIn Facebook, We control the Callback.
            if (GetLoggedIn())
            {
                URL = "https://www.youtube.com/watch?v=BVomQtrtMTM"; // FOR TEST

                //Share Request with FB.ShareLink
                FB.ShareLink(

                    contentURL: new System.Uri(URL), // Content URL

                    "", // Content Title

                    "", // Content Description

                    null,   // Content Photo URL

                    callback: OnFacebookShared); // CallBack
            }

            else
                AuthLogin(false, null); //Facebook Auth. Handler

        }

        //Facebook Share Callback method to catch the errors.
        private void OnFacebookShared(IShareResult result)
        {
            // If result has no errors, it means we have shared the Content in Facebook successfully
            if (!result.Cancelled)
            {
                DebugLogHandler("Facebook Sharing Completed Successfully!");
            }

            else
            {
                // If Facebook Sharing failed, we stop the cycle with the message
                DebugLogHandler("Facebook Sharing Failed: " + result.Error + "\n" + result.RawResult, true);
            }

        }

        #endregion

        #region INVITATION

        //Facebook Invite Friends to Game
        public void Invite()
        {
            //If the user loggedIn Facebook, We control the Callback.
            if (GetLoggedIn())
            {
                //FOR TEST*********************
                string Message = "Come on and Play!";

                string Title = "Smash Game";
                //*****************************

                //Facebook Invite Service Request
                FB.AppRequest(

                    message: Message, // Message

                    title: Title, // Invite FB Title

                    callback: OnFacebookInvited  // CallBack

                    );

            }

            else
                AuthLogin(false, null); //Facebook Auth. Handler

        }

        //Facebook Invitation Callback method to catch the errors.
        private void OnFacebookInvited(IAppRequestResult result)
        {
            // If result has no errors, it means we have shared the Content in Facebook successfully
            if (!(result.RequestID == null || string.IsNullOrEmpty(result.RequestID)))
            {
                DebugLogHandler("Facebook Invitation Completed Successfully!" + " RequestID: " + result.RequestID);
            }

            else
            {
                // If Facebook Sharing failed, we stop the cycle with the message
                DebugLogHandler("Facebook Invitation Failed: " + result.Error + "\n" + result.RawResult, true);
            }

        }

        #endregion

        #region FRIENDS

        //API Request to find players in User's Facebook Friend List.
        public void FetchFriends()
        {
            string query = "/me/friends";

            //The Facebook Request gets friends whose play the current game.
            FB.API(

                query, // Facebook query

                HttpMethod.GET, // API Get Request

                callback: OnFacebookFetchFriends // Callback

            );

        }

        //Facebook FetchFriends Callback method to catch the errors.
        private void OnFacebookFetchFriends(IGraphResult result)
        {
            //"FriendsText" stores request answer info.
            string FriendsText = string.Empty;

            var Dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);

            //"FriendList" User's Facebook Friend List
            var FriendList = (List<object>)Dictionary["data"];

            //Get Player name whose play this game.
            foreach (var dict in FriendList)
            {
                FriendsText += " ( " + ((Dictionary<string, object>)dict)["name"] + " )";
            }

            if (string.IsNullOrEmpty(FriendsText))
            {
                DebugLogHandler("Facebook GetFriends Failed" + result.Error + "\n" + result.RawResult, true);
            }

            else
                DebugLogHandler("Facebook FetchFriends Completed Successfully!" + "\n" + FriendsText);
        }

        #endregion

        #region LINK - UNLINK

        // Link account with Facebook
        public void LinkWithFacebook(string accessToken, Action<bool, string, bool> actionStatus)
        {
            PlayFabClientAPI.LinkFacebookAccount(new LinkFacebookAccountRequest() // Facebook Linking Request
            {
                AccessToken = accessToken

            }, (result) =>

            {
                //Request Facebook DisplayName
                this.GetFacebookDisplayName();

                actionStatus(true, "Account Linked With Facebook Succeed.", false);

                //Debug.Log("Account Linked With Facebook Succeed.");
            },

            (error) => 
            { 
                OnPlayfabFacebookLinkFailed(error, actionStatus);

            }); // Error Callback

        }

        // Link Error Callback
        private void OnPlayfabFacebookLinkFailed(PlayFabError error, Action<bool, string, bool> actionStatus)
        {
            // Specified Error Code for RECOVER
            if (error.Error == PlayFabErrorCode.LinkedAccountAlreadyClaimed) // Facebook Acc. is already used by another user.
            {
                Debug.LogWarning("The Facebook Account is already used by another user.");

                // Get User Name
                FB.API("me?fields=name", HttpMethod.GET, 
                    
                    (result) =>
                    {
                        // If result has no errors
                        if (string.IsNullOrEmpty(result.Error))
                        {
                            if (PlayfabCustomAuth.ISGuestAccount()) // If the account is not guest
                            {
                                string fbName = result.ResultDictionary["name"].ToString();

                                actionStatus(false, "Do you want to load " + fbName + "'s game ?", true);
                            }

                            else
                            {
                                actionStatus(false, error.GenerateErrorReport(), false);

                                FB.LogOut(); // LOGOUT FACEBOOK
                            }
                            
                        }

                        else
                        {
                            // If Facebook request failed, we stop the cycle with the message
                            actionStatus(false, "Facebook Failed: " + result.Error + "\n" + result.RawResult, false);

                            FB.LogOut(); // LOGOUT FACEBOOK
                        }
                    });

            }

            else
            {
                // Debug.LogError(error.GenerateErrorReport());

                actionStatus(false, error.GenerateErrorReport(), false);

                FB.LogOut(); // LOGOUT FACEBOOK
            }
        }

        // Unlink account with Facebook
        public void UnLinkWithFacebook(Action<bool, string> actionStatus)
        {
            PlayFabClientAPI.UnlinkFacebookAccount(new UnlinkFacebookAccountRequest()
            {

            }, (result) =>

            {
                FB.LogOut(); // Logout Facebook

                // Reset Display Name for Facebook
                this.ResetDisplayName();

                actionStatus(true, "Account UnLinked With Facebook Succeed.");

                // Debug.Log("Account UnLinked With Facebook Succeed.");
            },

            (error) =>
            {
                actionStatus(false, "PlayFab Facebook Auth Failed: " + error.GenerateErrorReport());

                // Debug.LogError("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport());
            });

        }

        #endregion

        #region DISPLAY NAME

        /*********************************************SET*******************************************/

        // Get DisplayName Request to Facebook
        private void GetFacebookDisplayName()
        {
            FB.API("me?fields=name", HttpMethod.GET, GetFacebookData);
        }

        private void GetFacebookData(IGraphResult result)
        {
            // If result has no errors
            if (string.IsNullOrEmpty(result.Error))
            {
                string fbName = result.ResultDictionary["name"].ToString();

                //PlayFab Set DisplayName Request
                this.SetDisplayName(fbName);
            }

            else
            {
                // If Facebook request failed, we stop the cycle with the message
                DebugLogHandler("Facebook Failed: " + result.Error + "\n" + result.RawResult, true);
            }

        }

        // Set DisplayName Request to PlayFab
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

            PlayerPrefs.SetString("DISPLAYNAME_FACEBOOK", result.DisplayName);
        }

        /******************************************RESET**********************************************/

           
        // Reset DisplayName
        private void ResetDisplayName()
        {
            PlayerPrefs.DeleteKey("DISPLAYNAME_FACEBOOK");

            // Reset as GPGS DisplayName
            if (PlayerPrefs.HasKey("DISPLAYNAME_GPGS"))
            {
                ResetDisplayName(PlayerPrefs.GetString("DISPLAYNAME_GPGS"));
            }

            // Reset as Guest DisplayName
            else
            {
                ResetDisplayName(PlayerPrefs.GetString("DISPLAYNAME_GUEST"));
            }
        }

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

        public static bool IsLinkedWithFacebook()
        {
            return PlayerPrefs.HasKey("DISPLAYNAME_FACEBOOK");
        }

        /**********************************************************************************************/

        #endregion

        #region ACCOUNT RECOVER

        // User wants to recover account, Recover it.  *** Yes Click Event ***
        public void RecoverAccount(Action<bool, string> actionStatus)
        {
            // Login Accout
            Debug.Log("Account Recovered.");

            Debug.Log("New Server Auth Code: " + AccessToken.CurrentAccessToken.TokenString);

            PlayFabFacebookLogin(AccessToken.CurrentAccessToken.TokenString);

            actionStatus(true, "Recover succesfully completed.");
        }

        // User dont want to recover accout, Keep current.  *** No Click Event ***
        public void DontRecoverAccount()
        {
            // Logout Facebook
            FB.LogOut();
        }

        #endregion
        /*
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "DeleteUserByID",

                FunctionParameter = new { PlayFabId = PlayfabCustomAuth.playfabID },

                GeneratePlayStreamEvent = true,

            }, xx, yy);*/
    }
}


