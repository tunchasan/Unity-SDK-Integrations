using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManagerScript : MonoBehaviour
{
    private string APP_ID = "ca-app-pub-1508527570491486~9884488791";

    private BannerView bannerAD;

    private InterstitialAd ınterstitialAD;

    private RewardBasedVideoAd rewardVideoAD;
    private void Start()
    {
        // Initialize the Google Mobile Ads SDK. When publish our app, we should use it.
        //MobileAds.Initialize(appId);
    }

    public void RequestBanner()
    {
        #if UNITY_ANDROID // This BannerID is for Testing.
                string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
               string BANNER_ID = "ca-app-pub-3940256099942544/2934735716";
        #else
               string BANNER_ID = "unexpected_platform";
        #endif

        bannerAD = new BannerView(BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TEST APP
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        bannerAD.LoadAd(adRequest);

    }

    public void DisplayBanner()
    {
        bannerAD.Show();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //AD is Loaded, Can Be Shown.
        DisplayBanner();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //AD failed to Load, Load it Again.
        RequestBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        
    }

    public void HandleBannerADEvents(bool Active)
    {
        if(Active) {

            // Called when an ad request has successfully loaded.
            this.bannerAD.OnAdLoaded += this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAD.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAD.OnAdOpening += this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAD.OnAdClosed += this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAD.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        } else {

            // Called when an ad request has successfully loaded.
            this.bannerAD.OnAdLoaded -= this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAD.OnAdFailedToLoad -= this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAD.OnAdOpening -= this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAD.OnAdClosed -= this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAD.OnAdLeavingApplication -= this.HandleOnAdLeavingApplication;
        }
       
    }

    public void OnEnable()
    {
        HandleBannerADEvents(true);
    }

    public void OnDisable()
    {
        HandleBannerADEvents(false);
    }
}
