using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobADs : MonoBehaviour
{
    //Admob APP ID
    public string APP_ID { get; set; }

    private void Awake()
    {
        APP_ID = "ca-app-pub-1508527570491486~9884488791";
    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK. When publish our app, we should use it.
        //MobileAds.Initialize(appId);
    }

    //APP_ID Getter
    public string getAPP_ID() { return APP_ID; }

    //APP_ID Setter
    public void setAPP_ID(string _APP_ID) { this.APP_ID = _APP_ID; }

}
