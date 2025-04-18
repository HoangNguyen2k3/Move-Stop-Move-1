using UnityEngine;

public class SettingVibrantAndSound : MonoBehaviour {
    [SerializeField] private GameObject vibrant;
    [SerializeField] private GameObject unvibrant;
    [SerializeField] private GameObject sound;
    [SerializeField] private GameObject unsound;

    private void Start() {
        CheckStatus();
    }
    public void CheckStatus() {
        if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT, 0) == 0) {
            unvibrant.SetActive(true);
            vibrant.SetActive(false);
        }
        else {
            vibrant.SetActive(true);
            unvibrant.SetActive(false);
        }
        if (PlayerPrefs.GetInt(ApplicationVariable.SOUND, 1) == 1) {
            if (SoundManager.Instance)
                SoundManager.Instance.SFXSound.mute = false;
            sound.SetActive(true);
            unsound.SetActive(false);
        }
        else {
            if (SoundManager.Instance)
                SoundManager.Instance.SFXSound.mute = true;
            unsound.SetActive(true);
            sound.SetActive(false);
        }
    }
    public void ClickToVibrant() {
        vibrant.SetActive(true);
        unvibrant.SetActive(false);
        PlayerPrefs.SetInt(ApplicationVariable.VIBRANT, 1);
    }
    public void ClickToUnVibrant() {
        unvibrant.SetActive(true);
        vibrant.SetActive(false);
        PlayerPrefs.SetInt(ApplicationVariable.VIBRANT, 0);
    }
    public void OpenSound() {
        SoundManager.Instance.SFXSound.mute = false;
        sound.SetActive(true);
        unsound.SetActive(false);
        PlayerPrefs.SetInt(ApplicationVariable.SOUND, 1);
    }
    public void UnSound() {
        SoundManager.Instance.SFXSound.mute = true;
        unsound.SetActive(true);
        sound.SetActive(false);
        PlayerPrefs.SetInt(ApplicationVariable.SOUND, 0);
    }



}
