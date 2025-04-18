using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [SerializeField] private CharaterObj playerCharacter;
    [SerializeField] private FullSkinObject[] skinObject;
    [SerializeField] private ClotherShop[] hats_save;
    [SerializeField] private ClotherShop[] pants_save;
    [SerializeField] private ClotherShop[] shield_save;

    private void Awake() {
        LoadData();
    }
    private void OnDisable() {
        SaveData();
    }
    private void OnApplicationQuit() {
        SaveData();
    }
    private void OnApplicationPause(bool pause) {
        SaveData();
    }
    #region Load Data
    private void LoadData() {
        playerCharacter.InitPlayer();
        if (!PlayerPrefs.HasKey(ApplicationVariable.FIRST_SAVE)) { return; }
        LoadDataPlayerInfo();
        LoadDataFullSkin();

        LoadDataHat();

        LoadDataPants();

        LoadDataShield();
    }
    private void LoadDataShield() {
        int temp_check = -1;
        ShieldWrap shieldWrapper = SavingData.LoadData(new ShieldWrap(), ApplicationVariable.PATH_CLOTHES_SAVE + "shield");
        if (shieldWrapper.status != null) {
            for (int i = 0; i < shieldWrapper.status.Length; i++) {
                shield_save[i].status = shieldWrapper.status[i];
                if (shield_save[i].status == "Selected") {
                    temp_check = i;
                }
            }
        }
        if (temp_check != -1) {
            playerCharacter.skinClother[2] = shield_save[temp_check];
        }
        else {
            playerCharacter.skinClother[2] = null;
        }
    }

    private void LoadDataPants() {
        int temp_check = -1;
        PantWrap pantWrapper = SavingData.LoadData(new PantWrap(), ApplicationVariable.PATH_CLOTHES_SAVE + "pants");
        if (pantWrapper.status != null) {
            for (int i = 0; i < pantWrapper.status.Length; i++) {
                pants_save[i].status = pantWrapper.status[i];
                if (pants_save[i].status == "Selected") {
                    temp_check = i;
                }
            }
        }
        if (temp_check != -1) {
            playerCharacter.skinClother[1] = pants_save[temp_check];
        }
        else {
            playerCharacter.skinClother[1] = null;
        }
    }

    private void LoadDataHat() {
        int temp_check = -1;
        HatWrap hatWrapper = SavingData.LoadData(new HatWrap(), ApplicationVariable.PATH_CLOTHES_SAVE + "hat");
        if (hatWrapper.status != null) {
            for (int i = 0; i < hatWrapper.status.Length; i++) {
                hats_save[i].status = hatWrapper.status[i];
                if (hats_save[i].status == "Selected") {
                    temp_check = i;
                }
            }
        }
        if (temp_check != -1) {
            playerCharacter.skinClother[0] = hats_save[temp_check];
        }
        else {
            playerCharacter.skinClother[0] = null;
        }
    }

    private void LoadDataFullSkin() {
        int temp_check = -1;
        WrapperFullSkin skinWrapper = SavingData.LoadData(new WrapperFullSkin(), ApplicationVariable.PATH_CLOTHES_SAVE + "fullskin");
        if (skinWrapper.status != null && skinObject.Length == skinWrapper.status.Length) {
            for (int i = 0; i < skinObject.Length; i++) {
                skinObject[i].status = skinWrapper.status[i];
                if (skinObject[i].status == "Selected") {
                    temp_check = i;
                }
            }
        }
        if (temp_check != -1) {
            playerCharacter.fullSkinPlayer = skinObject[temp_check];
        }
        else {
            playerCharacter.fullSkinPlayer = null;
        }
    }

    private void LoadDataPlayerInfo() {
        PlayerCharacterWrapper character = SavingData.LoadData(new PlayerCharacterWrapper(), ApplicationVariable.PATH_CLOTHES_PLAYER);
        playerCharacter.status_fullskin = character.status_fullskin;
        playerCharacter.coolDownAttack = character.cooldown_attack;
        playerCharacter.beginRange = character.beginRange;
    }
    #endregion
    #region Save Data
    private void SaveData() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.FIRST_SAVE)) {
            PlayerPrefs.SetInt(ApplicationVariable.FIRST_SAVE, 1);
        }

        SaveDataPlayerInfo();

        SaveDataFullSkin();

        SaveDataHat();

        SaveDataPants();

        SaveDataShield();
    }

    private void SaveDataShield() {
        string[] shieldStatusArray = new string[shield_save.Length];
        for (int i = 0; i < shield_save.Length; i++) {
            shieldStatusArray[i] = shield_save[i].status;
        }
        ShieldWrap shieldWrapper = new ShieldWrap { status = shieldStatusArray };
        SavingData.SaveData(shieldWrapper, ApplicationVariable.PATH_CLOTHES_SAVE + "shield");
    }

    private void SaveDataPants() {
        string[] pantStatusArray = new string[pants_save.Length];
        for (int i = 0; i < pants_save.Length; i++) {
            pantStatusArray[i] = pants_save[i].status;
        }
        PantWrap pantWrapper = new PantWrap { status = pantStatusArray };
        SavingData.SaveData(pantWrapper, ApplicationVariable.PATH_CLOTHES_SAVE + "pants");
    }

    private void SaveDataHat() {
        string[] hatStatusArray = new string[hats_save.Length];
        for (int i = 0; i < hats_save.Length; i++) {
            hatStatusArray[i] = hats_save[i].status;
        }
        HatWrap hatWrapper = new HatWrap { status = hatStatusArray };
        SavingData.SaveData(hatWrapper, ApplicationVariable.PATH_CLOTHES_SAVE + "hat");
    }

    private void SaveDataFullSkin() {
        string[] skinStatusArray = new string[skinObject.Length];
        for (int i = 0; i < skinObject.Length; i++) {
            skinStatusArray[i] = skinObject[i].status;
        }
        WrapperFullSkin skinWrapper = new WrapperFullSkin { status = skinStatusArray };
        SavingData.SaveData(skinWrapper, ApplicationVariable.PATH_CLOTHES_SAVE + "fullskin");
    }

    private void SaveDataPlayerInfo() {
        PlayerCharacterWrapper character = new PlayerCharacterWrapper {
            status_fullskin = playerCharacter.status_fullskin,
            cooldown_attack = playerCharacter.coolDownAttack,
            beginRange = playerCharacter.beginRange
        };
        SavingData.SaveData(character, ApplicationVariable.PATH_CLOTHES_PLAYER);
    }
    #endregion
}
