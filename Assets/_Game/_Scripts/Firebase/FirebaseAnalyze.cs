using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAnalyze : SingletonDontDestroy<FirebaseAnalyze> {
    private FirebaseApp app;
    public bool isInit { get; private set; }
    public async void Init() {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            app = FirebaseApp.DefaultInstance;
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            isInit = true;
            //Debug.Log("Firebase initialized successfully!");
        }
        else {
            //Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    }

    #region Button function
    public void LogEvent(string event_name) {
        if (isInit) {
            FirebaseAnalytics.LogEvent(event_name);
        }
    }

    public void LogEvent(string event_name, string param_name, int paramValue) {
        if (isInit) {
            FirebaseAnalytics.LogEvent(event_name, new Parameter[]{
            new Parameter(param_name,paramValue)
        });
        }
    }
    public void LogEvent(string event_name, string param_name, string paramValue) {
        if (isInit) {
            FirebaseAnalytics.LogEvent(event_name, new Parameter[]
            {
            new Parameter(param_name,paramValue)
            });
        }
    }
    #endregion
}