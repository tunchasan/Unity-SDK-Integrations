using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialADs : AdmobADs
{
    private InterstitialAd _InterstitialAD;

    private string _Interstitial_ID;

    [Header("Interstitial AD Properties")]
    public string _AndroidInterstitialAdID;

    public string _IOSInterstitialAdID;

    //Request Banner depends on platforms and Load the AD.
    public void RequestInterstitial()
    {
        //Create & Assign New Instance of InterstitialAD
        _InterstitialAD = new InterstitialAd(_Interstitial_ID);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        HandleInterstitialADEvents(true);

        //FOR TEST APP
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        //Load BannerAD
        _InterstitialAD.LoadAd(adRequest);
    }

    //Handle Interstitial_ID depends on Platform.
    public override void PlatformHandler()
    {
        #if UNITY_ANDROID //ANDROID
                _Interstitial_ID = _AndroidInterstitialAdID;   // This InterstitialID is for Testing.

        #elif UNITY_IPHONE // IOS
                _Interstitial_ID = _IOSInterstitialAdID;   // This InterstitialID is for Testing.

        #else //Other
                _Interstitial_ID = "unexpected_platform";

        #endif
    }

    //Show the InterstitialAD
    private void DisplayInterstitial()
    {
        _InterstitialAD.Show();
    }

    // Called when an ad request has successfully loaded.
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //AD is Loaded, Can Be Shown.
        DisplayInterstitial();
    }

    // Called when an ad request failed to load.
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //AD failed to Load, Load it Again.
        RequestInterstitial();
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

    //Handle all ADEvents for InterstitialAD
    private void HandleInterstitialADEvents(bool Active)
    {
        if(Active) {

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

        } else {

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

    //If the script is enabled, we activete the ADEvents.
    private void OnEnable()
    {
        HandleInterstitialADEvents(true);
    }

    //If the script is disabled, we deactivete the ADEvents.
    private void OnDisable()
    {
        HandleInterstitialADEvents(false);
    }

    //Destory the Banner Ad
    public void DestroyInterstitalAd()
    {
        _InterstitialAD.Destroy();
    }

}
