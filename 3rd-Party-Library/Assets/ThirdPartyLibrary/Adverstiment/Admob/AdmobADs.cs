using System;
using UnityEngine;

namespace Library.Advertisement.Admob
{    
     /// <summary>
     /// AdmobADs which provide the full range of Google Admob features available to the library - banner, interstitial and rewarded
     /// video ads for Android and IOS.
     /// </summary>
    
    public abstract class AdmobADs
    {
        /// <summary>
        /// These are the ad callback events that can be hooked into.
        /// </summary>
        public Action OnAdLoaded;
        
        public Action OnAdOpened;
        
        public Action OnAdClosed;

        public Action OnAdFailedToLoad;

        /// <summary>
        /// Stores application id based on platform.
        /// </summary>
        private string appID;

        /// <summary>
        /// Both fields represent specific platform unique App ID. We initialize mobile ads services with this ID.
        /// </summary>
        /// <summary>

        /*********************************************************************************************************/

        /// <summary>
        /// Stores unique app id for Android. When we select platform as android, we initialize our ads with this id.
        /// </summary>
        private const string _AndroidAppID = "ca-app-pub-1508527570491486~9884488791";    /* <------------------ */

        /// <summary>
        /// Stores unique app id for IOS. When we select platform as ios, we initialize our ads with this id.
        /// </summary>
        private const string _IOSAppID = "";     /* <----------------------------------------------------------- */

        /*********************************************************************************************************/

        /// <summary>
        ///  Initialize required admob core services.
        /// </summary>
        public AdmobADs()
        {
            /// PlatformHandler sets appID by depending on the platform.
            PlatformHandler();

            /// When we publish the app, we should initialize MobileAds with appID.
            // MobileAds.Initialize(appID);
        }

        /// <summary>
        /// Assings unique application id to appID. The id is used for initializing mobile ads services.
        /// </summary>
        private void PlatformHandler()
        {
            #if UNITY_ANDROID  // For Android
                      appID = _AndroidAppID;
            #elif UNITY_IPHONE // For IOS
                      appID = _IOSAppID;
            #else              // For Other Platform
                      appID = "unexpected_platform";
            #endif
        }

    }

}


