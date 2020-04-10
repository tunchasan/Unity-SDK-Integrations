using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
/*                                                                                                      CAN NOT BE USED
public class InterstitialAD : UnityADs
{
    [Header("Interstitial AD Properties")]
    public string InterstitialAdID;

    // Implement a function for showing a Interstitial ad:
    public void ShowInterstitialAD()
    {
        //Start "ShowInterstitialAD" Coroutine
        StartCoroutine(ShowInterstitialAdReady());
    }

    // Implement an coroutine that controls the Advertisement is ready or not
    IEnumerator ShowInterstitialAdReady()
    {
        //Start the while loop for 
        while (!Advertisement.IsReady(InterstitialAdID))
        {
            yield return new WaitForSeconds(_AdControlRate);
        }

        //Display the Ad.
        Advertisement.Show(InterstitialAdID);

        Debug.Log("HandleInterstitialAdDisplayed event received");

        //Stop "ShowInterstitialAD" Coroutine
        StopCoroutine(ShowInterstitialAdReady());
    }

    // Called when an ad request has successfully loaded.
    public override void OnUnityAdsReady(string placementId)
    {
        if (placementId == InterstitialAdID)
        {
            Debug.Log("HandleInterstitialAdLoaded event received");
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
                "HandleInterstitialAd event received to finish.");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("HandleInterstitialAdSkipped event received");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogError("HandleInterstitialAdError event received");
        }
    }

    // Called when an ad request be concluded by an error.
    public override void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.LogError("HandleInterstitialAdError event received");
    }

    // Called when the end-users triggers an ad.
    public override void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        Debug.Log("HandleInterstitialOpening event received");
    }
}
*/