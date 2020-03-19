using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAD : UnityADs
{
    [Header("Banner AD Properties")]
    public string BannerAdID;

    public BannerPosition _BannerPosition;

    // Implement a function for showing a Banner ad:
    public void ShowBannerAD()
    {
        if (Advertisement.IsReady())
        {
            //Set Position the Banner
            Advertisement.Banner.SetPosition(_BannerPosition);

            //Display the Ad.
            Advertisement.Banner.Show(BannerAdID);

            Debug.Log("HandleBannerAdDisplayed event received");
        }

    }

    // Called when an ad request has successfully loaded.
    public override void OnUnityAdsReady(string placementId)
    {
        if (placementId == BannerAdID)
        {
            Debug.Log("HandleBannerAdLoaded event received");
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
                "HandleBannerAd event received to finish.");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("HandleBannerAdSkipped event received");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogError("HandleBannerAdError event received");
        }
    }

    // Called when an ad request be concluded by an error.
    public override void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.LogError("HandleBannerAdError event received");
    }

    // Called when the end-users triggers an ad.
    public override void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        Debug.Log("HandleBannerOpening event received");
    }
}
