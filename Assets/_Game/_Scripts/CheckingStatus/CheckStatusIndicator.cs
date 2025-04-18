using UnityEngine;

public class CheckStatusIndicator : MonoBehaviour
{
    [SerializeField] private GameObject child;

    public bool inZombieMode = false;
    public bool isNamePlayer = false;
    public void CheckStatus() {
        if (!inZombieMode) {
            if (GameStateManager.Instance.CheckInStatusGame()) {
                Destroy(gameObject);
            }
            if (child != null && GameStateManager.Instance.CheckFalseIndicator() && child.activeSelf == true) {
                child.SetActive(false);
            }
        }
        else {
            if (ZombieGameController.Instance.currentInLobbyZombie) {
                child.SetActive(false);
            }
            else {
                child.SetActive(true);
            }
        }
    }
}
