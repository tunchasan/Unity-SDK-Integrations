using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{
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
                    Time.timeScale = 0;
                else
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

    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("https://store.steampowered.com/app/1174200/Lands_of_Pharaoh_Episode_1/"),
            "Check it Out!",
            "Good Egypt Game",
            new System.Uri("https://i.hizliresim.com/hxxahO.jpg")
            );
    }

    #endregion

    #region Invite

    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Lands of Pharaoh");
    }

    #endregion
}
