using TMPro;
using Unity.AI.Navigation;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayController : Singleton<GamePlayController>
{
    [Header("----------------Manager Game---------------")]
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CoinManager coinManager;
    [HideInInspector] public float num_coin = 0;
    [SerializeField] private TextMeshProUGUI earnCoinwin;

    [Header("--------------------Spawn Enemy Game---------------------")]
    [SerializeField] private float enemy_spawn_pertime;
    [SerializeField] public float enemy_remain; //Tong quai can danh hien tai ke ca chua spawn
    [HideInInspector] public string name_enemy_win;
    private float rangeSpawn = 30f;
    private float num_check_spawn = 30f;
    private float begin_range = 7f;
    private float normal_range = 25f;
    private float max_enemy_spawn_in_map = 10f;
    public bool islose = false;
    public PlayerController playerController;
    public LevelManager levelManager;
    private float enemy_not_spawn_num;
    [Header("---------------------UI-----------------------")]
    public TextMeshProUGUI dayZombieMode;
    private bool firstTime = true;
    private string enemy_text = "ALIVE: ";
    private Vector3 randomPoint;
    private UIGeneratePress ui_generate;
    [Header("--------------Next Level Map-------------")]
    //-------------------GiftBox-----------------------
    public GameObject giftBox;
    public bool isGiftBox = false;
    public bool isHoldGiftBox = false;
    //----------------Map Level------------------------
    public GameObject[] map_level;
    public NavMeshSurface navMeshSurface;
    public Sprite[] sprite_level;
    public Image sprite_main_level;

    public TextMeshProUGUI numEnemy;
    public TextMeshProUGUI currentZoneInPlayBtn;
    public Slider slider_star;
    private void Start() {
        SetupMapLevel();
        ui_generate = GetComponent<UIGeneratePress>();
        if (PlayerPrefs.HasKey(ApplicationVariable.LEVEL_GAME)) {
            enemy_remain = PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME);
        }
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
        if (isGiftBox)
            InvokeRepeating(nameof(SpawnGift), 0, 5f);
    }



    private void Update() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) {
            return;
        }
        if (playerController == null && !uiManager.iswin && !GameStateManager.Instance.CheckInStatusGame()) {
            uiManager.CheckLoseGame();
        }

        if (enemy_remain <= 0 && !GameStateManager.Instance.CheckInStatusGame()) {
            GameStateManager.Instance.ChangeState(GameStateManager.Instance.winGameState);
        }
        if (playerController == null && enemy_remain == 1) {
            EnemyAI enemy_win = FindFirstObjectByType<EnemyAI>();
            if (enemy_win.iswinning == false) {
                enemy_win.animator.SetBool(ApplicationVariable.WIN_STATE, true);
                enemy_win.iswinning = true;
            }
        }
        if (name_enemy_win != "") {
            uiManager.nameEnemyWin = name_enemy_win;
        }

        num_coin = levelManager.current_level;
        playerController.ProcessPlayer();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ApplicationVariable.ENEMY_TAG);
        foreach (var enemyObject in enemies) {
            if (enemyObject != null) {
                EnemyAI ai = enemyObject.GetComponent<EnemyAI>();
                if (ai != null) {
                    ai.ProcessEnemy();
                }
            }
        }
        GameObject[] CheckStatus = GameObject.FindGameObjectsWithTag("CheckStatus");
        foreach (var checkStatus in CheckStatus) {
            if (checkStatus != null) {
                checkStatus.GetComponent<CheckStatusIndicator>().CheckStatus();
                if (checkStatus.GetComponent<OffScreenIndicator>())
                    checkStatus.GetComponent<OffScreenIndicator>().CheckOffScreen();
            }
        }
    }

    #region Revive and manage Player
    public void Revive() {
        playerController.RevivePlayer();
    }
    public void DestroyPlayer() {
        Destroy(playerController.gameObject);
    }
    public void LoopSpawn() {
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    private string quickAddText(float num) {
        return enemy_text + num.ToString();
    }
    public void MinusEnemy() {
        enemy_remain--;
        enemy_alive.text = quickAddText(enemy_remain);
    }
    #endregion

    #region Spawn

    private void SpawnGift() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby && firstTime == true) {
            SpawnEnemyFirstTime();
        }
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) { return; }
        if (giftBox.activeSelf == false && isHoldGiftBox == false) {
            giftBox.SetActive(true);
            Vector3 temp_pos = GetRandomNavMeshPositionPlayer(30f);
            temp_pos.y += 5f;
            giftBox.transform.position = temp_pos;
        }
    }
    private void SpawnEnemy() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby && firstTime == true) {
            SpawnEnemyFirstTime();
        }
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) { return; }
        if (enemy_not_spawn_num == 0) {
            return;
        }
        if (enemy_remain <= 0) { enemy_alive.text = quickAddText(0); return; }
        int currentEnemyInMap = GameObject.FindGameObjectsWithTag(ApplicationVariable.ENEMY_TAG).Length;
        if (currentEnemyInMap < max_enemy_spawn_in_map) {
            if (enemy_not_spawn_num > enemy_spawn_pertime) {
                SpawnEnemyPerTime(enemy_spawn_pertime);
                enemy_not_spawn_num -= enemy_spawn_pertime;
            }
            else {
                SpawnEnemyPerTime(enemy_not_spawn_num);
                enemy_not_spawn_num = 0;
            }
        }
    }
    private void SpawnEnemyFirstTime() {
        firstTime = false;
        rangeSpawn = begin_range;
        SpawnEnemyPerTime(2);
        enemy_not_spawn_num -= 2;
        rangeSpawn = normal_range;
        CancelInvoke(nameof(SpawnEnemy));
    }
    /*    private void SpawnEnemyPerTime(float a) {
            for (int i = 0; i < a; i++) {
                int random_enemy = Random.Range(0, enemy.Length);
                Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
            }
        }*/
    private void SpawnEnemyPerTime(float a) {
        for (int i = 0; i < a; i++) {
            //int random_enemy = Random.Range(0, enemy.Length);
            //GameObject spawned = Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
            GameObject enemy_spawn = ObjectPooler.Instance.GetGameObject();
            enemy_spawn.transform.position = GetRandomNavMeshPosition(transform.position, rangeSpawn);

            /*            NavMeshAgent agent = spawned.GetComponent<NavMeshAgent>();
                        if (agent != null && NavMesh.SamplePosition(spawned.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                            agent.Warp(hit.position);
                        }*/
            NavMeshAgent agent = enemy_spawn.GetComponent<NavMeshAgent>();
            if (agent != null && NavMesh.SamplePosition(enemy_spawn.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                agent.Warp(hit.position);
            }
        }
    }

    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius) {
        for (int i = 0; i < num_check_spawn; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {
                randomPoint = hit.position;
                if ((playerController && Vector3.Distance(randomPoint, playerController.gameObject.transform.position) < 7f) || !CheckReviveCondition(randomPoint, 7f)) {
                    continue;
                }
                return hit.position;
            }
        }
        return origin;
    }
    private bool CheckReviveCondition(Vector3 posCheck, float distance) {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag(ApplicationVariable.ENEMY_TAG);
        foreach (var item in enemy) {
            float dis = Vector3.Distance(item.transform.position, posCheck);
            if (dis < distance) {
                return false;
            }
        }
        return true;
    }
    public Vector3 GetRandomNavMeshPositionPlayer(float radius) {
        for (int i = 0; i < num_check_spawn; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {

                randomPoint = hit.position;
                if (!CheckReviveCondition(randomPoint, 10f)) {
                    continue;
                }
                return hit.position;
            }
        }
        return transform.position;
    }
    #endregion

    #region Misc
    public void SettingEnemyMaxCount(float num) {
        enemy_remain = num;
        enemy_not_spawn_num = enemy_remain;
    }
    public void Earnx3Gold() {
        coinManager.AddingCoinXn(2);
    }
    public void RestartScene() {
        GamePlayController.Instance.gameObject.GetComponentInChildren<CoinManager>().AddingCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void CheckRecordPlayer() {
        int temp = PlayerPrefs.GetInt(ApplicationVariable.MAX_RECORD_GAME, 0);
        if (temp < num_coin) {
            PlayerPrefs.SetInt(ApplicationVariable.MAX_RECORD_GAME, (int)num_coin);
        }
    }
    private void SetupMapLevel() {
        dayZombieMode.text = PlayerPrefs.GetInt(ApplicationVariable.DAY_ZOMBIE_MODE, 1).ToString();
        if (!PlayerPrefs.HasKey(ApplicationVariable.CURRENT_MAP)) {
            PlayerPrefs.SetInt(ApplicationVariable.CURRENT_MAP, 1);
        }
        int current_map = PlayerPrefs.GetInt(ApplicationVariable.CURRENT_MAP);
        switch (current_map) {
            case 1:
                PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, ApplicationVariable.NUM_ENEMY_MAP_1);
                break;
            case 2:
                PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, ApplicationVariable.NUM_ENEMY_MAP_2);
                break;
            case 3:
                PlayerPrefs.SetFloat(ApplicationVariable.LEVEL_GAME, ApplicationVariable.NUM_ENEMY_MAP_3);
                break;
        }
        numEnemy.text = PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME).ToString();
        int best_current_map = PlayerPrefs.GetInt("MaxRecordMap" + current_map.ToString(), (int)PlayerPrefs.GetFloat(ApplicationVariable.LEVEL_GAME));
        currentZoneInPlayBtn.text = "ZONE:" + current_map.ToString() + "  -  " + "BEST:#" + best_current_map.ToString();
        if (current_map > 1) {
            isGiftBox = true;
        }
        map_level[current_map - 1].SetActive(true);
        for (int i = 0; i < map_level.Length; i++) {
            if (i != (current_map - 1)) {
                Destroy(map_level[i]);
            }
        }
        navMeshSurface.BuildNavMesh();
        int max_record = PlayerPrefs.GetInt(ApplicationVariable.MAX_RECORD_GAME, 0);
        if (max_record < 18) {
            sprite_main_level.sprite = sprite_level[0];
            slider_star.value = (float)max_record / 17;
        }
        else if (max_record >= 18 && max_record < 40) {
            sprite_main_level.sprite = sprite_level[1];
            slider_star.value = (float)max_record / 39;
        }
        else if (max_record >= 40) {
            sprite_main_level.sprite = sprite_level[2];
            slider_star.value = (float)max_record / 100;
        }
    }
    #endregion
}
