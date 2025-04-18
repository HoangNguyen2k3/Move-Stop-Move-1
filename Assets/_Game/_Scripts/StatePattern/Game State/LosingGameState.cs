using UnityEngine;

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
    public void EnterState(GameStateManager manager) {
        manager.currentStateGame = ApplicationVariable.StateGame.Losing;
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.lose_sound);
        reviveGameObj.SetActive(false);
        gameOverReal.SetActive(true);
        uiGenerate.ShowAndHiddenGameObject();
        //GameStateManager.Instance.is_losing = true;
        uiManager.ProcessEndGame();
        setting.ShowAndHiddenGameObject();
        smtHidden1.SetActive(false);
        smtHidden2.SetActive(false);
    }

    public void ExitState(GameStateManager manager) {
    }

    public void UpdateState(GameStateManager manager) {
    }
}
