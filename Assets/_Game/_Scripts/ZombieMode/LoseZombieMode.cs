using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseZombieMode : MonoBehaviour
{
    [SerializeField] private GameObject[] day_bar;
    [SerializeField] private GameObject icon;
    [SerializeField] private SaveDayZombieMode saveDayZombieMode;
    [SerializeField] private Color done_color;
    [SerializeField] private Color notdone_color;
    [SerializeField] private Color current_color;
    [SerializeField] private TextMeshProUGUI[] day;
    private void OnEnable() {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.lose_sound);
        int temp = saveDayZombieMode.current_day - 1;

        if (saveDayZombieMode.current_day > 5) {
            for (int i = 0; i < 5; i++) {
                day[i].text = "Day" + (i + 6).ToString();
            }
            temp -= 5;
        }
        icon.transform.position = day_bar[temp].transform.position;
        for (int i = 0; i < day_bar.Length; i++) {
            if (i == (temp)) {
                day_bar[i].GetComponent<Image>().color = current_color;
            }
            else if (i < (temp)) {
                day_bar[i].GetComponent<Image>().color = done_color;
            }
            else {
                day_bar[i].GetComponent<Image>().color = notdone_color;
            }
        }
    }
}
