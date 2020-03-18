using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityADs : MonoBehaviour
{
    protected string appID;

    [Header("Unity ADs Service Configuration")]
    public string _AndroidAppID;

    public string _IOSAppID;

    public bool testMode;

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
