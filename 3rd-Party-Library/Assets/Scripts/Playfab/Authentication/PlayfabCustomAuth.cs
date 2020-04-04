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

    //FOR TEST

    public GameObject _loginPanel;

    public GameObject _addLoginPanel;

    public GameObject _recoverButton;

    // Start is called before the first frame update
    private void Awake()
    {
        //Determinate, user logged in before or not.
        RememberPlayer();
    }

    #region REGISTER

    /**************************************************************************************************************/

    //Register a New Player with Email, Password and Username
    public void RegisterNewPlayer()
    {
        //Register the Player
        var registerRequest = new RegisterPlayFabUserRequest { Email = _userEmail, Password = _userPasword, Username = _userName };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    //Register Failure CallBack Function
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Email&Password Register Request...");

        Debug.LogError("Error Report: " + error.GenerateErrorReport());
    }

    //Register Success CallBack Function
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register with Email&Password request completed Succesfuly.");

        //Store Information to remember player next time.

        PlayerPrefs.SetString("EMAIL", _userEmail);

        PlayerPrefs.SetString("PASSWORD", _userPasword);

        _loginPanel.SetActive(false);
    }

    /**************************************************************************************************************/

    public void RegisterAnonymousPlayer()
    {
        //Register the Player
        var addLoginRequest = new AddUsernamePasswordRequest { Email = _userEmail, Password = _userPasword, Username = _userName };

        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAnonymousLoginSuccess, OnAnonymousLoginFailure);
    }

    //Recover Success CallBack Function
    private void OnAnonymousLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Recover *Anonymous User* request completed Succesfuly.");

        //Store Information to remember player next time.

        PlayerPrefs.SetString("EMAIL", _userEmail);

        PlayerPrefs.SetString("PASSWORD", _userPasword);

        _loginPanel.SetActive(false);

        _addLoginPanel.SetActive(false);
    }

    //Recover Failure CallBack Function
    private void OnAnonymousLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Recover *Anonymous User* Request...");

        Debug.LogError("Error Report: " + error.GenerateErrorReport());
    }

    /**************************************************************************************************************/

    #endregion

    #region AUTHENTICATE

    /**************************************************************************************************************/

    //Authenticate via EMAIL and PASSWORD
    #region EMAIL-PASSWORD

    //Login with email and password.
    public void LoginWithEmailANDPassword()
    {
        var request = new LoginWithEmailAddressRequest { Email = _userEmail, Password = _userPasword };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    //Login Failure CallBack Function
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with Email&Password Auth. Request...");

        Debug.LogError("Error Report: " + error.GenerateErrorReport());
    }

    //Login Success CallBack Function
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Authenticated " + "( " + _userEmail + " ) " + "Succeed");

        //Store Information to remember player next time.

        PlayerPrefs.SetString("EMAIL", _userEmail);

        PlayerPrefs.SetString("PASSWORD", _userPasword);

        _loginPanel.SetActive(false);

        _addLoginPanel.SetActive(false);

        _recoverButton.SetActive(false);
    }

    /**************************************************************************************************************/

    #endregion

    //Logining with DeviceID as Anonymous
    #region DEVICE ( ANDROID - IOS )

    /**************************************************************************************************************/

    public void AnonymousLogin(bool linkAction)
    {

        if (linkAction) // is this Link Mobile ID action ?
        {
            LinkWithMobileID(); // Link with MobileID
        }

        else
        {
            #if UNITY_ANDROID // On Android

            //Login Request with Android Device
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
    
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);

            #endif

            #if UNITY_IOS // On IOS

            //Login Request with IOS Device
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };

            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);

            #endif
        }
        
    }

    //Android - Login Failure CallBack Function
    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.LogError("Mobile Login Error Report: " + error.GenerateErrorReport());
    }

    //Android - Login Success CallBack Function
    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Login with DeviceID request completed Succesfuly.");

        _loginPanel.SetActive(false);
    }

    //Get Mobile Unique ID
    private static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;

        return deviceID;
    }

    /**************************************************************************************************************/

    #endregion

    #endregion

    #region RECOVER

    //Remember the Player and recover information to login automaticly.
    private void RememberPlayer()
    {
        PlayerPrefs.DeleteAll();

        _addLoginPanel.SetActive(false);
        _recoverButton.SetActive(false);

        //Remember the Player
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            //Get Stored Information and Login with the data.

            _userEmail = PlayerPrefs.GetString("EMAIL");

            _userPasword = PlayerPrefs.GetString("PASSWORD");

            var request = new LoginWithEmailAddressRequest { Email = _userEmail, Password = _userPasword };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        
        else
        {
            //AnonymousLogin();

            //_addLoginPanel.SetActive(false);
        }
    }

    #endregion

    #region LINK

    // Link Account with MobileIDs
    private void LinkWithMobileID()
    {
        #if UNITY_ANDROID // On ANDROID

        //Link user account with ANDROID ID
        PlayFabClientAPI.LinkAndroidDeviceID(new LinkAndroidDeviceIDRequest()
        {
            AndroidDeviceId = ReturnMobileID()

        }, (result) =>

        {
            Debug.Log("Account Linked With Android DeviceID Succeed.");
        },

        OnLoginMobileFailure); // Error Callback

        #endif

        #if UNITY_IOS // On IOS

        //Link user account with IOS ID
        PlayFabClientAPI.LinkIOSDeviceID(new LinkIOSDeviceIDRequest()
        {
            DeviceId = ReturnMobileID()

        }, (result) =>

        {
            Debug.Log("Account Linked With IOS DeviceID Succeed.");
        },

        OnLoginMobileFailure); // Error Callback

        #endif
    }

    #endregion

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

    //FOR TEST
    public void OpenAddLogin()
    {
        _addLoginPanel.SetActive(true);

        _recoverButton.SetActive(false);
    }

    //Remove Player Stored Auth. Data
    public void RemovePlayerStoredAuthData()
    {
        PlayerPrefs.DeleteKey("EMAIL");

        PlayerPrefs.DeleteKey("PASSWORD");
    }

}

