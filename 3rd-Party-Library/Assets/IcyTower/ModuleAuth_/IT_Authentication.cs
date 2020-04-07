using System.Collections;
using System.Collections.Generic;
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
        if (_gpgsAuth.LoggedIn())
        {
            /// PlayFabGPGS handles automatically Login with GPGS
        }

        // Not LoggedIn before with GPGS
        else
        {   
            /// Login as Guest with Unique DeviceID
            _customAuth.AnonymousLogin(false);
        }

    }

    public void LinkFacebook()
    {

    }

    public void UnLinkFacebook()
    {

    }

    public void LinkGooglePlay()
    {

    }

    public void UnLinkGooglePlay()
    {

    }
}
