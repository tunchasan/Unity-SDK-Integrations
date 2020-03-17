using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class BannerADs : AdmobADs
{
    private BannerView _bannerAD;

    private string _BANNER_ID;

    //Enum Types
    public enum AdType { Banner_320x50, MediumRectangle_300x250, IABBanner_468x60, Leaderboard_728x90, SmartBanner };

    [Header("Banner AD Configuration")]
    public string _AndroidBannerID;

    public string _IOSBannerID;

    [Header("Banner AD Properties")]
    public AdPosition _BannerPosition;

    public AdType _BannerType;

    //Load Default Banner as SmartBanner
    private void Start()
    {
        RequestBanner();
    }

    //Request Banner depends on platforms and Load the AD.
    private void RequestBanner()
    {
        //Handle which Platform is using on by device and Update BANNER_ID.
        PlatformHandler();

        //Create & Assign New Instance of BannerAD
        _bannerAD = new BannerView(_BANNER_ID, AdTypeHandler(), _BannerPosition);

        //FOR REAL APP
        //AdRequest adRequest = new AdRequest.Builder().Build();

        //FOR TEST APP
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        //Load BannerAD
        _bannerAD.LoadAd(adRequest);

    }

    //AdType Selection Based On _AdType
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

    //Handle Banner_ID depends on Platform.
    public override void PlatformHandler()
    {
        #if UNITY_ANDROID //ANDROID
                _BANNER_ID = _AndroidBannerID;   // This BannerID is for Testing.

        #elif UNITY_IPHONE // IOS
                _BANNER_ID = _IOSBannerID;   // This BannerID is for Testing.

        #else //Other
                _BANNER_ID = "unexpected_platform";

        #endif
    }

    //Show the BannerAD
    public void DisplayBanner()
    {
        _bannerAD.Show();
    }

    //Hide the BannerAD
    public void HideBanner()
    {
        _bannerAD.Hide();
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
    private void HandleBannerADEvents(bool Active)
    {
        if(Active) {

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

        } else {

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

    //If the script is enabled, we activete the ADEvents.
    private void OnEnable()
    {
        HandleBannerADEvents(true);
    }

    //If the script is disabled, we deactivete the ADEvents.
    private void OnDisable()
    {
        HandleBannerADEvents(false);
    }

    //Destory the Banner Ad
    public void DestoryBanner()
    {
        _bannerAD.Destroy();
    }

}
