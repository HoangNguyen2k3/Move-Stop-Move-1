using System;
using UnityEngine;

public class Rewarded : Singleton<Rewarded>
{
    [Header("Normal Map")]
    [Header("---------WeaponAndClothesShop")]
    [SerializeField] private WeaponShopUI weaponShopUI;
    [SerializeField] private ChooseType clothes;
    [Header("----------Revive")]
    [SerializeField] private UIGeneratePress revive_show1;
    [SerializeField] private UIGeneratePress revive_show2;
    [Header("Zombie Map")]
    [SerializeField] private ZombieGameController zombieManager;
    [SerializeField] private EnterToOtherScene enterToOtherScene;

    string adUnitId = "b14b72eb2d1e8ff3";
    public string current_ads;
    public bool inZombieMode = false;
    private bool firstTime1 = true;
    public GameObject failAds;
    bool hasReceivedReward = false;

    void Start() {
        SubscribeEvents();
    }

    void OnDestroy() {
        UnsubscribeEvents();
    }

    void SubscribeEvents() {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    }

    void UnsubscribeEvents() {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
    }

    public void LoadRewardedAd() {
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        if (inZombieMode) {
            if (current_ads == "ReviveZombie") {
                if (ZombieGameController.Instance.currentInRevive == true) {
                    if (current_ads == "ReviveZombie") {
                        Time.timeScale = 0f;
                    }
                }
                MaxSdk.ShowRewardedAd(adUnitId);
            }
            else {
                if (current_ads == "ChooseAbilities") {
                    Time.timeScale = 0f;
                }
                MaxSdk.ShowRewardedAd(adUnitId);
            }
        }
        else {
            if (GameStateManager.Instance.currentStateGame != ApplicationVariable.StateGame.InRevive) {
                if (current_ads == "Revive" || current_ads == "ChooseAbilities" || current_ads == "ReviveZombie") {
                    Time.timeScale = 0f;
                }
                MaxSdk.ShowRewardedAd(adUnitId);
            }
        }
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
        FailSmtFromAds();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
        FailSmtFromAds();

        if (firstTime1) {
            LoadRewardedAd();
            firstTime1 = false;
        }
        else {
            FailSmtFromAds();
        }
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        if (this == null || gameObject == null) return; // <-- Check to prevent crash
                                                        // Debug.Log("OnRewardedAdHiddenEvent called from: " + gameObject.name);

        if (hasReceivedReward) {
            GainSmtFromAds();
        }
        else {
            ExitAds();
        }
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) {
        hasReceivedReward = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void SettingAds(string name) {
        current_ads = name;
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    public void GainSmtFromAds() {
        switch (current_ads) {
            case "Revive":
                Time.timeScale = 1f;
                GamePlayController.Instance?.Revive();
                revive_show1.ShowAndHiddenGameObject();
                break;
            case "X3Money":
                GamePlayController.Instance?.Earnx3Gold();
                GamePlayController.Instance?.RestartScene();
                break;
            case "ChooseAbilities":
                Time.timeScale = 1f;
                ZombieGameController.Instance?.ChooseAbilities();
                ZombieGameController.Instance?.ChangeInShop();
                //                ZombieManager.Instance?.CheckAdsCloth();
                break;
            case "ReviveZombie":
                Time.timeScale = 1f;
                ZombieGameController.Instance?.RevivePlayer();
                ZombieGameController.Instance?.ShowAndHidden();
                break;
            case "WatchAds":
                weaponShopUI?.AddOneTimeAds();
                break;
            case "AdsOneTime":
                clothes?.ClickAds();
                break;
            case "GainWeaponCustom":
                weaponShopUI?.GainAdsCustomWeapon();
                break;
            case "X3MoneyZombie":
                zombieManager?.EarnCoinX3();
                enterToOtherScene.EnterToScene();
                break;
        }
    }

    public void ExitAds() {
        Time.timeScale = 1f;
        switch (current_ads) {
            case "Revive":
                Time.timeScale = 1f;
                GamePlayController.Instance?.DestroyPlayer();
                revive_show2.ShowAndHiddenGameObject();
                break;
            case "ChooseAbilities":
            case "ReviveZombie":
                Time.timeScale = 1f;
                ZombieGameController.Instance?.ShowAndHidden();
                break;
        }
    }

    public void FailSmtFromAds() {
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
        }
        ExitAds();
        if (failAds != null)
            failAds.SetActive(true);

    }
}
