using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string nameEnemyWin;
    private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameOverReal;
    [SerializeField] private TextMeshProUGUI[] earnCoinlose;
    [SerializeField] private TextMeshProUGUI[] textNameEnemy;
    [SerializeField] private TextMeshProUGUI[] num_rank_lose_txt;
    [SerializeField] private TextMeshProUGUI[] name_player_txt;
    [SerializeField] private UIGeneratePress setting;
    public bool iswin = false;
    private void Start() {
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    public async void CheckLoseGame() {
        await Task.Delay(5);
        if (player == null && !iswin && !GameStateManager.Instance.CheckInStatusGame()) {
            iswin = true;
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.loseGameState);
        }
    }

    public void SetReviveActive() {
        if (!GameStateManager.Instance.CheckInStatusGame()) {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.reviveState);
            setting.ShowAndHiddenGameObject();
            gameOver.SetActive(true);
        }
    }
    public void ProcessEndGame() {
        for (int i = 0; i < textNameEnemy.Length; i++) {
            textNameEnemy[i].text = nameEnemyWin;
        }

        for (int i = 0; i < earnCoinlose.Length; i++) {
            earnCoinlose[i].text = GamePlayController.Instance.num_coin.ToString();
        }
        for (int i = 0; i < num_rank_lose_txt.Length; i++) {
            float temp = GamePlayController.Instance.enemy_remain;
            temp += 1;
            num_rank_lose_txt[i].text = "#" + temp.ToString();
        }
        for (int i = 0; i < name_player_txt.Length; i++) {
            name_player_txt[i].text = PlayerPrefs.GetString("NamePlayer", "YOU");
        }
        GamePlayController.Instance.CheckRecordPlayer();
    }

    public void RestartScene() {
        GamePlayController.Instance.gameObject.GetComponentInChildren<CoinManager>().AddingCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
