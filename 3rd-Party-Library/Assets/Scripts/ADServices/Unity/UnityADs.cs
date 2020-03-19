using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityADs : MonoBehaviour, IUnityAdsListener
{
    protected string appID;

    [Header("Unity ADs Service Configuration")]
    public string _AndroidAppID;

    public string _IOSAppID;

    public bool testMode;

    private void Start()
    {
        //Detect Platform
        PlatformHandler();

        //Add Advertisement Listener
        Advertisement.AddListener(this);

        // Initialize the Ads listener and service
        Advertisement.Initialize(appID, testMode);
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

    // Called when an ad request has successfully loaded.
    public virtual void OnUnityAdsReady(string placementId)
    {
         Debug.Log("HandleTheAd event received to load");
    }

    // Called when an ad request be concluded by an error.
    public virtual void OnUnityAdsDidError(string message)
    {
        Debug.LogError("HandleTheAd event received to an error.");
    }

    // Called when an ad request has successfully displayed.
    public virtual void OnUnityAdsDidStart(string placementId)
    {
        Debug.LogError("HandleTheAd event received to start.");
    }

    // Called when an ad has Finished, Skipped or Failed.
    public virtual void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Defined conditional logic for each ad completion status based on showResult.
        if (showResult == ShowResult.Finished)
        {
            Debug.Log("HandleTheAd event received to finish.");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Optionally, take actions to take when the end-users triggers an ad.
            Debug.Log("HandleTheAd event received to skip.");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("HandleTheAd event received to an Error.");
        }
    }
}
