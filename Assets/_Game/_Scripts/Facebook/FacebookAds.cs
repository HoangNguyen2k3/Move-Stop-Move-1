using AudienceNetwork;
using TMPro;
using UnityEngine;

public class FacebookAds : MonoBehaviour
{
    private RewardedVideoAd rewardedVideoAd;
    bool isLoaded;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;
    public void LoadRewardedVideo()
    {
        text.text = "SetUp";
        // Create the rewarded video unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        this.rewardedVideoAd = new RewardedVideoAd("VID_HD_16_9_15S_APP_INSTALL#YOUR_PLACEMENT_ID");

        this.rewardedVideoAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.rewardedVideoAd.RewardedVideoAdDidLoad = (delegate ()
        {
            text1.text = "SetUp 1";
            // Debug.Log("RewardedVideo ad loaded.");
            this.isLoaded = true;
        });
        this.rewardedVideoAd.RewardedVideoAdDidFailWithError = (delegate (string error)
        {
            text3.text = "SetUp 3 loi " + error;
            // Debug.Log("RewardedVideo ad failed to load with error: " + error);
        });
        this.rewardedVideoAd.RewardedVideoAdWillLogImpression = (delegate ()
        {
            // Debug.Log("RewardedVideo ad logged impression.");
        });
        this.rewardedVideoAd.RewardedVideoAdDidClick = (delegate ()
        {
            // Debug.Log("RewardedVideo ad clicked.");
        });

        this.rewardedVideoAd.RewardedVideoAdDidClose = (delegate ()
        {
            //  Debug.Log("Rewarded video ad did close.");
            if (this.rewardedVideoAd != null)
            {
                this.rewardedVideoAd.Dispose();
            }
        });

        // Initiate the request to load the ad.
        this.rewardedVideoAd.LoadAd();
    }

    public void ShowRewardedVideo()
    {
        if (this.isLoaded)
        {
            this.rewardedVideoAd.Show();
            this.isLoaded = false;
            text2.text = "SetUp 2";
        }
        else
        {
            // Debug.Log("Ad not loaded. Click load to request an ad.");
        }
    }
}
