using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Library.Advertisement.Admob
{
    /// <summary>
    /// The class provides the full range of Admob rewarded video advertisements available to the library.
    /// </summary>
    public class RewardedADs : AdmobADs
    {
        /// <summary>
        /// Core object of the class, all requests is doing with this object.
        /// </summary>
        private RewardedAd _RewardedAD = null;

        /// <summary>
        /// Defines unique rewarded video advertisement id.
        /// Rewarded video advertisement's requests are managed on this id.
        /// </summary>
        private string _Rewarded_ID;

        /*********************************************************************************************************************/

        /// <summary>
        /// Admob advertisements request only can be concluded as success with right identifier. The identifiers differ on ios 
        /// and android. Rewarded video advertisement request are managed on this identifiers.
        /// </summary>
        private const string _AndroidRewardedVideoAdID = "ca-app-pub-3940256099942544/5224354917";    /* <------------------ */

        /// <summary>
        /// Rewarded video advertisement request are managed on this id.
        /// </summary>
        private const string _IOSRewardedVideoAdID = "ca-app-pub-3940256099942544/1712485313";    /* <---------------------- */

        /*********************************************************************************************************************/

        /// <summary>
        /// Prepares service for first success advertisement request.
        /// </summary>
        public RewardedADs()
        {
            //Detect Platform
            PlatformADHandler();
        }

        /// <summary>
        /// Creates a request that handles loading rewarded ads. Then sends the request to Admob advertisement server.
        /// Also manages the action events subscription status for current advertisement.
        /// </summary>
        public void LoadRewardedVideoAd()
        {
            if(_RewardedAD != null)
            {
                // Subcription
                HandleRewardedVideoADEvents(false);
            }

            //Create & Assign New Instance of RewardedAD
            _RewardedAD = new RewardedAd(_Rewarded_ID);

            // Subcription
            HandleRewardedVideoADEvents(true);

            //FOR REAL APP
            //AdRequest adRequest = new AdRequest.Builder().Build();

            //FOR TEST APP
            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

            //Load RewardedVideoAD
            _RewardedAD.LoadAd(adRequest);
        }

        /// <summary>
        /// Display current rewarded video advertisement
        /// </summary>
        public void ShowRewardedVideoAd()
        {
            if (_RewardedAD.IsLoaded())
            {
                _RewardedAD.Show();
            }

            else
            {
                Debug.Log("Failed: There is no loaded rewarded video ad.");
            }

        }

        /// <summary>
        /// Returns whether rewarded video advertisement is ready to display.
        /// </summary>
        public bool IsLoaded()
        {
            return _RewardedAD.IsLoaded();
        }

        /// <summary>
        /// Assings unique rewarded advertisement id to _Rewarded_ID. The id is used for initializing rewarded ads.
        /// </summary>
        private void PlatformADHandler()
        {
             #if UNITY_ANDROID //ANDROID
                _Rewarded_ID = _AndroidRewardedVideoAdID;   // This RewardedID is for Testing.

             #elif UNITY_IPHONE // IOS
                _Rewarded_ID = _IOSInterstitialAdID;   // This RewardedID is for Testing.

             #else //Other
                _Rewarded_ID = "unexpected_platform";

             #endif
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        private void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdLoaded event received");
        }

        /// <summary>
        /// Called when an ad request failed to load.
        /// </summary>
        private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
        {
            Debug.LogError(
                "HandleRewardedAdFailedToLoad event received with message: "
                                 + args.Message);
        }

        /// <summary>
        /// Called when an ad is clicked.
        /// </summary>
        private void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdOpening event received");
        }

        /// <summary>
        /// Called when an ad request failed to show.
        /// </summary>
        private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.LogError(
                "HandleRewardedAdFailedToShow event received with message: "
                                 + args.Message);
        }

        /// <summary>
        /// Called when the user should be rewarded for interacting with the ad.
        /// </summary>
        private void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdClosed event received");
        }

        /// <summary>
        /// Called when the ad is closed.
        /// </summary>
        private void HandleUserEarnedReward(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            Debug.Log(
                "HandleRewardedAdRewarded event received for "
                            + amount.ToString() + " " + type);
        }

        /// <summary>
        /// Handles all advertisement events subscription work
        /// </summary>
        private void HandleRewardedVideoADEvents(bool Active)
        {
            if (Active)
            {
                // Called when an ad request has successfully loaded.
                this._RewardedAD.OnAdLoaded += HandleRewardedAdLoaded;

                // Called when an ad request failed to load.
                this._RewardedAD.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

                // Called when an ad is shown.
                this._RewardedAD.OnAdOpening += HandleRewardedAdOpening;

                // Called when an ad request failed to show.
                this._RewardedAD.OnAdFailedToShow += HandleRewardedAdFailedToShow;

                // Called when the user should be rewarded for interacting with the ad.
                this._RewardedAD.OnUserEarnedReward += HandleUserEarnedReward;

                // Called when the ad is closed.
                this._RewardedAD.OnAdClosed += HandleRewardedAdClosed;

            }

            else
            {

                // Called when an ad request has successfully loaded.
                this._RewardedAD.OnAdLoaded -= HandleRewardedAdLoaded;

                // Called when an ad request failed to load.
                this._RewardedAD.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;

                // Called when an ad is shown.
                this._RewardedAD.OnAdOpening -= HandleRewardedAdOpening;

                // Called when an ad request failed to show.
                this._RewardedAD.OnAdFailedToShow -= HandleRewardedAdFailedToShow;

                // Called when the user should be rewarded for interacting with the ad.
                this._RewardedAD.OnUserEarnedReward -= HandleUserEarnedReward;

                // Called when the ad is closed.
                this._RewardedAD.OnAdClosed -= HandleRewardedAdClosed;

            }

        }

    }

}

