using System.Collections;
using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using System;
    using UnityEngine.Advertisements;

    /// <summary>
    /// The class provides the full range of Unity rewarded video ads. features.
    /// </summary>
    public class UnityRewardedVidedAd : UnityAD
    {
        /// <summary>
        /// These are the ad callback events that can be hooked into.
        /// </summary>
        public Action<string, int> OnUserEarnedReward;

        /// <summary>
        /// Defines, reward type.
        /// </summary>
        public string RewardType { get; private set; }

        /// <summary>
        /// Defines rewarded item amount.
        /// </summary>
        public int RewardAmount { get; private set; }

        /**********************************************************************************/

        /// <summary>
        /// Defines unique rewarded advertisement id.
        /// Rewarded video advertisement's requests are managed on this id.
        /// </summary>
        public const string _rewardedVideoAdID = "rewardedVideo";    /* <-----------------*/

        /**********************************************************************************/

        public UnityRewardedVidedAd(string rewardType, int rewardAmount)
        {
            RewardType = rewardType;

            RewardAmount = rewardAmount;
        }

        /// <summary>
        /// Creates a request that handles displaying and loading rewarded video ads.
        /// Then sends the request to Unity advertisement server.
        /// We catch upcoming information by the request's callbacks.
        /// </summary>
        public void LoadAndShowRewardedVideoAd()
        {
            // Update "activeAD" Value
            _activeAD = _rewardedVideoAdID;

            // Display the Ad.
            Advertisement.Show(_rewardedVideoAdID);

            Debug.Log("HandleRewardedAdDisplayed event received");
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        public override void OnUnityAdsReady(string placementId)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (_activeAD == _rewardedVideoAdID)
            {
                if (placementId == _rewardedVideoAdID)
                {
                    Debug.Log("HandleRewardedAdLoaded event received");

                    OnAdLoaded?.Invoke();
                }
            }
        }

        /// <summary>
        /// Called when an ad has Finished, Skipped or Failed.
        /// </summary>
        public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (_activeAD == _rewardedVideoAdID)
            {
                // Define conditional logic for each ad completion status:
                if (showResult == ShowResult.Finished)
                {
                    // Reward the user for watching the ad to completion.
                    Debug.Log(
                        "HandleRewardedAdRewarded event received for 10 Golds");

                    OnUserEarnedReward?.Invoke(RewardType, RewardAmount);
                }
                else if (showResult == ShowResult.Skipped)
                {
                    // Do not reward the user for skipping the ad.
                    Debug.Log("HandleRewardedAdClosed event received");

                    OnAdClosed?.Invoke();
                }
                else if (showResult == ShowResult.Failed)
                {
                    Debug.LogError("HandleRewardedAdError event received");

                    OnAdFailedToShow?.Invoke();
                }

                _activeAD = null; // Fix multiple reward issue
            }
        }

        /// <summary>
        /// Called when an ad request be concluded by an error.
        /// </summary>
        public override void OnUnityAdsDidError(string message)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (_activeAD == _rewardedVideoAdID)
            {
                // Log the error.
                Debug.LogError("HandleRewardedAdError event received");

                OnAdFailedToLoad?.Invoke();
            }
        }

        /// <summary>
        /// Called when the end-users triggers an ad.
        /// </summary>     
        public override void OnUnityAdsDidStart(string placementId)
        {
            // Control the activeAD value with RewardedVideoAdID
            if (_activeAD == _rewardedVideoAdID)
            {
                // Optional actions to take when the end-users triggers an ad.
                Debug.Log("HandleRewardedAdOpening event received");

                OnAdOpened?.Invoke();
            }
        }
    }
}
