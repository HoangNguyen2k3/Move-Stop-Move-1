using TMPro;
using UnityEngine;

public class LevelGameManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI[] text;
    [SerializeField] private TextMeshProUGUI num_enemy;

    public float enemy_level_easy = 20;
    public float enemy_level_medium = 35;
    public float enemy_level_hard = 50;
    string temp = "CURRENT LEVEL: ";

    private void Start() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.CURRENT_LEVEL_GAME)) {
            PlayerPrefs.SetString(ApplicationVariable.CURRENT_LEVEL_GAME, "EASY");
            ChangeGameLevel_Easy();
        }
        else {
            switch (PlayerPrefs.GetString(ApplicationVariable.CURRENT_LEVEL_GAME)) {
                case "EASY":
                    ChangeGameLevel_Easy();
                    break;
                case "MEDIUM":
                    ChangeGameLevel_Medium();
                    break;
                case "HARD":
                    ChangeGameLevel_Hard();
                    break;
            }
        }
    }
    public void ChangeGameLevel_Medium() {
        GamePlayController.Instance.SettingEnemyMaxCount(enemy_level_medium);
        ChangeTextLevel(temp + "MEDIUM");
        PlayerPrefs.SetString(ApplicationVariable.CURRENT_LEVEL_GAME, "MEDIUM");
        PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, 35);
        num_enemy.text = "ALIVE: " + enemy_level_medium.ToString();
    }
    public void ChangeGameLevel_Easy() {
        GamePlayController.Instance.SettingEnemyMaxCount(enemy_level_easy);
        ChangeTextLevel(temp + "EASY");
        PlayerPrefs.SetString(ApplicationVariable.CURRENT_LEVEL_GAME, "EASY");
        PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, 20);
        num_enemy.text = "ALIVE: " + enemy_level_easy.ToString();
    }
    public void ChangeGameLevel_Hard() {
        GamePlayController.Instance.SettingEnemyMaxCount(enemy_level_hard);
        ChangeTextLevel(temp + "HARD");
        PlayerPrefs.SetString(ApplicationVariable.CURRENT_LEVEL_GAME, "HARD");
        PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, 50);
        num_enemy.text = "ALIVE: " + enemy_level_hard.ToString();
    }
    public void ChangeTextLevel(string current_level) {
        foreach (TextMeshProUGUI text in text) {
            text.text = current_level;
        }
    }
}
