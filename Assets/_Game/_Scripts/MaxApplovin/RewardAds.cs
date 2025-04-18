using System;
using UnityEngine;
public class RewardAds : Singleton<RewardAds>
{
    string adUnitId = "b14b72eb2d1e8ff3";
    Action OnRewardSuccess, OnRewardFail;
    bool isPauseGame = false;
    ApplicationVariable.ConditionAds conditionAds;


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
        if (isPauseGame) {
            if (conditionAds == ApplicationVariable.ConditionAds.None) {
                Time.timeScale = 0;
                MaxSdk.ShowRewardedAd(adUnitId);
            }
            else if (conditionAds == ApplicationVariable.ConditionAds.ReviveNormal) {
                if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InRevive) {
                    Time.timeScale = 0f;
                    MaxSdk.ShowRewardedAd(adUnitId);
                }
            }
            else {
                if (ZombieGameController.Instance.currentInRevive == true) {
                    Time.timeScale = 0f;
                    MaxSdk.ShowRewardedAd(adUnitId);
                }
            }
        }
        else {
            MaxSdk.ShowRewardedAd(adUnitId);
        }
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
        OnRewardFail?.Invoke();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        if (isPauseGame) {
            Time.timeScale = 1;
        }
        isPauseGame = false;
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) {
        OnRewardSuccess?.Invoke();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowRewardAds(Action OnComplete, Action OnFail, bool isPause, ApplicationVariable.ConditionAds conditionAds) {
        OnRewardSuccess = OnComplete;
        OnRewardFail = OnFail;
        isPauseGame = isPause;
        this.conditionAds = conditionAds;
        LoadRewardedAd();
    }
}
