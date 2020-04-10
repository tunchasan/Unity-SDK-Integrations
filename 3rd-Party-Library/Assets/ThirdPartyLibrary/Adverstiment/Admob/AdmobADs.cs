using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library.Advertisement.Admob
{
public class AdmobADs : MonoBehaviour
{
    private string appID;

    [Header("Google Admob Service Configuration")]
    public string _AndroidAppID;

    public string _IOSAppID;

    //Initialize some stuff.
    private void Start()
    {
        PlatformHandler();

        // Initialize the Google Mobile Ads SDK. When publish our app, we should use it.
        //MobileAds.Initialize(appID);
    }

    //Handle APPID depends on Platform.
    public void PlatformHandler()
    {
        #if UNITY_ANDROID
                  appID = _AndroidAppID;
        #elif UNITY_IPHONE
                  appID = _IOSAppID;
        #else
                  appID = "unexpected_platform";
        #endif
    }

}
}


