using System.Collections;
using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using UnityEngine.Advertisements;

    public class VideoADs : UnityADs
    {
        [Header("Video AD Properties")]
        public string VideoAdID;

        // Implement a function for showing a video ad:
        public void ShowVideoAD()
        {
            //Update "activeAD" Value
            activeAD = VideoAdID;

            //Start "ShowVideoAdReady" Coroutine
            StartCoroutine(ShowVideoAdReady());
        }

        // Implement an coroutine that controls the Advertisement is ready or not 
        IEnumerator ShowVideoAdReady()
        {
            //Start the while loop for 
            while (!Advertisement.IsReady(VideoAdID))
            {
                yield return new WaitForSeconds(_AdControlRate);
            }

            //Display the Ad.
            Advertisement.Show(VideoAdID);

            Debug.Log("HandleVideoAdDisplayed event received");

            //Stop "ShowVideoAdReady" Coroutine
            StopCoroutine(ShowVideoAdReady());
        }

        // Called when an ad request has successfully loaded.
        public override void OnUnityAdsReady(string placementId)
        {
            // Control the activeAD value with VideoAdID
            if (activeAD == VideoAdID)
            {
                if (placementId == VideoAdID)
                {
                    Debug.Log("HandleVideoAdLoaded event received");
                }
            }
        }

        // Called when an ad has Finished, Skipped or Failed.
        public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Control the activeAD value with VideoAdID
            if (activeAD == VideoAdID)
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
        }

        // Called when an ad request be concluded by an error.
        public override void OnUnityAdsDidError(string message)
        {
            // Control the activeAD value with VideoAdID
            if (activeAD == VideoAdID)
            {
                // Log the error.
                Debug.LogError("HandleVideoAdError event received");
            }
        }

        // Called when the end-users triggers an ad.
        public override void OnUnityAdsDidStart(string placementId)
        {
            // Control the activeAD value with VideoAdID
            if (activeAD == VideoAdID)
            {
                // Optional actions to take when the end-users triggers an ad.
                Debug.Log("HandleVideoAdOpening event received");
            }
        }
    }
}

