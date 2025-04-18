using UnityEngine;

public class LobbyGameState : MonoBehaviour, IGameState
{
    public void EnterState(GameStateManager manager) {
        //manager.inLobby = true;
        manager.currentStateGame = ApplicationVariable.StateGame.InLobby;
    }

    public void ExitState(GameStateManager manager) {
        //manager.inLobby = false;
    }

    public void UpdateState(GameStateManager manager) {
    }
}
