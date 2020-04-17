using UnityEngine;

namespace Library.Advertisement.Admob
{    
     /// <summary>
     /// AdmobADs which provide the full range of Google Admob features available to the library - banner, interstitial and rewarded
     /// video ads for Android and IOS.
     /// </summary>
    
    public class AdmobADs : MonoBehaviour
    {
        private string appID;

        /// <summary>
        /// Both fields represent specific platform unique App ID. We initialize mobile ads services with this ID.
        /// </summary>
        /// <summary>

        /// <summary>
        /// Stores unique app id for Android. When we select platform as android, we initialize our ads with this id.
        /// </summary>
        [Header("Google Admob Service Configuration")]
        public string _AndroidAppID;

        /// <summary>
        /// Stores unique app id for IOS. When we select platform as ios, we initialize our ads with this id.
        /// </summary>
        public string _IOSAppID;

        /// <summary>
        ///  Initialize Services depending on platform with unique appID.
        /// </summary>
        
        private void Start()
        {
            /// 
            PlatformHandler();

            /// When we publish the app, we should initialize MobileAds with appID.
            // MobileAds.Initialize(appID);
        }

        /// <summary>
        /// PlatformHandler sets appID by depending on the platform.
        /// </summary>
        public void PlatformHandler()
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


