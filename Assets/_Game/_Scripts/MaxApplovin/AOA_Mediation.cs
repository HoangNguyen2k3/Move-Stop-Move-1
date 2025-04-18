using UnityEditor;
using UnityEngine;

public class AOA_Mediation : MonoBehaviour {
    private string AppOpenAdUnitId = "ca-app-pub-6409857233709298/6124157663";
    public bool isInit { get; private set; }
    private void OnApplicationPause(bool pause) {
        if (pause) {
        }
        else {
            LoadAndShow();
        }
    }
    public void LoadAd() {
        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
        isInit = true;
    }

    public void ShowAdIfReady() {
        if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId)) {
            MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
        }
        else {
            LoadAd();
        }
    }
    public void LoadAndShow() {
        LoadAd();
        ShowAdIfReady();

    }
}
