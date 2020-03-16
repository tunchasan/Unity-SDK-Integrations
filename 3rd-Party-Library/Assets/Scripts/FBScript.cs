using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{
    public Text FriendsText;

    /******************************FB INITIALIZATION PART******************************/
    private void Awake()
    {
        //if Facebook is not initialized, Initialized the FB Services then active the FB App
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                //is FB Actived ?
                if (FB.IsInitialized)
                    //Active the FB App
                    FB.ActivateApp();
                else
                    //İf the FB App is not actived, Debug it as error Message.
                    Debug.LogError("Couldn't initialize");

            }, isGameShown =>
            {
                if (!isGameShown)
                    // Pause the game - we will need to hide
                    Time.timeScale = 0;
                else
                    // Resume the game - we're getting focus again
                    Time.timeScale = 1;
            });
                
        }
        //if Facebook is Initialized, Directly Active the FB App.
        else
        {
            FB.ActivateApp();
        }
    }

    /**************************************************************************************/

    #region Login / Logout
    
    public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };

        FB.LogInWithReadPermissions(permissions);
    }

    public void FacebookLogout()
    {
        FB.LogOut();
    }

    #endregion

    #region Share

    public void FacebookShare(string URL)
    {
        //
        URL = "https://play.google.com/store/apps/details?id=com.lilithgame.roc.gp";
        //

        FB.ShareLink(
            
            new System.Uri(URL)

            );

    }

    #endregion

    #region Invite

    public void FacebookInviteFriends(string Message, string Title)
    {   //
        Message = "Hey! Come and play this awesome game!";

        Title = "Invite your friends to join you";
        //
        FB.AppRequest(
            
            message: Message,
            
            title: Title
            
            );

    }

    #endregion

    #region Friends

    public void GetFriendsOnline()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
         {
             var dictionary = (Dictionary<string,object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);

             var friendList = (List<object>)dictionary["data"];

             FriendsText.text = string.Empty;

             //Get Player name whose play this game.
             foreach(var dict in friendList)
             {
                 FriendsText.text += ((Dictionary<string,object>)dict)["name"];
             }

         });

    }

    #endregion
}
