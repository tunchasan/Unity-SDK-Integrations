using UnityEngine;

public class IT_Authentication : MonoBehaviour
{
    private PlayfabCustomAuth _customAuth;

    private PlayfabFacebook _facebookAuth;

    private PlayFabGPGS _gpgsAuth;

    private string _nickName;
     
    private void Awake()
    {
        _customAuth = new PlayfabCustomAuth();

        _facebookAuth = new PlayfabFacebook();

        _gpgsAuth = new PlayFabGPGS();

        _nickName = "";
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

    public void LinkWithFacebook()
    {
        _facebookAuth.AuthLogin(true);
    }

    public void UnLinkWithFacebook()
    {
        _facebookAuth.UnLinkWithFacebook();
    }

    public void LinkWithGooglePlay()
    {
        _gpgsAuth.LoginPlayGameService(true);
    }

    public void UnLinkWithGooglePlay()
    {
        _gpgsAuth.UnLinkWithGooglePlayAccount();
    }

}
