using AudienceNetwork;
using System.Threading.Tasks;
using UnityEngine;

public class FacebookInterstitial : MonoBehaviour {
    private string interstitialID = "VID_HD_9_16_39S_APP_INSTALL#YOUR_PLACEMENT_ID";
    private InterstitialAd interstitialAd;
    private bool isLoaded;
    public bool isInit { get; private set; }

    private void Start() {
    }
    public void LoadInterstitial() {
        this.interstitialAd = new InterstitialAd(interstitialID);
        this.interstitialAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.interstitialAd.InterstitialAdDidLoad = (delegate () {
            //    Debug.Log("Interstitial ad loaded.");
            this.isLoaded = true;
        });
        interstitialAd.InterstitialAdDidFailWithError = (delegate (string error) {
            //    Debug.Log("Interstitial ad failed to load with error: " + error);
            LoadInterstitial();
        });
        interstitialAd.InterstitialAdWillLogImpression = (delegate () {
            //    Debug.Log("Interstitial ad logged impression.");
        });
        interstitialAd.InterstitialAdDidClick = (delegate () {
            //    Debug.Log("Interstitial ad clicked.");
        });

        this.interstitialAd.interstitialAdDidClose = (delegate () {
            //    Debug.Log("Interstitial ad did close.");
            if (this.interstitialAd != null) {
                this.interstitialAd.Dispose();
            }
            LoadInterstitial();
        });

        // Initiate the request to load the ad.
        this.interstitialAd.LoadAd();
    }
    // Show button
    public void ShowInterstitial() {
        if (this.isLoaded) {
            this.interstitialAd.Show();
            this.isLoaded = false;
            isInit = true;
        }
        else {
            Debug.Log("Interstitial Ad not loaded!");
        }
    }
    public void ShowAddFacebook() {
        int temp_rand = Random.Range(0, 4);
        if (temp_rand == 0) {
            /*            LoadInterstitial();
                        await Task.Delay(200);*/
            ShowInterstitial();
        }
    }
}
