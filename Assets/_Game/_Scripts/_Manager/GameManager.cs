using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonDontDestroy<GameManager>
{
    public FirebaseAnalyze firebaseAnalyzePrefab;
    public Firebasedatabase firebaseDatabasePrefab;
    public AppsFlyerInit appsFlyerInitPrefab;
    public FacebookInterstitial Facebook;
    public AOA_Mediation AOA_Mediation;

    private void Start() {
        InitAppLovin();
        StartCoroutine(InitializeAllSDKs());
    }

    private IEnumerator InitializeAllSDKs() {
        //Luon load fire base dau tien
        firebaseAnalyzePrefab?.Init();
        yield return new WaitForSeconds(.1f);
        yield return new WaitUntil(() => firebaseAnalyzePrefab.isInit);

        firebaseDatabasePrefab?.InitFb();
        yield return new WaitForSeconds(.1f);
        yield return new WaitUntil(() => firebaseDatabasePrefab.isInit);

        appsFlyerInitPrefab?.Init();
        yield return new WaitUntil(() => appsFlyerInitPrefab.isInit);


        AOA_Mediation?.LoadAd();
        yield return new WaitUntil(() => AOA_Mediation.isInit);
        try {
            Facebook?.LoadInterstitial();
        }
        catch (Exception e) {
            Debug.LogError("Error loading Facebook Interstitial: " + e.Message);
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(1);
    }

    private static void InitAppLovin() {
        if (!MaxSdk.IsInitialized()) {
            MaxSdk.InitializeSdk();
        }

        MaxSdk.SetHasUserConsent(true);
        MaxSdk.SetDoNotSell(false);

        if (!MaxSdk.IsInitialized()) {
            MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "d449665f-b75e-4767-a48c-f36c91853b84" });
        }

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            MaxSdk.ShowMediationDebugger();
        };
    }
}
