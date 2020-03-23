using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabCustomAuth : MonoBehaviour
{
    private string _userEmail;
    private string _userPasword;
    private string _userName;

    public GameObject panel;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        //Remember the Player
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            _userEmail = PlayerPrefs.GetString("EMAIL");

            _userPasword = PlayerPrefs.GetString("PASSWORD");

            var request = new LoginWithEmailAddressRequest { Email = _userEmail, Password = _userPasword };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Email&Password Auth. Request...");

        Debug.LogError("Error Report: " + error.GenerateErrorReport());

        //Register New Player
        OnRegisterPlayer();
    }

    private void OnRegisterPlayer()
    {
        //Register the Player
        var registerRequest = new RegisterPlayFabUserRequest { Email = _userEmail, Password = _userPasword, Username = _userName };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Email&Password Register Request...");

        Debug.LogError("Error Report: " + error.GenerateErrorReport());
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register with Email&Password request completed Succesfuly.");

        //Remember the user for next time.

        PlayerPrefs.SetString("EMAIL", _userEmail);

        PlayerPrefs.SetString("PASSWORD", _userPasword);

        panel.SetActive(false);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Auth. with Email&Password request completed Succesfuly.");

        PlayerPrefs.SetString("EMAIL", _userEmail);

        PlayerPrefs.SetString("PASSWORD", _userPasword);

        panel.SetActive(false);
    }

    public void GetUserEmail(string emailIn)
    {
        _userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        _userPasword = passwordIn;
    }

    public void GetUserName(string usernameIn)
    {
        _userName = usernameIn;
    }

    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = _userEmail, Password = _userPasword };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
}
