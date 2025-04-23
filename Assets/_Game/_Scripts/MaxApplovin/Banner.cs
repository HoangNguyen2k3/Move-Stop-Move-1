using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{
    public static Banner instance;
    string bannerAdUnitId = "e9d23e60e7b23dab";
    public bool isZombieMode = false;
    public bool isSuccess = false;
    /*    private void Awake()
        {
            if (isZombieMode)
                InitBanner();
        }*/

    public void InitBanner() {
        instance = this;
        Init();
        if (GameManager.Instance != null && MaxSdk.IsInitialized()) {
            LoadBanner();
        }

        /*        if (!MaxSdk.IsInitialized())
                {
                    MaxSdk.InitializeSdk();
                }
                Init();
                MaxSdk.SetHasUserConsent(true);
                MaxSdk.SetDoNotSell(false);     
                if (!MaxSdk.IsInitialized())
                    MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "d449665f-b75e-4767-a48c-f36c91853b84" }); // Thay b?ng ID th?t
                MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
                {
                    MaxSdk.ShowMediationDebugger();
                    if (GameManager.Instance != null && MaxSdk.IsInitialized())
                    {
                        Debug.Log("teset");
                        LoadBanner();
                    }
                };*/
    }

    private void OnEnable() {
        if (GamePlayController.Instance != null)
            /*MaxSdk.LoadBanner(bannerAdUnitId);*/
            LoadBanner();
    }
    // Start is called before the first frame update

    /*  void Start()
      {
          Init();
          MaxSdk.SetHasUserConsent(true); // Gi? l?p ng??i dùng ??ng ý (dùng cho test)
          MaxSdk.SetDoNotSell(false);     // Gi? l?p không ch?n "Do Not Sell"
          if (!MaxSdk.IsInitialized())
              MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "d449665f-b75e-4767-a48c-f36c91853b84" }); // Thay b?ng ID th?t
          MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
          {
              // AppLovin SDK is initialized, start loading ads
              // Banners are automatically sized to 320?50 on phones and 728?90 on tablets
              // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
              //LoadBanner();
              MaxSdk.ShowMediationDebugger();
              if (GameManager.Instance != null && MaxSdk.IsInitialized())
              {
                  Debug.Log("teset");
                  LoadBanner();
              }
          };
          //        MaxSdk.SetSdkKey("iBJBZKgGo-0qqAq7TEpTUwQvhiD-rH6vTDqDeKyQxBOFCH-OBJt8nFs7dP_-A715Z1pu4UC6HSTG-EISryHdg2");
          // MaxSdk.SetUserId("USER_ID");
          //if (MaxSdk.IsInitialized) 
          if (!MaxSdk.IsInitialized())
          {
              MaxSdk.InitializeSdk();
          }
      }*/
    private void Init() {
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += Banner_OnAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += Banner_OnAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += Banner_OnAdClickedEvent;
    }

    private void Banner_OnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2) {
        //throw new System.NotImplementedException();
    }

    private void Banner_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2) {
        // throw new System.NotImplementedException();
        isSuccess = true;
    }

    private void Banner_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2) {
        //throw new System.NotImplementedException();
        isSuccess = false;
    }

    public void LoadBanner() {
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    public void HideBanner() {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    // Update is called once per frame
    void Update() {

    }
    private void OnDisable() {
        if (MaxSdk.IsInitialized())
            HideBanner();
    }
}