using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class BannerADs : AdmobADs
{
    private BannerView bannerAD;

    public string BANNER_ID;

    //Request Banner depends on platforms and Load the AD.
    public void RequestBanner()
    {
        //Handle which Platform is using on by device and Update BANNER_ID.
        PlatformHandler();

        //Create & Assign New Instance of BannerAD
        bannerAD = new BannerView(BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TEST APP
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        //Load BannerAD
        bannerAD.LoadAd(adRequest);
    }

    //Handle Banner_ID depends on Platform.
    private void PlatformHandler()
    {
        #if UNITY_ANDROID //ANDROID
                BANNER_ID = "ca-app-pub-3940256099942544/6300978111";   // This BannerID is for Testing.

        #elif UNITY_IPHONE // IOS
                BANNER_ID = "ca-app-pub-3940256099942544/2934735716";   // This BannerID is for Testing.

        #else //Other
                BANNER_ID = "unexpected_platform";

        #endif
    }

    //Show the BannerAD
    public void DisplayBanner()
    {
        bannerAD.Show();
    }

    // Called when an ad request has successfully loaded.
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //AD is Loaded, Can Be Shown.
        DisplayBanner();
    }

    // Called when an ad request failed to load.
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //AD failed to Load, Load it Again.
        RequestBanner();
    }

    // Called when an ad is clicked.
    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        
    }

    // Called when the user returned from the app after an ad click.
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        
    }

    // Called when the ad click caused the user to leave the application.
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        
    }

    //Handle all ADEvents for BannerAD
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

    //If the script is enabled, we activete the ADEvents.
    public void OnEnable()
    {
        HandleBannerADEvents(true);
    }

    //If the script is disabled, we deactivete the ADEvents.
    public void OnDisable()
    {
        HandleBannerADEvents(false);
    }

}
