using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class dataToSave {
    public string userName;
    public int totalCoin;
    public int currentDayInZombieMode;
    public string current_Weapon_Unlock;
    public string statusVolume;
    public string currentLevelNormalMap;
}

public class Firebasedatabase : SingletonDontDestroy<Firebasedatabase> {
    public dataToSave dts;
    private string userId;
    private string user_device_name;
    private DatabaseReference database;
    public bool isInit { get; private set; }
    public async void InitFb() {
        //        Debug.Log("Firebase Start Init");
        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        //        Debug.Log("Firebase dependency status: " + dependencyStatus);
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            //           Debug.Log("Database initialized!");
            userId = SystemInfo.deviceUniqueIdentifier;
            user_device_name = SystemInfo.deviceName;

            //           Debug.Log("Firebase initialized.");
            //           Debug.Log("Device ID: " + userId);
            //           Debug.Log("Device Name: " + user_device_name);
            //InvokeRepeating(nameof(SaveData), 5f, 60f);
            SaveData();
            isInit = true;
        }
        else {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    }

    public void SaveDataToFB() {
        if (database == null) {
            Debug.LogError("Firebase database is not initialized yet!");
            return;
        }

        string json = JsonUtility.ToJson(dts);
        database.Child("users").Child(user_device_name).SetRawJsonValueAsync(json);
    }

    public void SaveDataFn() {
        if (database == null) {
            Debug.LogError("Firebase database is not initialized yet!");
            return;
        }

        string json = JsonUtility.ToJson(dts);
        database.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void LoadDataFn() {
        StartCoroutine(LoadDataEnum());
    }

    private IEnumerator LoadDataEnum() {
        if (database == null) {
            Debug.LogError("Firebase database is not initialized yet!");
            yield break;
        }

        var serverData = database.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(() => serverData.IsCompleted);

        if (serverData.Exception != null) {
            Debug.LogError("L?i khi t?i d? li?u: " + serverData.Exception);
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if (!string.IsNullOrEmpty(jsonData)) {
            //Debug.Log("D? li?u server ?ã ???c t?i");
            dts = JsonUtility.FromJson<dataToSave>(jsonData);
        }
        else {
            //Debug.Log("Không tìm th?y d? li?u trên server");
        }
    }

    public void SaveData() {
        if (database == null) {
            Debug.LogError("Firebase database is not initialized yet!");
            return;
        }

        dts.userName = PlayerPrefs.GetString("NamePlayer", "YOU");
        dts.totalCoin = (int)PlayerPrefs.GetFloat("Coin", 0);
        dts.currentDayInZombieMode = PlayerPrefs.GetInt("DayZombieMode", 1);
        dts.current_Weapon_Unlock = PlayerPrefs.GetString("EquipCurrentWeapon", "Hammer");
        dts.statusVolume = PlayerPrefs.GetInt("Sound", 1) == 1 ? "Turn on" : "Turn off";
        dts.currentLevelNormalMap = PlayerPrefs.GetString("LevelGame", "EASY");

        SaveDataToFB();
        //Debug.Log("?ã l?u d? li?u thành công");
    }

    public void SaveData1() {
        if (database == null) {
            Debug.LogError("Firebase database is not initialized yet!");
            return;
        }

        dts.userName = PlayerPrefs.GetString("NamePlayer", "YOU");
        dts.totalCoin = (int)PlayerPrefs.GetFloat("Coin", 0);
        dts.currentDayInZombieMode = PlayerPrefs.GetInt("DayZombieMode", 1);
        dts.current_Weapon_Unlock = PlayerPrefs.GetString("EquipCurrentWeapon", "Hammer");
        dts.statusVolume = PlayerPrefs.GetInt("Sound", 1) == 1 ? "Turn on" : "Turn off";
        dts.currentLevelNormalMap = PlayerPrefs.GetString("LevelGame", "EASY");

        string json = JsonUtility.ToJson(dts);
        string randomKey = user_device_name + UnityEngine.Random.Range(0, 1000000000).ToString();
        database.Child("users").Child(randomKey).SetRawJsonValueAsync(json);
        // Debug.Log("?ã l?u d? li?u v?i mã ng?u nhiên");
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveData();
        }
    }

    private void OnApplicationQuit() {
        // Debug.Log("?ng d?ng ?ang thoát - l?u d? li?u...");
        SaveData();
    }
}
