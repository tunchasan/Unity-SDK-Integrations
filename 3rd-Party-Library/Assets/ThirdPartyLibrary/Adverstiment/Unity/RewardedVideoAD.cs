using System.Collections;
using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using UnityEngine.Advertisements;

    public class RewardedVideoAD : UnityADs
    {
        [Header("Rewarded Video AD Properties")]
        public string RewardedVideoAdID;

        // Implement a function for showing a rewarded video ad:
        public void ShowRewardedVideo()
        {
            //Update "activeAD" Value
            activeAD = RewardedVideoAdID;

            //Start "ShowRewardedAdReady" Coroutine
            StartCoroutine(ShowRewardedAdReady());
        }

        // Implement a coroutine that controls the Advertisement is ready or not 
        IEnumerator ShowRewardedAdReady()
        {
            //Start the while loop for 
            while (!Advertisement.IsReady(RewardedVideoAdID))
            {
                yield return new WaitForSeconds(_AdControlRate);
            }

            //Display the Ad.
            Advertisement.Show(RewardedVideoAdID);

            Debug.Log("HandleRewardedAdDisplayed event received");

            //Stop "ShowRewardedAdReady" Coroutine
            StopCoroutine(ShowRewardedAdReady());
        }

        // Called when an ad request has successfully loaded.
        public override void OnUnityAdsReady(string placementId)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (activeAD == RewardedVideoAdID)
            {
                if (placementId == RewardedVideoAdID)
                {
                    Debug.Log("HandleRewardedAdLoaded event received");
                }
            }
        }

        // Called when an ad has Finished, Skipped or Failed.
        public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (activeAD == RewardedVideoAdID)
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
        }

        // Called when an ad request be concluded by an error.
        public override void OnUnityAdsDidError(string message)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (activeAD == RewardedVideoAdID)
            {
                // Log the error.
                Debug.LogError("HandleRewardedAdError event received");
            }
        }

        // Called when the end-users triggers an ad.
        public override void OnUnityAdsDidStart(string placementId)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (activeAD == RewardedVideoAdID)
            {
                // Optional actions to take when the end-users triggers an ad.
                Debug.Log("HandleRewardedAdOpening event received");
            }
        }
    }
}
