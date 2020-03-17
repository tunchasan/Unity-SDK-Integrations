using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobADs : MonoBehaviour
{
    [Header("Google Admob Service Configuration")]
    public string _AndroidAppID;

    public string _IOSAppID;

    private void Awake()
    {
        PlatformHandler();
    }

    //Handle APPID depends on Platform.
    public virtual void PlatformHandler()
    {
        #if UNITY_ANDROID
                 string appId = _AndroidAppID;
        #elif UNITY_IPHONE
                 string appId = _IOSAppID;
        #else
                 string appId = "unexpected_platform";
        #endif
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK. When publish our app, we should use it.
        //MobileAds.Initialize(appId);
    }

}
