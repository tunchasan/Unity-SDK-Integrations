using UnityEngine;
using UnityEngine.Advertisements;

public class VideoADs : UnityADs
{
    [Header("Video AD Properties")]
    public string VideoAdID;

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
    public override void OnUnityAdsReady(string placementId)
    {
        if (placementId == VideoAdID) {

            //AD is Loaded, Can Be Shown.
            //ShowVideoAD();

            Debug.Log("HandleVideoAdLoaded event received");
        }
    }

    // Called when an ad has Finished, Skipped or Failed.
    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
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

    // Called when an ad request be concluded by an error.
    public override void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.LogError("HandleVideoAdError event received");
    }

    // Called when the end-users triggers an ad.
    public override void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        Debug.Log("HandleVideoAdOpening event received");
    }
}
