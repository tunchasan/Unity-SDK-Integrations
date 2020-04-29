using UnityEngine;
using UnityEngine.UI;
using Library.FaceBook;
using Library.GooglePlay;
using Library.Authentication;

public class AuthenticationExample : MonoBehaviour
{
    private PlayfabCustomAuth customAuth;

    private FacebookService facebookAuth;

    private GooglePlayGameService gpgsAuth;

    private void Awake()
    {
        customAuth = new PlayfabCustomAuth();

        facebookAuth = new FacebookService();

        gpgsAuth = new GooglePlayGameService();

        ConnectGooglePlayButton.onClick.AddListener(ConnectGooglePlay);

        ConnectFacebookButton.onClick.AddListener(ConnectFacebook);
    }

    private void Update()
    {
        if (Input.GetKey("w"))
            Debug.Log(PlayfabCustomAuth.ISGuestAccount());
    }

    /**************************************************************************************************/
    // AUTHENTICATION

    private void Start()
    {
        // LoggedIn before with GPGS
        if (gpgsAuth.LoggedInBefore())
        {
            /// GooglePlayGameService handles automatically Login with GPGS
            Debug.Log("GPGS login in automatically...");

            // Link GPGS Acc.
            gpgsAuth.LoginPlayGameService(

                linkAction: false,

                (actionResult, actionMessage, actionRecover) =>
                {
                    if (actionResult) // Request completed with no error ( Succeed )
                    {
                        
                    }

                    else // Request completed failure
                    {
                       
                    }

                });

        }

        // Not LoggedIn before with GPGS
        else
        {
            Debug.Log("Mobile Device login automatically...");

            /// Login as Guest with Unique DeviceID
            customAuth.AnonymousLogin(

                linkAction: false,

                (actionResult, actionMessage) =>
                {
                    if (actionResult) // Request completed with no error ( Succeed )
                    {

                    }

                    else // Request completed failure
                    {

                    }

                });

        }
    }

    /**************************************************************************************************/
    // CONNECT SERVICES

    [Header("Connect Service Configuration")]
    public Button ConnectGooglePlayButton;

    public Button ConnectFacebookButton;

    public void ConnectGooglePlay() // Connect Google Play
    {
        ConnectGooglePlayButton.interactable = false; // Disable Button

        if (GooglePlayGameService.LoggedIn) // Loggedin with GPGS
        {
            // UnLink GPGS Request
            gpgsAuth.UnLinkWithGooglePlayAccount(

                (actionResult,actionMessage) =>
                {
                    if (actionResult) // Request completed success
                    {
                        Debug.Log(actionMessage);
                    }

                    else // Request completed failure
                    {
                        Debug.LogError(actionMessage);
                    }

                    ConnectGooglePlayButton.interactable = true; // Enable Button

                });

        }

        else // Not Loggedin with GPGS, Connect with it.
        {
            // Link GPGS Acc.
            gpgsAuth.LoginPlayGameService(
                
                linkAction: true,

                (actionResult, actionMessage, actionRecover) =>
                {
                    if (actionResult) // Request completed with no error ( Succeed )
                    {
                        Debug.Log(actionMessage);
                    }

                    else // Request completed failure
                    {
                        if (actionRecover)
                        {
                            Debug.LogError(actionMessage);
                        }

                        else // Recover Action
                        {
                            StartRecoverWithGooglePlay(actionMessage);
                        }
                    }

                    ConnectGooglePlayButton.interactable = true; // Enable Button

                });

        }
    }

    public void ConnectFacebook() // Connect Facebook
    {
        ConnectFacebookButton.interactable = false;

        if (FacebookService.IsLinkedWithFacebook()) // Loggedin with Facebook
        {
            // UnLink Facebook Acc.
            facebookAuth.UnLinkWithFacebook(

                (actionResult, actionMessage) =>
                {
                    if (actionResult) // Request completed success
                    {
                        Debug.Log(actionMessage);
                    }

                    else // Request completed failure
                    {
                        Debug.LogError(actionMessage);
                    }

                    ConnectFacebookButton.interactable = true; // Enable Button

                });
        }

        else
        {   
            // Link Facebook Acc.
            facebookAuth.AuthLogin(
                
                linkAction: true,

                (actionResult, actionMessage, actionRecover) =>
                {
                    if (actionResult) // Request completed with no error ( Succeed )
                    {
                        Debug.Log(actionMessage);
                    }

                    else // Request completed failure
                    {
                        if (actionRecover)
                        {
                            Debug.LogError(actionMessage);
                        }

                        else // Recover Action
                        {
                            StartRecoverWithFacebook(actionMessage);
                        }
                    }

                    ConnectFacebookButton.interactable = true; // Enable Button

                });
        }
    }

    /**************************************************************************************************/
    // RECOVER
    //Debug.LogWarning("Warning: progress in the current game will be removed.);

    [Header("Recover PopUpMenu Configuration")]
    public GameObject RecoverPopUpMenu;

    public Text RecoverPopUpText;

    public Button RecoverPopUpYesButton;

    public Button RecoverPopUpNoButton;


    public GameObject RecoverResultPopUpMenu;

    public Text RecoverResultText;

    public Button RecoverResultPopUpOKButton;


    public void StartRecoverWithFacebook(string recoverText)
    {
        RecoverPopUpText.text = recoverText + "\n\n\n\n (Warning: progress in the current game will be removed.)";

        RecoverPopUpYesButton.onClick.AddListener(RecoverAccountWithFacebook);

        RecoverPopUpNoButton.onClick.AddListener(DontRecoverAccountWithFacebook);

        RecoverPopUpMenu.SetActive(true);
    }

    public void RecoverAccountWithFacebook() // Recover with Facebook -> YES
    {
        facebookAuth.RecoverAccount(

            (success, actionMessage) =>
            {
                if (!success) // Recover Success
                {
                    // User succesfully recover account.

                    // DO WHATEVER YOU WANNA DO HERE, AFTER RECOVERING
                }

                else // Recover Failed
                {
                    // Can be shown new pop up here
                    RecoverResultText.text = actionMessage;

                    RecoverResultPopUpMenu.SetActive(true);

                    RecoverResultPopUpOKButton.onClick.AddListener(CloseRecoverResultPopUp);
                }

            });

        RecoverPopUpYesButton.onClick.RemoveListener(RecoverAccountWithFacebook);

        RecoverPopUpNoButton.onClick.RemoveListener(DontRecoverAccountWithFacebook);

        RecoverPopUpMenu.SetActive(false);
    }

    public void DontRecoverAccountWithFacebook() // Recover with Facebook -> NO
    {
        facebookAuth.DontRecoverAccount();

        RecoverPopUpYesButton.onClick.RemoveListener(RecoverAccountWithFacebook);

        RecoverPopUpNoButton.onClick.RemoveListener(DontRecoverAccountWithFacebook);

        RecoverPopUpMenu.SetActive(false);
    }

    public void StartRecoverWithGooglePlay(string recoverText) // Start Recover
    {
        RecoverPopUpText.text = recoverText;

        RecoverPopUpYesButton.onClick.AddListener(RecoverAccountWithGPGS);

        RecoverPopUpNoButton.onClick.AddListener(DontRecoverAccountWithGPGS);

        RecoverPopUpMenu.SetActive(true);
    }

    public void RecoverAccountWithGPGS() // Recover with Google Play -> YES
    {
        gpgsAuth.RecoverAccount();

        RecoverPopUpYesButton.onClick.RemoveListener(RecoverAccountWithGPGS);

        RecoverPopUpNoButton.onClick.RemoveListener(DontRecoverAccountWithGPGS);

        RecoverPopUpMenu.SetActive(false);
    }

    public void DontRecoverAccountWithGPGS() // Recover with Google Play -> NO
    {
        gpgsAuth.DontRecoverAccount();

        RecoverPopUpYesButton.onClick.RemoveListener(RecoverAccountWithGPGS);

        RecoverPopUpNoButton.onClick.RemoveListener(DontRecoverAccountWithGPGS);

        RecoverPopUpMenu.SetActive(false);
    }

    public void CloseRecoverResultPopUp() // Close Recover Result Popup Menu
    {
        RecoverResultPopUpMenu.SetActive(false);

        RecoverResultPopUpOKButton.onClick.RemoveListener(CloseRecoverResultPopUp);
    }

    /**************************************************************************************************/

}
