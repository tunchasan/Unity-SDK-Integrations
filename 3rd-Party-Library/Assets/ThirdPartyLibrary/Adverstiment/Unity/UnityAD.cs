using UnityEngine;

namespace Library.Advertisement.UnityAd
{
    using System;
    using UnityEngine.Advertisements;

    /// <summary>
    /// UnityADs which provide the full range of Unity ads features available to the library - banner, video and rewarded
    /// video ads for Android and IOS.
    /// </summary>
    public abstract class UnityAD : IUnityAdsListener
    {
        /// <summary>
        /// These are the ad callback events that can be hooked into.
        /// </summary>
        public Action OnAdLoaded;

        public Action OnAdOpened;

        public Action OnAdClosed;

        public Action OnAdFailedToLoad;

        public Action OnAdFailedToShow;

        /// <summary>
        /// Stores application id based on platform.
        /// </summary>
        protected string _appID;

        /// <summary>
        /// Stores current active unity adverstiment type.
        /// </summary>
        protected static string _activeAD;

        /****************************************************************************************************************/

        /// <summary>
        /// Both fields represent specific platform unique App ID. We initialize mobile ads services with this ID.
        /// </summary>

        /// <summary>
        /// Defines unique app id for Android. When we select platform as android, we initialize our ads with this id.
        /// </summary>
        private const string _AndroidAppID = "3513340";    /* <---------------------------------------------------------*/

        /// <summary>
        /// Defines unique app id for IOS. When we select platform as ios, we initialize our ads with this id.
        /// </summary>
        private const string _IOSAppID = "3513341";    /* <------------------------------------------------------------ */

        /// <summary>
        /// Defines whether the application's advertisement work on test mode or real mode.
        /// </summary>
        private const bool _testMode = true;    /* <--------------------------------------------------------------------*/

        /****************************************************************************************************************/

        /// <summary>
        ///  Initialize Services depending on platform with unique appID. 
        ///  After initialization, Unity Ads will begin to cache the first ad. 
        ///  Unity Ads will cache the next available ad after each consecutive ad shown. 
        /// </summary>
        public UnityAD()
        {
            //Detect Platform
            PlatformHandler();

            //AdListener to Script for tracking events.
            Advertisement.AddListener(this);

            // Initialize the Ads listener and service
            Advertisement.Initialize(_appID, _testMode);
        }

        /// <summary>
        /// Assings unique application id to appID. The id is used for initializing mobile ads services.
        /// </summary>>
        private void PlatformHandler()
        {
            #if UNITY_ANDROID
                  _appID = _AndroidAppID;
            #elif UNITY_IPHONE
                  appID = _IOSAppID;
            #else
                  appID = "unexpected_platform";
            #endif
        }

        /// <summary>
        /// Controls and returns whether current advertisement is ready
        /// </summary>
        public virtual bool IsReady()
        {
            return Advertisement.IsReady();
        }

        /// <summary>
        /// Called when an ad request has successfully loaded.
        /// </summary>
        public virtual void OnUnityAdsReady(string placementId)
        {
            Debug.Log("HandleTheAd event received to load");
        }

        /// <summary>
        /// Called when an ad request be concluded by an error.
        /// </summary>
        public virtual void OnUnityAdsDidError(string message)
        {
            Debug.LogError("HandleTheAd event received to an error.");
        }

        /// <summary>
        /// Called when an ad request has successfully displayed.
        /// </summary>
        public virtual void OnUnityAdsDidStart(string placementId)
        {
            Debug.LogError("HandleTheAd event received to start.");
        }

        /// <summary>
        /// Called when an ad has Finished, Skipped or Failed.
        /// </summary>
        public virtual void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Defined conditional logic for each ad completion status based on showResult.
            if (showResult == ShowResult.Finished)
            {
                Debug.Log("HandleTheAd event received to finish.");
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Optionally, take actions to take when the end-users triggers an ad.
                Debug.Log("HandleTheAd event received to skip.");
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("HandleTheAd event received to an Error.");
            }
        }
    }

}

