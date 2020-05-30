using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

namespace Library.Authentication
{
    public class PlayfabCustomAuth
    {
        private string _userEmail;

        private string _userPasword;

        private string _userName;

        public static bool RecoverActionStatus { get; set; }

        // Can be used after success login
        public static string UserDisplayName { get; set; }

        // Can be used after success login
        public static string PlayFabID { get; private set; }

        // Can be used after success login
        public static bool IsFreshAccount { get; private set; }

        // Can be used after success login
        public static bool IsLinkedWithGoogle { get; private set; }

        // Can be used after success login
        public static bool IsLinkedWithFacebook { get; private set; }

        // Can be used after success login
        public static void UpdateLinkWithFacebookStatus(bool result)
        {
            IsLinkedWithFacebook = result;

            IsFreshAccount = !(IsLinkedWithGoogle || IsLinkedWithFacebook);
        }

        // Can be used after success login
        public static void UpdateLinkWithGoogleStatus(bool result)
        {
            IsLinkedWithGoogle = result;

            IsFreshAccount = !(IsLinkedWithGoogle || IsLinkedWithFacebook);
        }

        // Can be used after success login
        public static bool ISGuestAccount()
        {
            return IsFreshAccount;
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
        }

        /**************************************************************************************************************/

        #endregion

        //Logining with DeviceID as Anonymous
        #region DEVICE ( ANDROID - IOS )

        /**************************************************************************************************************/

        public void AnonymousLogin(bool linkAction, Action<bool, string> actionStatus)
        {
            if (linkAction) // is this Link Mobile ID action ?
            {
                LinkWithMobileID(actionStatus); // Link with MobileID
            }

            else
            {
                #if UNITY_ANDROID // On Android

                GetPlayerCombinedInfoRequestParams requestParams = LoginPayloadRequestSetter();

                //Login Request with Android Device
                var requestAndroid = new LoginWithAndroidDeviceIDRequest {

                    AndroidDeviceId = ReturnMobileID(),

                    CreateAccount = true,

                    InfoRequestParameters = requestParams
                };

                PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, 
                    
                    (result) =>
                    {
                        Debug.Log("Login with DeviceID request completed Succesfuly.");

                        FetchAccountData(result);

                        actionStatus(true, "Login with DeviceID request completed Succesfuly.");
                    },

                    (error) =>
                    {
                        Debug.LogError("Mobile Login Error Report: " + error.GenerateErrorReport());

                        actionStatus(false, error.GenerateErrorReport());
                    });

                #endif

                #if UNITY_IOS // On IOS

                GetPlayerCombinedInfoRequestParams requestParams = LoginPayloadRequestSetter();

                //Login Request with IOS Device
                var requestIOS = new LoginWithIOSDeviceIDRequest { 
                    
                    DeviceId = ReturnMobileID(), 
                    
                    CreateAccount = true,
                
                    InfoRequestParameters = requestParams
                };

                PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS,
                    
                    (result) =>
                    {
                        Debug.Log("Login with IOS DeviceID request completed Succesfuly.");

                        FetchAccountData(result);

                        actionStatus(true, "Login with IOS DeviceID request completed Succesfuly.");
                    },

                    (error) =>
                    {
                        Debug.LogError("IOS Mobile Login Error Report: " + error.GenerateErrorReport());

                        actionStatus(false, error.GenerateErrorReport());
                    });

                #endif
            }

        }

        public static void FetchAccountData(LoginResult result)
        {
            PlayFabID = result.PlayFabId; // Update PlayFabID

            IsFreshAccount = result.NewlyCreated;

            if (result.InfoResultPayload.AccountInfo.GoogleInfo != null)
                IsLinkedWithGoogle = true;

            if (result.InfoResultPayload.AccountInfo.FacebookInfo != null)
                IsLinkedWithFacebook = true;

            if (!IsFreshAccount)
            {
                UserDisplayName = result.InfoResultPayload.PlayerProfile.DisplayName;
            }
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
        public void RememberPlayer()
        {
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

        #region LINK - UNLINK

        // Link Account with MobileIDs
        public static void LinkWithMobileID(Action<bool, string> actionStatus)
        {
            #if UNITY_ANDROID // On ANDROID

            //Link user account with ANDROID ID
            PlayFabClientAPI.LinkAndroidDeviceID(new LinkAndroidDeviceIDRequest()
            {
                AndroidDeviceId = ReturnMobileID()

            }, (result) =>

            {
                Debug.Log("Account Linked With Android DeviceID Succeed.");

                actionStatus(true, "Account Linked With Android DeviceID Succeed.");
            },

            (error) =>
            {
                Debug.LogError("Link with MobileDeviceID: " + error.GenerateErrorReport());

                actionStatus(false, error.GenerateErrorReport());

            }); // Error Callback

        #endif

        #if UNITY_IOS // On IOS

         //Link user account with IOS ID
            PlayFabClientAPI.LinkIOSDeviceID(new LinkIOSDeviceIDRequest()
            {
                DeviceId = ReturnMobileID()

            }, 
            
            (result) =>
            {
                Debug.Log("Account Linked With IOS DeviceID Succeed.");

                actionStatus(true, "Account Linked With IOS DeviceID Succeed.");
            },

            (error) =>
            {
                Debug.LogError("Link with IOS DeviceID: " + error.GenerateErrorReport());

                actionStatus(false, error.GenerateErrorReport());
            });    

        #endif
        }

        // UnLink Account with MobileIDs
        public static void UnLinkWithMobileID()
        {
            #if UNITY_ANDROID // On ANDROID

            //Link user account with ANDROID ID
            PlayFabClientAPI.UnlinkAndroidDeviceID(new UnlinkAndroidDeviceIDRequest()
            {
                AndroidDeviceId = ReturnMobileID()

            }, (result) =>

            {
                Debug.Log("Account UnLinked With Android DeviceID Succeed.");
            },

            (error)=>
            {
                Debug.LogError("Unlink with MobileDeviceID: " + error.GenerateErrorReport());

            }); // Error Callback

            #endif

            #if UNITY_IOS // On IOS

            //Link user account with IOS ID
            PlayFabClientAPI.UnlinkIOSDeviceID(new UnlinkIOSDeviceIDRequest()
            {
                DeviceId = ReturnMobileID()
            }, 
            
            (result) =>
            {
                Debug.Log("Account UnLinked With IOS DeviceID Succeed.");
            },

            (error) =>
            {
                Debug.LogError("Unlink with IOS DeviceID: " + error.GenerateErrorReport());

            }); // Error Callback

            #endif
        }

        #endregion

        #region DISPLAY - NAME

        public static void UpdateGuestUserDisplayName(string userName, Action actionSuccess, Action actionError)
        {
            //Display Name Request
            var requestDisplayName = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };

            PlayFabClientAPI.UpdateUserTitleDisplayName(
                
                requestDisplayName,

                (result) =>
                {
                    Debug.Log("Display Name Changed: " + result.DisplayName);

                    PlayerPrefs.SetString("DISPLAYNAME_GUEST", result.DisplayName);

                    actionSuccess();

                    UserDisplayName = result.DisplayName;
                },

                (error) =>
                {
                    Debug.LogError("Display Name Change Error: " + error.GenerateErrorReport());

                    actionError();

                });
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

        //Remove Player Stored Auth. Data
        public static void RemovePlayerStoredAuthData()
        {
            PlayerPrefs.DeleteKey("EMAIL");

            PlayerPrefs.DeleteKey("PASSWORD");
        }

        public static GetPlayerCombinedInfoRequestParams LoginPayloadRequestSetter()
        {
            GetPlayerCombinedInfoRequestParams ınfoRequestParams = new GetPlayerCombinedInfoRequestParams();

            ınfoRequestParams.GetPlayerProfile = true;

            ınfoRequestParams.GetUserAccountInfo = true;

            PlayerProfileViewConstraints viewConstraints = new PlayerProfileViewConstraints();

            viewConstraints.ShowDisplayName = true;

            ınfoRequestParams.ProfileConstraints = viewConstraints;

            return ınfoRequestParams;
        }

    }

}


