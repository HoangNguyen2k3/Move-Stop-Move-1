using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LosingGameState : MonoBehaviour, IGameState
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject gameOverReal;
    [SerializeField] private UIGeneratePress uiGenerate;
    [SerializeField] private GameObject reviveGameObj;
    //Setting
    [SerializeField] private UIGeneratePress setting;
    [SerializeField] private GameObject smtHidden1;
    [SerializeField] private GameObject smtHidden2;

    public Sprite[] sprite_Icon;
    public Image BeginLevelIcon;
    public Image NextLevelIcon;
    public TextMeshProUGUI beginLevel;
    public TextMeshProUGUI nextLevel;
    public Slider sliderProcess;
    private float startProcess = 0.33f;
    private float stopProcess = 0.69f;
    public void EnterState(GameStateManager manager) {
        manager.currentStateGame = ApplicationVariable.StateGame.Losing;
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.lose_sound);
        reviveGameObj.SetActive(false);
        SettingMapLose();
        uiGenerate.ShowAndHiddenGameObject();
        uiManager.ProcessEndGame();
        gameOverReal.SetActive(true);
        setting.ShowAndHiddenGameObject();
        smtHidden1.SetActive(false);
        smtHidden2.SetActive(false);
    }

    private void SettingMapLose() {
        int current_map = PlayerPrefs.GetInt(ApplicationVariable.CURRENT_MAP, 1);
        float rank = 0;
        if (current_map < 3) {
            beginLevel.text = "ZONE: " + current_map;
            nextLevel.text = "ZONE: " + (current_map + 1);
            BeginLevelIcon.sprite = sprite_Icon[current_map - 1];
            NextLevelIcon.sprite = sprite_Icon[current_map];
            rank = GamePlayController.Instance.enemy_remain + 1;
            if (rank > PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME)) {
                sliderProcess.value = startProcess;
            }
            else {
                float sliderValue = startProcess + (1 - (rank / PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME))) * (stopProcess - startProcess);
                sliderProcess.value = sliderValue;
            }
        }
        else {
            beginLevel.text = "ZONE: 3";
            nextLevel.text = "ZONE ?";
            BeginLevelIcon.sprite = sprite_Icon[current_map - 1];
            NextLevelIcon.sprite = sprite_Icon[current_map];
            rank = GamePlayController.Instance.enemy_remain + 1;
            float sliderValue = startProcess + (1 - (rank / PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME))) * (stopProcess - startProcess);
            sliderProcess.value = sliderValue;
        }
        if (PlayerPrefs.HasKey(ApplicationVariable.MAX_RECORD_MAP + current_map.ToString())) {
            int temp = PlayerPrefs.GetInt(ApplicationVariable.MAX_RECORD_MAP + current_map.ToString());
            if (temp > rank) {
                PlayerPrefs.SetInt(ApplicationVariable.MAX_RECORD_MAP + current_map.ToString(), (int)rank);
            }
        }
        else {
            PlayerPrefs.SetInt(ApplicationVariable.MAX_RECORD_MAP + current_map.ToString(), (int)rank);
        }
    }

    public void ExitState(GameStateManager manager) {
    }

    public void UpdateState(GameStateManager manager) {
    }
}
