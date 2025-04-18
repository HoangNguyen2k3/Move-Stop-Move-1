using UnityEngine;

public class PlayingState : MonoBehaviour, IGameState
{
    public void EnterState(GameStateManager manager) {
        manager.currentStateGame = ApplicationVariable.StateGame.InPlaying;
    }

    public void ExitState(GameStateManager manager) {
    }

    public void UpdateState(GameStateManager manager) {
    }
}
