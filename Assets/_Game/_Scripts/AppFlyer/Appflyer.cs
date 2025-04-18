using AppsFlyerSDK;
using UnityEngine;

public class AppsFlyerInit : MonoBehaviour {
    private string appsFlyerDevKey = "XDdweEXXFBrNngtR3848T3";
    private string appleAppId = "YOUR_APP_ID";
    public bool isInit { get; private set; }
    void Start() {
        Init();
    }

    public void Init() {
        AppsFlyer.initSDK(appsFlyerDevKey, appleAppId);
        AppsFlyer.startSDK();
        isInit = true;
    }

    public void SendTestEvent() {
        AppsFlyer.sendEvent("test_event", new System.Collections.Generic.Dictionary<string, string>
        {
            { "event_name", "app_opened" },
            { "time", System.DateTime.Now.ToString() }
        });

        //Debug.Log("AppsFlyer event sent!");
    }
}
