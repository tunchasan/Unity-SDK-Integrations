using UnityEngine;
using Library.Advertisement.Admob;
using System;
using GoogleMobileAds.Api;
using Library.Advertisement.UnityAd;

public class AdverstisementExample : MonoBehaviour
{
    private AdmobBannerAd _bannerADs;

    private AdmobRewardedVideoAd _rewardedADs, _rewardedADs2;

    private AdmobInterstitialAd _interstitialADs;

    private UnityBannerAd _bannerAd;

    private UnityRewardedVidedAd _rewardedVideoAd;

    private UnityVideoAd _videoAds;

    void Awake()
    {
        _bannerADs = new AdmobBannerAd();

        _rewardedADs = new AdmobRewardedVideoAd("Gem", 50);

        _rewardedADs2 = new AdmobRewardedVideoAd("PowerUp_MAGNET", 2);

        _interstitialADs = new AdmobInterstitialAd();
    }

    private void Start()
    {
        _bannerADs.OnAdLoaded += OnBannerAdLoaded;
        _bannerADs.OnAdOpened += OnBannerAdOpened;
        _bannerADs.OnAdClosed += OnBannerAdClosed;
        _bannerADs.OnAdFailedToLoad += OnBannerAdFailedToLoad;
        _bannerADs.OnAdLeavingApplication += OnBannerAdLeavingApplication;

        _rewardedADs.OnAdLoaded += OnRewardAdLoaded;
        _rewardedADs.OnAdClosed += OnRewardedAdClosed;
        _rewardedADs.OnAdOpened += OnRewardAdOpened;
        _rewardedADs.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
        _rewardedADs.OnAdFailedToShow += OnRewardedAdFailedToShow;
        _rewardedADs.OnUserEarnedReward += OnRewardedAdEarnedReward;
    }

    private void OnRewardedAdEarnedReward(string rewardType, int rewardAmount)
    {
        Debug.Log("OnRewardedAdEarnedReward! " + rewardType + " " + rewardAmount);
    }

    private void OnRewardedAdFailedToShow()
    {
        Debug.Log("OnRewardedAdFailedToShow!");
    }

    private void OnRewardedAdFailedToLoad()
    {
        Debug.Log("OnRewardedAdFailedToLoad!");
    }

    private void OnRewardAdOpened()
    {
        Debug.Log("OnRewardAdOpened!");
    }

    private void OnRewardedAdClosed()
    {
        Debug.Log("OnRewardedAdClosed!");
    }

    private void OnRewardAdLoaded()
    {
        Debug.Log("OnRewardAdLoaded!");
    }

    private void OnBannerAdLeavingApplication()
    {
        Debug.Log("OnBannerAdLeavingApplication!");
    }

    private void OnBannerAdFailedToLoad()
    {
        Debug.Log("OnBannerAdFailedToLoad!");
    }

    private void OnBannerAdClosed()
    {
        Debug.Log("OnBannerAdClosed!");
    }

    private void OnBannerAdOpened()
    {
        Debug.Log("OnBannerAdOpened!");
    }

    private void OnBannerAdLoaded()
    {
        Debug.Log("OnBannerAdLoaded!");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) // Interstitial
        {
            _bannerADs.LoadBannerAd();
        }

        else if (Input.GetKey("s"))
        {
            _rewardedADs.LoadRewardedVideoAd();
        }

        else if (Input.GetKey("d"))
        {
            _rewardedADs2.LoadRewardedVideoAd();
        }

        else if (Input.GetKey("a"))
        {
            _interstitialADs.LoadInterstitialAd();
        }

    }

}
