using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinZombieMode : MonoBehaviour
{
    [SerializeField] private GameObject[] day_bar;
    [SerializeField] private TextMeshProUGUI day;
    [SerializeField] private SaveDayZombieMode saveDayZombieMode;
    [SerializeField] private Color done_color;
    [SerializeField] private Color notdone_color;

    [SerializeField] private TextMeshProUGUI[] daybar;
    private void OnEnable()
    {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.win_sound);
        int temp = saveDayZombieMode.current_day - 1;

        if (saveDayZombieMode.current_day > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                daybar[i].text = "Day" + (i + 6).ToString();
            }
            temp -= 5;
        }
        day.text = "you survived day " + saveDayZombieMode.current_day.ToString() + "!";
        for (int i = 0; i < day_bar.Length; i++)
        {
            if (i < (temp + 1))
            {
                day_bar[i].GetComponent<Image>().color = done_color;
            }
            else
            {
                day_bar[i].GetComponent<Image>().color = notdone_color;
            }
        }
    }

}
