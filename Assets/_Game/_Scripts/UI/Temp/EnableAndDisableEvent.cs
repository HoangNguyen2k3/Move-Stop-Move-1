using UnityEngine;

public class EnableAndDisableEvent : MonoBehaviour
{
    [SerializeField] private bool isZombieMap;
    public bool inSetting = false;

    private void OnEnable() {
        if (inSetting) {
            EnterSetting();
            return;
        }
        EnterRevive();
    }
    private void OnDisable() {
        if (inSetting) {
            OutSetting();
            return;
        }
        OutRevive();
    }
    private void EnterRevive() {
        if (isZombieMap) {
            ZombieGameController.Instance.currentInLobbyZombie = true;
            ZombieGameController.Instance.currentInRevive = true;
        }
        else {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.reviveState);
        }
    }
    private void OutRevive() {
        if (isZombieMap) {
            ZombieGameController.Instance.currentInLobbyZombie = false;
            ZombieGameController.Instance.currentInRevive = false;
        }
        else {
            /*            GameStateManager.Instance.inRevive = false;
                        GameStateManager.Instance.endRevive = true;*/
            //GameStateManager.Instance.ChangeState(GameStateManager.Instance.reviveState);
            if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InRevive) {
                GameStateManager.Instance.currentStateGame = ApplicationVariable.StateGame.InPlaying;
            }
        }
    }

    private void EnterSetting() {
        GameStateManager.Instance.currentStateGame = ApplicationVariable.StateGame.Setting;
    }

    private void OutSetting() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.Setting) {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.playingState);
        }
    }
}
