using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingSoundUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color OnColor;
    [SerializeField] private Color OffColor;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool isSoundButton;

    private void OnEnable()
    {
        if (isSoundButton)
        {
            if (PlayerPrefs.GetInt(ApplicationVariable.SOUND, 0) == 0)
            {
                text.text = "OFF";
                image.color = OffColor;
                if (SoundManager.Instance)
                    SoundManager.Instance.SFXSound.mute = true;
            }
            else
            {
                text.text = "ON";
                image.color = OnColor;
                if (SoundManager.Instance)
                    SoundManager.Instance.SFXSound.mute = false;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT, 0) == 0)
            {
                text.text = "OFF";
                image.color = OffColor;
            }
            else
            {
                text.text = "ON";
                image.color = OnColor;
            }
        }
    }
    public void ClickSound()
    {
        if (isSoundButton)
        {
            if (text.text == "ON")
            {
                text.text = "OFF";
                image.color = OffColor;
                PlayerPrefs.SetInt(ApplicationVariable.SOUND, 0);
                if (SoundManager.Instance)
                    SoundManager.Instance.SFXSound.mute = true;
            }
            else
            {
                text.text = "ON";
                image.color = OnColor;
                PlayerPrefs.SetInt(ApplicationVariable.SOUND, 1);
                if (SoundManager.Instance)
                    SoundManager.Instance.SFXSound.mute = false;
            }
        }
        else
        {
            if (text.text == "ON")
            {
                text.text = "OFF";
                image.color = OffColor;
                PlayerPrefs.SetInt(ApplicationVariable.VIBRANT, 0);
            }
            else
            {
                text.text = "ON";
                image.color = OnColor;
                PlayerPrefs.SetInt(ApplicationVariable.VIBRANT, 1);
            }
        }
    }
}
