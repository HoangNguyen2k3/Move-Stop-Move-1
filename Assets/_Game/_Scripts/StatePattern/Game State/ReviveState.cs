using UnityEngine;

public class ReviveState : MonoBehaviour, IGameState
{
    public void EnterState(GameStateManager manager) {
        //GameStateManager.Instance.inRevive = true;
        manager.currentStateGame = ApplicationVariable.StateGame.InRevive;
    }

    public void ExitState(GameStateManager manager) {
        //GameStateManager.Instance.inRevive = false;
    }

    public void UpdateState(GameStateManager manager) {
    }
}
