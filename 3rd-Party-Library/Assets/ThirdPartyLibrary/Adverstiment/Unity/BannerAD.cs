using System.Collections;
using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using UnityEngine.Advertisements;

    /// <summary>
    /// The class provides the full range of Unity Banner advertisements. Also gives chance to manage banner advertisement 
    /// deep properties such as position.
    /// </summary>
    public class BannerAD : UnityADs
    {
        /**********************************************************************/

        /// <summary>
        /// Defines unique video advertisement id.
        /// Banner advertisement's requests are managed on this id.
        /// </summary>
        public const string BannerAdID = "BannerAd";   /* <-------------------*/

        /**********************************************************************/

        /// <summary>
        /// Defines banner advertisements ingame position such as TOP_LEFT, TOP_CENTER, TOP_RIGHT, BOTTOM_LEFT,
        /// BOTTOM_CENTER, BOTTOM_RIGHT and CENTER.
        /// </summary>
        private BannerPosition _BannerPosition = BannerPosition.BOTTOM_CENTER;

        /// <summary>
        /// Creates a request that handles displaying and loading banner ads. Then sends the request to Unity advertisement server.
        /// We catch upcoming information by the request's callbacks.
        /// Also the method helps us to manage banner advertisement's properties. ( Banner Position )
        /// </summary>
        public void LoadAndShowBannerAd()
        {
            //Update "activeAD" Value
            _activeAD = BannerAdID;

            //Set Position the Banner
            Advertisement.Banner.SetPosition(_BannerPosition);

            //Display the Ad.
            Advertisement.Banner.Show(BannerAdID);

            Debug.Log("HandleBannerAdDisplayed event received");
        }

        /// <summary>
        /// Makes banner ads. visibility to false. The banner ads. can be shown again by using ShowBannerAD method.
        /// </summary>
        public void HideBannerAD()
        {
            Advertisement.Banner.Hide();
        }

        /// <summary>
        /// Modifies current banner advertisement position.
        /// </summary>
        public void SetBannerPosition(BannerPosition position)
        {
            Advertisement.Banner.SetPosition(position);

            _BannerPosition = position;
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        public override void OnUnityAdsReady(string placementId)
        {
            // Control the activeAD value with BannerAdID
            if (_activeAD == BannerAdID)
            {
                if (placementId == BannerAdID)
                {
                    Debug.Log("HandleBannerAdLoaded event received");
                }
            }
        }

        /// <summary>
        /// Called when an ad has Finished, Skipped or Failed.
        /// </summary>
        public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Control the activeAD value with BannerAdID
            if (_activeAD == BannerAdID)
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
        }

        /// <summary>
        /// Called when an ad request be concluded by an error.
        /// </summary>
        public override void OnUnityAdsDidError(string message)
        {
            // Control the activeAD value with BannerAdID
            if (_activeAD == BannerAdID)
            {
                // Log the error.
                Debug.LogError("HandleBannerAdError event received");
            }
        }

        /// <summary>
        /// Called when the end-users triggers an ad.
        /// </summary>  
        public override void OnUnityAdsDidStart(string placementId)
        {
            // Control the activeAD value with BannerAdID
            if (_activeAD == BannerAdID)
            {
                // Optional actions to take when the end-users triggers an ad.
                Debug.Log("HandleBannerOpening event received");
            }
        }
    }
}

