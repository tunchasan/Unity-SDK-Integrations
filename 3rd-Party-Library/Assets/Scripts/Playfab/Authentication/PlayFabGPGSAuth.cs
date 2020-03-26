using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class PlayFabGPGSAuth : MonoBehaviour
{
    public void Start()
    {
        // The following grants profile access to the Google Play Games SDK.
        // Note: If you also want to capture the player's Google email, be sure to add
        // .RequestEmail() to the PlayGamesClientConfiguration
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .AddOauthScope("profile")
        .RequestServerAuthCode(false)
        .Build();

        PlayGamesPlatform.InitializeInstance(config);

        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void Click()
    {
        Social.localUser.Authenticate((bool success) => {

            if (success)
            {
                object x = new object();
                Debug.Log("Google Signed In");
                var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode(x);
                Debug.Log("Server Auth Code: " + serverAuthCode);

                PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    ServerAuthCode = serverAuthCode,
                    CreateAccount = true
                }, (result) =>
                {
                    Debug.Log("Signed In as " + result.PlayFabId);

                }, OnPlayFabError);
            }
            else
            {
                
                Debug.Log("Google Failed to Authorize your login");
            }

        });
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        throw new NotImplementedException();
    }
}
