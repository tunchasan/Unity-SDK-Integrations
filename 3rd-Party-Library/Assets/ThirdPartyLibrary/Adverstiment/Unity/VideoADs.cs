using System.Collections;
using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using UnityEngine.Advertisements;

    /// <summary>
    /// The class provides the full range of Unity video ads. features.
    /// </summary>
    public class VideoADs : UnityADs
    {
        /********************************************************************************/

        /// <summary>
        /// Defines unique video advertisement id.
        /// Video advertisement's requests are managed on this id.
        /// </summary>
        public const string VideoAdID = "video";    /* <--------------------------------*/

        /********************************************************************************/

        /// <summary>
        /// Creates a request that handles displaying and loading video ads.
        /// Then sends the request to Unity advertisement server.
        /// We catch upcoming information by the request's callbacks.
        /// </summary>
        public void LoadAndShowVideoAD()
        {
            //Update "activeAD" Value
            _activeAD = VideoAdID;

            //Display the Ad.
            Advertisement.Show(VideoAdID);

            Debug.Log("HandleVideoAdDisplayed event received");
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        public override void OnUnityAdsReady(string placementId)
        {
            // Control the activeAD value with VideoAdID
            if (_activeAD == VideoAdID)
            {
                if (placementId == VideoAdID)
                {
                    Debug.Log("HandleVideoAdLoaded event received");
                }
            }
        }

        /// <summary>
        /// Called when an ad has Finished, Skipped or Failed.
        /// </summary>
        public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Control the activeAD value with VideoAdID
            if (_activeAD == VideoAdID)
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

                _activeAD = null; // Fix multiple reward issue
            }
        }

        /// <summary>
        /// Called when an ad request be concluded by an error.
        /// </summary>
        public override void OnUnityAdsDidError(string message)
        {
            // Control the activeAD value with VideoAdID
            if (_activeAD == VideoAdID)
            {
                // Log the error.
                Debug.LogError("HandleVideoAdError event received");
            }
        }

        /// <summary>
        /// Called when the end-users triggers an ad.
        /// </summary>     
        public override void OnUnityAdsDidStart(string placementId)
        {
            // Control the activeAD value with VideoAdID
            if (_activeAD == VideoAdID)
            {
                // Optional actions to take when the end-users triggers an ad.
                Debug.Log("HandleVideoAdOpening event received");
            }
        }
    }
}

