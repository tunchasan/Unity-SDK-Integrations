using UnityEngine;

public class IT_Authentication : MonoBehaviour
{
    private PlayfabCustomAuth _customAuth;

    private PlayfabFacebook _facebookAuth;

    private PlayFabGPGS _gpgsAuth;
     
    private void Awake()
    {
        _customAuth = new PlayfabCustomAuth();

        _facebookAuth = new PlayfabFacebook();

        _gpgsAuth = new PlayFabGPGS();
    }

    private void Start()
    {
        // LoggedIn before with GPGS
        if (_gpgsAuth.LoggedInBefore())
        {
            /// PlayFabGPGS handles automatically Login with GPGS
            Debug.Log("GPGS login in automatically...");
        }

        // Not LoggedIn before with GPGS
        else
        {   
            /// Login as Guest with Unique DeviceID
            _customAuth.AnonymousLogin(false);

            Debug.Log("Mobile Device login in automatically...");
        }

    }

    public void ConnectGPGS()
    {
        if (_gpgsAuth.GetLoggedIn()) // Loggedin with GPGS
        {
            // UnLink GPGS Acc.
            _gpgsAuth.UnLinkWithGooglePlayAccount();   
        }

        else // Not Loggedin with GPGS, Connect with it.
        {
            // Link GPGS Acc.
            _gpgsAuth.LoginPlayGameService(true);
        }
    }

    public void ConnectFacebook()
    {
        if (_facebookAuth.GetLoggedIn())
        {

        }
    }

    public void gg()
    {
        _facebookAuth.AuthLogin(true);
    }

}
