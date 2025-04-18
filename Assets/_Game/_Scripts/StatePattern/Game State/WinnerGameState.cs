using TMPro;
using Unity.Cinemachine;
using UnityEngine;

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
    public void EnterState(GameStateManager manager) {
        if (SoundManager.Instance) {
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.win_sound);
        }
        manager.currentStateGame = ApplicationVariable.StateGame.Winning;
        uiManager.ProcessEndGame();
        revive_gameObj.SetActive(false);
        winningCam.Priority = 10;
        winningGame.SetActive(true);
        earnCoinwin.text = GamePlayController.Instance.num_coin.ToString();
        playerController.animator.SetBool(ApplicationVariable.WIN_STATE, true);
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
