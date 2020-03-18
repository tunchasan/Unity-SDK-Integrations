//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Advertisements;
//using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
//public class RewardedVideoAD : UnityADs
//{
//    private Button myButton;

//    [Header("Rewarded Video AD Properties")]
//    public string RewardedVideoAdID;

//    private void Start()
//    {
//        //Detect Platform
//        PlatformHandler();

//        // Initialize the Ads service:
//        Advertisement.Initialize(appID, testMode);
//    }
//}

using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedVideoAD : UnityADs, IUnityAdsListener
{

    [Header("Rewarded Video AD Properties")]
    public string RewardedVideoAdID;
    
    private void Start()
    {
        //Detect Platform
        PlatformHandler();

        //Add Advertisement Listener
        Advertisement.AddListener(this);
    }

    //Request RewardedAd to Load it.
    public void RequestRewardedAD()
    {
        // Initialize the Ads listener and service
        Advertisement.Initialize(appID, testMode);
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo()
    {
        //Display the Ad.
        Advertisement.Show(RewardedVideoAdID);
    }

    // Called when an ad request has successfully loaded.
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == RewardedVideoAdID) {

            //AD is Loaded, Can Be Shown.
            ShowRewardedVideo();

            Debug.Log("HandleRewardedAdLoaded event received");
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log(
                "HandleRewardedAdRewarded event received for 10 Golds");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("HandleRewardedAdClosed event received");
        }
        else if (showResult == ShowResult.Failed)
        {
           Debug.LogError("HandleRewardedAdError event received");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.LogError("HandleRewardedAdError event received");
    }

    // Called when the end-users triggers an ad.
    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        Debug.Log("HandleRewardedAdOpening event received");
    }
}
