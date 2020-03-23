// Import statements introduce all the necessary classes for this example.
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;

public class PlayfabFacebook : MonoBehaviour
{
    // Holds the latest message to be displayed on the screen.
    private string _debugMessage;

    #region AUTHENTICATON

    private void Start()
    {
        DebugLogHandler("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method

        // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
        FB.Init(OnFacebookInitialized);
    }

    //Control User's Auth. Status
    private bool FacebookLoggedIn()
    {
        return FB.IsLoggedIn;
    }

    private void OnFacebookInitialized()
    {     
        // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
        if (FacebookLoggedIn())
            FB.LogOut();
    }

    //Facebook Auth. Handler
    public void Auth()
    {
        DebugLogHandler("Logging into Facebook...");

        // We invoke basic login procedure and pass in the callback to process the result
        FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
    }

    private void OnFacebookLoggedIn(ILoginResult result)
    {
        // If result has no errors, it means we have authenticated in Facebook successfully
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            DebugLogHandler("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");

            /*
             * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
             * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
             */
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        }
        else
        {
            // If Facebook authentication failed, we stop the cycle with the message
            DebugLogHandler("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
        }
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

    #endregion

    #region SHARE

    //Function that manages Facebook Share Service
    public void Share(string URL)
    {
        //Control "LoggedIn" Facebook or not ?
        if (FacebookLoggedIn())
        {
            URL = "https://www.youtube.com/watch?v=BVomQtrtMTM"; // FOR TEST

            //Share Request with FB.ShareLink
            FB.ShareLink(

                new System.Uri(URL), // Content URL

                "", // Content Title

                "", // Content Description

                null,   // Content Photo URL

                OnFacebookShared); // CallBack
        }

        //If we did not "LoggedIn" FB, -> Auth()
        else { Auth(); }
    }

    //Facebook Share Callback method to catch the errors.
    private void OnFacebookShared(IShareResult result)
    {
        // If result has no errors, it means we have shared the Content in Facebook successfully
        if (result == null || string.IsNullOrEmpty(result.Error))
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

}