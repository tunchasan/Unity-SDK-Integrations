using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Library.Advertisement.Admob
{
    public class RewardedADs : AdmobADs
    {
        private RewardedAd _RewardedAD;

        private string _Rewarded_ID;

        [Header("Rewarded Video AD Properties")]
        public string _AndroidRewardedVideoAdID;

        public string _IOSRewardedVideoAdID;

        //Request RewardedAd depends on platforms and Load the AD.
        public void RequestRewardedVideoAD()
        {
            //Detect Platform
            PlatformADHandler();

            //Create & Assign New Instance of RewardedAD
            _RewardedAD = new RewardedAd(_Rewarded_ID);

            HandleRewardedVideoADEvents(true);

            //FOR REAL APP
            //AdRequest adRequest = new AdRequest.Builder().Build();

            //FOR TEST APP
            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

            //Load RewardedVideoAD
            _RewardedAD.LoadAd(adRequest);
        }

        //Handle Rewarded_ID depends on Platform.
        public void PlatformADHandler()
        {
             #if UNITY_ANDROID //ANDROID
            _Rewarded_ID = _AndroidRewardedVideoAdID;   // This RewardedID is for Testing.

             #elif UNITY_IPHONE // IOS
                _Rewarded_ID = _IOSInterstitialAdID;   // This RewardedID is for Testing.

             #else //Other
                _Rewarded_ID = "unexpected_platform";

             #endif
        }

        //Show the RewardedAD
        private void DisplayRewardedVideoAD()
        {
            _RewardedAD.Show();
        }

        // Called when an ad request has successfully loaded.
        public void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            //AD is Loaded, Can Be Shown.
            DisplayRewardedVideoAD();

            Debug.Log("HandleRewardedAdLoaded event received");
        }

        // Called when an ad request failed to load.
        public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
        {
            Debug.LogError(
                "HandleRewardedAdFailedToLoad event received with message: "
                                 + args.Message);
        }

        // Called when an ad is shown.
        public void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdOpening event received");
        }

        // Called when an ad request failed to show.
        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.LogError(
                "HandleRewardedAdFailedToShow event received with message: "
                                 + args.Message);
        }

        // Called when the user should be rewarded for interacting with the ad.
        public void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdClosed event received");
        }

        // Called when the ad is closed.
        public void HandleUserEarnedReward(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;
            Debug.Log(
                "HandleRewardedAdRewarded event received for "
                            + amount.ToString() + " " + type);
        }

        //Handle all ADEvents for InterstitialAD
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

        //If the script is disabled, we deactivete the ADEvents.
        private void OnDisable()
        {
            HandleRewardedVideoADEvents(false);
        }

    }

}

