using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Library.Advertisement.Admob
{
    /// <summary>
    /// The class provides the full range of Admob interstitial advertisements available to the library.
    /// </summary>
    public class AdmobInterstitialAd : AdmobADs
    {
        /// <summary>
        /// This is the ad callback events that can be hooked into.
        /// </summary>
        public Action OnAdLeavingApplication;

        /// <summary>
        /// Core object of the class, all requests is doing with this object.
        /// </summary>
        private InterstitialAd _InterstitialAD = null;

        /// <summary>
        /// Defines unique interstitial advertisement id.
        /// Interstitial advertisement's requests are managed on this id.
        /// </summary>
        private string _Interstitial_ID;

        /*************************************************************************************************************************/

        /// <summary>
        /// Admob advertisements request only can be concluded as success with right identifier. The identifiers differ on ios and 
        /// android. Interstitial advertisement request are managed on this identifiers.
        /// </summary>
        private const string _AndroidInterstitialAdID = "ca-app-pub-3940256099942544/1033173712";     /* <---------------------- */

        /// <summary>
        /// Interstitial advertisement request are managed on this id.
        /// </summary>
        private const string _IOSInterstitialAdID = "ca-app-pub-3940256099942544/4411468910";    /* <--------------------------- */

        /*************************************************************************************************************************/

        /// <summary>
        /// Prepares service for first success advertisement request.
        /// </summary>
        public AdmobInterstitialAd()
        {
            //Detect Platform
            PlatformADHandler();
        }

        /// <summary>
        /// Creates a request that handles loading interstitial ads. Then sends the request to Admob advertisement server.
        /// Also manages the action events subscription status for current advertisement.
        /// </summary>
        public void LoadInterstitialAd()
        {
            //Create & Assign New Instance of InterstitialAD
            _InterstitialAD = new InterstitialAd(_Interstitial_ID);

            // Subcription
            HandleInterstitialADEvents(true);

            //FOR REAL APP
            //AdRequest adRequest = new AdRequest.Builder().Build();

            //FOR TEST APP
            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

            //Load BannerAD
            _InterstitialAD.LoadAd(adRequest);
        }

        /// <summary>
        /// Display current interstitial advertisement
        /// </summary>
        public void ShowInterstitialAd()
        {
            if (_InterstitialAD.IsLoaded()) {
                
                _InterstitialAD.Show(); 
            }

            else {

                Debug.Log("Failed: There is no loaded interstitial ad.");
            }
        }

        /// <summary>
        /// Destroy current interstitial advertisement
        /// </summary>
        public void DestroyInsterstitialAd()
        {
            _InterstitialAD.Destroy(); // Destroy Banner object
        }

        /// <summary>
        /// Returns whether rewarded video advertisement is ready to display.
        /// </summary>
        public bool IsLoaded()
        {
            return _InterstitialAD.IsLoaded();
        }

        /// <summary>
        /// Assings unique rewarded advertisement id to _Interstitial_ID. The id is used for initializing interstitial ads.
        /// </summary>
        private void PlatformADHandler()
        {
            #if UNITY_ANDROID //ANDROID
                 _Interstitial_ID = _AndroidInterstitialAdID;   // This InterstitialID is for Testing.

            #elif UNITY_IPHONE // IOS
                _Interstitial_ID = _IOSInterstitialAdID;   // This InterstitialID is for Testing.

            #else //Other
                _Interstitial_ID = "unexpected_platform";

            #endif
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        private void HandleOnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleAdLoaded event received");

            OnAdLoaded?.Invoke();
        }

        /// <summary>
        /// Called when an ad request failed to load.
        /// </summary>
        private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.LogError("HandleFailedToReceiveAd event received with message: "
                            + args.Message);

            OnAdFailedToLoad?.Invoke();
        }

        /// <summary>
        /// Called when an ad is clicked.
        /// </summary>
        private void HandleOnAdOpened(object sender, EventArgs args)
        {
            Debug.Log("HandleAdOpened event received");

            OnAdOpened?.Invoke();
        }

        /// <summary>
        /// Called when the user returned from the app after an ad click.
        /// </summary>
        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleAdClosed event received");

            OnAdClosed?.Invoke();
        }

        /// <summary>
        /// Called when the ad click caused the user to leave the application.
        /// </summary>
        private void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            Debug.Log("HandleAdLeavingApplication event received");

            OnAdLeavingApplication?.Invoke();
        }

        /// <summary>
        /// Handles all advertisement events subscription work
        /// </summary>
        private void HandleInterstitialADEvents(bool Active)
        {
            if (Active)
            {
                // Called when an ad request has successfully loaded.
                this._InterstitialAD.OnAdLoaded += HandleOnAdLoaded;

                // Called when an ad request failed to load.
                this._InterstitialAD.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                // Called when an ad is shown.
                this._InterstitialAD.OnAdOpening += HandleOnAdOpened;

                // Called when the ad is closed.
                this._InterstitialAD.OnAdClosed += HandleOnAdClosed;

                // Called when the ad click caused the user to leave the application.
                this._InterstitialAD.OnAdLeavingApplication += HandleOnAdLeavingApplication;

            }
            else
            {

                // Called when an ad request has successfully loaded.
                this._InterstitialAD.OnAdLoaded -= HandleOnAdLoaded;

                // Called when an ad request failed to load.
                this._InterstitialAD.OnAdFailedToLoad -= HandleOnAdFailedToLoad;

                // Called when an ad is shown.
                this._InterstitialAD.OnAdOpening -= HandleOnAdOpened;

                // Called when the ad is closed.
                this._InterstitialAD.OnAdClosed -= HandleOnAdClosed;

                // Called when the ad click caused the user to leave the application.
                this._InterstitialAD.OnAdLeavingApplication -= HandleOnAdLeavingApplication;

            }

        }

    }
}

