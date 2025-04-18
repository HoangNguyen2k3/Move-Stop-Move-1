using UnityEngine;

public class CheckStatusGame : MonoBehaviour
{
    /*    private LobbyManager lobby;*/
    [SerializeField] private GameObject child;
    /*    private void Start()
        {
            lobby = FindFirstObjectByType<LobbyManager>();
        }*/
    void Update() {
        // if (GameManager.Instance.iswinning || GameManager.Instance.islose)
        if (GameStateManager.Instance.CheckInStatusGame()) {
            Destroy(gameObject);
        }
        //if (lobby.currentinLobby && child.activeSelf == true)
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby && child.activeSelf == true) {
            child.SetActive(false);
        }
        else if (GameStateManager.Instance.currentStateGame != ApplicationVariable.StateGame.InLobby && child.activeSelf == false) {
            child.SetActive(true);
        }

    }
}
