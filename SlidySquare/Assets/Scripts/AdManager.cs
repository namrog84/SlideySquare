using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    static int count = 0;
    // Use this for initialization
    public static float TimePlayed = 0;


	void Start () {
        count++;
        if (count != 1)
        {
            //Debug.Log("My Precious!");
            //there can be only one!
            Destroy(gameObject);
            return;
        }
        else
        {
            //Debug.Log("Ad is Operational");
            DontDestroyOnLoad(gameObject);
            RequestInterstitial();
        }

        TimePlayed = PlayerPrefs.GetFloat("TotalTime", 0);
    }

	
	// Update is called once per frame
	void Update () {
        TimePlayed += Time.deltaTime;
	}

    public static int PlayCount = 0;
    public static bool Skipped = false;

    public static void PlayAd()
    {
        if (interstitial.IsLoaded())
        {
            Debug.Log("Playing an AD");
            PlayCount = 0;
            Skipped = false;
            interstitial.Show();
        
        }
    }
    public static InterstitialAd interstitial;
    public static void RequestInterstitial()
    {
        PlayCount++;
        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8160801769621904/8167305585";
        #elif UNITY_IPHONE
                            string adUnitId = "ca-app-pub-8160801769621904/8167305585";
        #else
                string adUnitId = "ca-app-pub-8160801769621904/8167305585";
        #endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        //interstitial.AdClosed += HandleIntersistitalLoaded;

        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }


}
