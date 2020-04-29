using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Library.Advertisement.Admob
{
    /// <summary>
    /// The class provides the full range of Admob banner advertisements available to the library.
    /// Also can be managed banner's deep properties such as type and position.
    /// </summary>
    public class AdmobBannerAd : AdmobADs
    {
        /// <summary>
        /// This is the ad callback events that can be hooked into.
        /// </summary>
        public Action OnAdLeavingApplication;

        /// <summary>
        /// Core object of the class, all requests is doing with this object.
        /// </summary>
        private BannerView _bannerAD;

        /// <summary>
        /// Defines unique banner advertisement id.
        /// Banner advertisement's requests are managed on this id.
        /// </summary>
        private string _Banner_ID;

        /// <summary>
        /// Defines banner advertisements ingame types such as 'Banner_320x50', 'MediumRectangle_300x250','SmartBanner'...
        /// </summary>
        public enum AdType { Banner_320x50, MediumRectangle_300x250, IABBanner_468x60, Leaderboard_728x90, SmartBanner };

        private AdType _BannerType;

        /*******************************************************************************************************************/

        /// <summary>
        /// Admob advertisements request only can be concluded as success with right identifier. The identifiers differ on 
        /// ios and android. For Android , '_AndroidBannerID'. Banner advertisement request are managed on this id.
        /// </summary>
        private const string _AndroidBannerID = "ca-app-pub-3940256099942544/6300978111";    /* <--------------------------*/

        /// <summary>
        /// For Ios , '_IOSBannerID'. Banner advertisement request are managed on this id.
        /// </summary>
        private const string _IOSBannerID = "ca-app-pub-3940256099942544/2934735716";    /* <------------------------------*/

        /*******************************************************************************************************************/

        /// <summary>
        /// Prepares service for first success advertisement request.
        /// </summary>
        public AdmobBannerAd()
        {
            //Detect Platform
            PlatformADHandler();
        }

        /// <summary>
        /// Creates a request that handles displaying and loading banner ads. Then sends the request to Admob advertisement server.
        /// We catch upcoming information by the request's callbacks.
        /// Also the method helps us to manage banner advertisement's properties. ( Banner Position )
        /// </summary>
        public void LoadBannerAd()
        {
            //Create & Assign New Instance of BannerAD
            _bannerAD = new BannerView(_Banner_ID, AdSize.SmartBanner, AdPosition.Bottom);

            // Subscribe events
            HandleBannerADEvents(true);

            //FOR TEST APP
            AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

            //FOR REAL APP
            //AdRequest adRequest = new AdRequest.Builder().Build();

            //Load BannerAD
            _bannerAD.LoadAd(adRequest);
        }

        /// <summary>
        /// Modifies current banner advertisement position.
        /// </summary>
        public void SetBannerPosition(AdPosition position)
        {
            _bannerAD.SetPosition(position);
        }

        /// <summary>
        /// Detects current banner's choosen type and returns a banner object.
        /// </summary>
        private AdSize AdTypeHandler()
        {
            switch (_BannerType)
            {
                case AdType.Banner_320x50: //320x50
                    {
                        return AdSize.Banner;
                    }

                case AdType.IABBanner_468x60: //468x60
                    {
                        return AdSize.IABBanner;
                    }

                case AdType.Leaderboard_728x90: //728x90
                    {
                        return AdSize.Leaderboard;
                    }

                case AdType.MediumRectangle_300x250:
                    {
                        return AdSize.MediumRectangle; //300x250
                    }

                case AdType.SmartBanner:
                    {
                        //Smart Banners detect the width of the device in its current orientation and create the ad view that size.
                        return AdSize.SmartBanner;
                    }

                default:
                    {
                        //Smart Banners detect the width of the device in its current orientation and create the ad view that size.
                        return AdSize.SmartBanner;
                    }

            }

        }

        /// <summary>
        /// Assings unique banner advertisement id to _BANNER_ID. The id is used for initializing banner ads.
        /// </summary>
        private void PlatformADHandler()
        {
            #if UNITY_ANDROID //ANDROID
                _Banner_ID = _AndroidBannerID;   // This BannerID is for Testing.

            #elif UNITY_IPHONE // IOS
                _BANNER_ID = _IOSBannerID;   // This BannerID is for Testing.

            #else //Other
                _BANNER_ID = "unexpected_platform";

            #endif
        }

        /// <summary>
        /// Display current banner advertisement
        /// </summary>
        public void ShowBanner()
        {
            _bannerAD.Show();
        }

        /// <summary>
        /// Destroy current banner advertisement
        /// </summary>
        public void DestroyBannerAd()
        {
            _bannerAD.Destroy(); // Destroy Banner object
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
        private void HandleBannerADEvents(bool Active)
        {
            if (Active)
            {

                // Called when an ad request has successfully loaded.
                this._bannerAD.OnAdLoaded += this.HandleOnAdLoaded;

                // Called when an ad request failed to load.
                this._bannerAD.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                // Called when an ad is clicked.
                this._bannerAD.OnAdOpening += this.HandleOnAdOpened;

                // Called when the user returned from the app after an ad click.
                this._bannerAD.OnAdClosed += this.HandleOnAdClosed;

                // Called when the ad click caused the user to leave the application.
                this._bannerAD.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

            }
            else
            {

                // Called when an ad request has successfully loaded.
                this._bannerAD.OnAdLoaded -= this.HandleOnAdLoaded;

                // Called when an ad request failed to load.
                this._bannerAD.OnAdFailedToLoad -= this.HandleOnAdFailedToLoad;

                // Called when an ad is clicked.
                this._bannerAD.OnAdOpening -= this.HandleOnAdOpened;

                // Called when the user returned from the app after an ad click.
                this._bannerAD.OnAdClosed -= this.HandleOnAdClosed;

                // Called when the ad click caused the user to leave the application.
                this._bannerAD.OnAdLeavingApplication -= this.HandleOnAdLeavingApplication;
            }

        }

    }

}

