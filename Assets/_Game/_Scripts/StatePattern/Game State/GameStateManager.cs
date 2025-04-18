using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameStateManager : Singleton<GameStateManager>
{
    private IGameState currentGameState;
    [Header("-----------Normal Game---------------")]
    public LobbyGameState lobbyState;
    public PlayingState playingState;
    public LosingGameState loseGameState;
    public WinnerGameState winGameState;
    public ReviveState reviveState;
    public ApplicationVariable.StateGame currentStateGame;
    /*    public bool is_winning = false;
        public bool is_losing = false;
        public bool inLobby = false;
        public bool inPlaying = false;
        public bool inRevive = false;
        public bool endRevive = false;*/

    private void Start() {
        if (lobbyState != null) {
            currentGameState = lobbyState;
            lobbyState.EnterState(this);
        }
    }
    public void ChangeState(IGameState newState) {
        if (currentGameState != null) {
            currentGameState.ExitState(this);
        }
        currentGameState = newState;
        if (currentGameState != null) {
            currentGameState.EnterState(this);
        }
    }
    #region Chua dung den
    /*    private void Update() {
            if (currentGameState != null) {
                currentGameState.UpdateState(this);
            }
        }*/
    #endregion
    public bool CheckInStatusGame() {
        /*        if (is_losing == true || is_winning == true) {
                    return true;
                }
                return false;*/
        if (currentStateGame == ApplicationVariable.StateGame.Winning || currentStateGame == ApplicationVariable.StateGame.Losing) {
            return true;
        }
        return false;
    }
    public bool CheckFalseIndicator() {
        if (currentStateGame == ApplicationVariable.StateGame.Winning || currentStateGame == ApplicationVariable.StateGame.Losing ||
           currentStateGame == ApplicationVariable.StateGame.InRevive || currentStateGame == ApplicationVariable.StateGame.Setting ||
           currentStateGame == ApplicationVariable.StateGame.InLobby) {
            return true;
        }
        return false;
    }
}
