using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class WinnerGameState : MonoBehaviour, IGameState
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CinemachineCamera winningCam;
    [SerializeField] private TextMeshProUGUI earnCoinwin;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIGeneratePress ui_generate;
    [SerializeField] private GameObject revive_gameObj;
    [SerializeField] private UIGeneratePress setting;
    [SerializeField] private GameObject smtHidden1;
    [SerializeField] private GameObject smtHidden2;
    [Header("---------------Level Map Game----------------")]
    public Sprite[] sprite_Icon;
    public Image BeginLevelIcon;
    public Image NextLevelIcon;

    public TextMeshProUGUI textCongratulation;
    public TextMeshProUGUI beginLevel;
    public TextMeshProUGUI nextLevel;
    public void EnterState(GameStateManager manager) {
        if (SoundManager.Instance) {
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.win_sound);
        }
        manager.currentStateGame = ApplicationVariable.StateGame.Winning;
        uiManager.ProcessEndGame();
        revive_gameObj.SetActive(false);
        winningCam.Priority = 10;
        earnCoinwin.text = GamePlayController.Instance.num_coin.ToString();

        GamePlayController.Instance.CheckRecordPlayer();
        int current_map = PlayerPrefs.GetInt(ApplicationVariable.CURRENT_MAP);
        textCongratulation.text = "Congratulation, you have just unlocked Zone " + current_map + "!";
        if (current_map < 3) {
            beginLevel.text = "ZONE: " + current_map;
            nextLevel.text = "ZONE: " + (current_map + 1);
            BeginLevelIcon.sprite = sprite_Icon[current_map - 1];
            NextLevelIcon.sprite = sprite_Icon[current_map];
            current_map++;
            PlayerPrefs.SetInt(ApplicationVariable.CURRENT_MAP, current_map);
        }
        /*        float rank = GamePlayController.Instance.enemy_remain + 1;
                if (PlayerPrefs.HasKey("MaxRecordMap" + current_map.ToString())) {
                    int temp = PlayerPrefs.GetInt("MaxRecordMap" + current_map.ToString());
                    if (temp > rank) {
                        PlayerPrefs.SetInt("MaxRecordMap" + current_map.ToString(), (int)rank);
                    }
                }
                else {
                    PlayerPrefs.SetInt("MaxRecordMap" + current_map.ToString(), (int)rank);
                }*/
        playerController.animator.SetBool(ApplicationVariable.WIN_STATE, true);
        winningGame.SetActive(true);
        ui_generate.ShowAndHiddenGameObject();
        playerController.isWinning = true;
        setting.ShowAndHiddenGameObject();
        smtHidden1.SetActive(false);
        smtHidden2.SetActive(false);
    }
    public void ExitState(GameStateManager manager) {
    }
    public void UpdateState(GameStateManager manager) {
    }
}
