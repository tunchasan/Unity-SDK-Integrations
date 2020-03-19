using UnityEngine;
using UnityEngine.Advertisements;

public class VideoADs : UnityADs, IUnityAdsListener
{
    [Header("Video AD Properties")]
    public string VideoAdID;
    
    private void Start()
    {
        //Detect Platform
        PlatformHandler();

        //Add Advertisement Listener
        Advertisement.AddListener(this);

        // Initialize the Ads listener and service
        Advertisement.Initialize(appID, testMode);
    }

    // Implement a function for showing a video ad:
    public void ShowVideoAD()
    {
        if (Advertisement.IsReady())
        {
            //Display the Ad.
            Advertisement.Show(VideoAdID);

            Debug.Log("HandleVideoAdDisplayed event received");
        }
        
    }

    // Called when an ad request has successfully loaded.
    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == VideoAdID) {

            //AD is Loaded, Can Be Shown.
            //ShowVideoAD();

            Debug.Log("HandleVideoAdLoaded event received");
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Optionally Reward the user for watching the ad to completion.
            Debug.Log(
                "HandleVideoAd event received to finish.");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("HandleVideoAdSkipped event received");
        }
        else if (showResult == ShowResult.Failed)
        {
           Debug.LogError("HandleVideoAdError event received");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.LogError("HandleVideoAdError event received");
    }

    // Called when the end-users triggers an ad.
    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        Debug.Log("HandleVideoAdOpening event received");
    }
}
