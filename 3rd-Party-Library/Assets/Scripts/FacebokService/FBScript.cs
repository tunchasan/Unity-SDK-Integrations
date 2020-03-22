using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{
    public Text FriendsText;

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

    // Login & Logout Field
    #region Login / Logout
    
    //Function that manages Facebook Login
    public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };

        FB.LogInWithReadPermissions(permissions);
    }

    //Function that's manage Facebook Logout
    public void FacebookLogout()
    {
        FB.LogOut();
    }

    #endregion

    //Facebook Share Field
    #region Share

    //Function that manages Facebook Share Service
    public void FacebookShareGame(string URL)
    {
        //
        URL = "https://play.google.com/store/apps/details?id=com.lilithgame.roc.gp";
        //

        FB.ShareLink(
            
            new System.Uri(URL), // App URL

            "",
            
            "",
            
            null

            );

    }

    #endregion

    // Facebook Invite Field
    #region Invite

    //Facebook Invite Friends to Game
    public void FacebookInviteFriends(string Message, string Title)
    {   
        //Facebook Invite Service Request
        FB.AppRequest(
            
            message: Message,
            
            title: Title
            
            );

    }

    //Facebook Invite Friends to Game //FOR TEST
    public void FacebookInviteFriends()
    {   
        //Facebook Invite Service Request
        FB.AppRequest(

            message: "Hey! Come and play this awesome game!",

            title: "Invite your friends to join you"

            );

    }

    #endregion

    #region Friends

    //API Request to find players in User Friend List.
    public void FacebookGetFriends()
    {
        string query = "/me/friends";

        //The Facebook Request gets friends whose play the current game.
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
