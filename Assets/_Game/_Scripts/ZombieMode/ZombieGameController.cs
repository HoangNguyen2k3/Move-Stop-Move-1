using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class ZombieGameController : Singleton<ZombieGameController>
{
    [Header("-------------Manage State Game-------------")]
    [SerializeField] private TextMeshProUGUI enemy_alive;
    [SerializeField] private GameObject winningGame;
    //[SerializeField] private List<GameObject> enemy;
    [SerializeField] private GameObject loseGame;
    [SerializeField] private GameObject loseReal;
    [SerializeField] private float enemy_spawn_pertime;
    [SerializeField] public float enemy_remain;
    private float enemy_not_spawn_num;
    [HideInInspector] public string name_enemy_win;
    public bool iswinning = false;
    public bool islose = false;
    private bool temp = false;
    private bool startingGame = false;
    private Vector3 randomPoint;
    private float rangeSpawn = 25f;

    [Header("-----------Manage Player-------------")]
    public PlayerZombie playerController;
    public CircleRange circleRange;

    [Header("---------------ZombieMode---------------")]
    [SerializeField] private CinemachineCamera camWinning;
    public bool currentInLobbyZombie = true;
    [Header("-------------Coin Manager-----------------")]
    [HideInInspector] public float num_coin = 0;
    [SerializeField] private TextMeshProUGUI coin_win;
    [SerializeField] private TextMeshProUGUI coin_lose;
    [SerializeField] private TextMeshProUGUI coin_win_x3;
    [SerializeField] private TextMeshProUGUI coin_lose_x3;
    private CoinManager coinManager;
    public bool x2GoldAbilities = false;
    [Header("--------------------Indicator---------------------")]
    public GameObject floatingTextPlayer;
    public GameObject CanvasIndicator;
    [Header("---------------Boss Map--------------------")]
    [SerializeField] private bool isMapBoss = false;
    public GameObject boss;
    private bool isSpawnBoss = false;
    private int maxEnemyType;

    [Header("----------------Zombie Current Day-------------------")]
    [SerializeField] private SaveDayZombieMode saveDayZombieMode;
    [SerializeField] private TextMeshProUGUI day_zombie;
    [Header("---------------Ads---------------------")]
    [SerializeField] private GameObject adsManager;
    [SerializeField] private RandomAbilities randomAbilities;
    [SerializeField] private UIGeneratePress generatePress;
    [SerializeField] private UIGeneratePress reviveAds;
    public bool currentInRevive = false;

    private void Start() {
        saveDayZombieMode.current_day = PlayerPrefs.GetInt(ApplicationVariable.DAY_ZOMBIE_MODE, 1);
        day_zombie.text = "DAY " + saveDayZombieMode.current_day.ToString();
        if (saveDayZombieMode.current_day == 5 || saveDayZombieMode.current_day == 10) {
            isMapBoss = true;
        }
        enemy_remain = saveDayZombieMode.num_enemy_day[saveDayZombieMode.current_day - 1];
        coinManager = GetComponentInChildren<CoinManager>();
        enemy_not_spawn_num = enemy_remain;
        enemy_alive.text = quickAddText(enemy_remain);
        SpawnEnemy();
        int temp = Random.Range(1, 10);
        if (temp == 1) {
            adsManager.GetComponent<Interstitial>().LoadInterstitial();
        }
    }
    private void Update() {
        if (!startingGame && playerController.gameObject.activeSelf == true) {
            startingGame = true;
            InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
        }
        if (islose && !temp) {

            currentInLobbyZombie = true;
            CanvasIndicator.SetActive(false);
            SetUpCoin();
            temp = true;
            loseReal.SetActive(true);
            CancelInvoke(nameof(SpawnEnemy));
        }
        if (islose) { return; }
        if (enemy_remain <= 0 && !iswinning) {
            currentInLobbyZombie = true;
            CanvasIndicator.SetActive(false);
            floatingTextPlayer.SetActive(false);
            iswinning = true;
            enemy_alive.text = quickAddText(0);
            SetUpCoin();
            winningGame.SetActive(true);
            camWinning.Priority = 10;
            if (saveDayZombieMode.current_day < 10) {
                saveDayZombieMode.current_day += 1;
                PlayerPrefs.SetInt(ApplicationVariable.DAY_ZOMBIE_MODE, saveDayZombieMode.current_day);
            }
            playerController.animator.SetBool(ApplicationVariable.WIN_STATE, true);
            playerController.isWinning = true;
        }
        playerController.PlayerZombieController();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ApplicationVariable.ENEMY_TAG);
        foreach (var enemyObject in enemies) {
            if (enemyObject != null) {
                ZombieEnemy ai = enemyObject.GetComponent<ZombieEnemy>();
                if (ai != null) {
                    ai.ZombieProcess();
                }
            }
        }
        circleRange.ControlPos();
    }
    private void FixedUpdate() {
        if (playerController && playerController.isActiveAndEnabled)
            playerController?.MovementPlayer();
    }

    #region Shop In Zombie Mode
    public void ChangeInShop() {
        currentInLobbyZombie = false;
    }
    public void SetUpCoin() {
        if (x2GoldAbilities) { num_coin *= 2; }
        coin_win.text = num_coin.ToString();
        coin_lose.text = num_coin.ToString();
        coin_win_x3.text = (num_coin * 3).ToString();
        coin_lose_x3.text = (num_coin * 3).ToString();
    }
    public void EarnCoin() {
        coinManager.AddingCoin();
    }
    public void EarnCoinX3() {
        coinManager.AddingCoinXn(3);
    }
    private string quickAddText(float num) {
        return num.ToString();
    }
    #endregion

    #region Spawn Enemy
    public void MinusEnemy() {
        enemy_remain--;
        enemy_alive.text = quickAddText(enemy_remain);
    }
    private void SpawnEnemy() {
        if (enemy_not_spawn_num == 0) {
            return;
        }
        if (enemy_remain <= 0) { enemy_alive.text = quickAddText(0); return; }
        if (enemy_not_spawn_num > enemy_spawn_pertime) {
            SpawnEnemyPerTime(enemy_spawn_pertime);
            enemy_not_spawn_num -= enemy_spawn_pertime;
        }
        else {
            SpawnEnemyPerTime(enemy_not_spawn_num);
            enemy_not_spawn_num = 0;
        }
    }
    private void SpawnEnemyPerTime(float a) {
        for (int i = 0; i < a; i++) {
            if (isMapBoss) {
                /*            for (int i = 0; i < a; i++) {
                                int random_enemy = Random.Range(0, enemy.Count);
                                Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
                                if (enemy[random_enemy].GetComponent<EnemiesHealth>().isBoss) {
                                        enemy.Remove(enemy[random_enemy]);
                                }
                            }*/
                if (!isSpawnBoss) {
                    int random_boss = Random.Range(0, 3);
                    if (random_boss == 0) {
                        Instantiate(boss, GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
                        isSpawnBoss = true;
                        continue;
                    }
                    GameObject enemy_spawn = ObjectPooler.Instance.GetGameObject();
                    enemy_spawn.transform.position = GetRandomNavMeshPosition(transform.position, rangeSpawn);
                }
                else {
                    GameObject enemy_spawn = ObjectPooler.Instance.GetGameObject();
                    enemy_spawn.transform.position = GetRandomNavMeshPosition(transform.position, rangeSpawn);
                }
            }
            else {
                //int random_enemy = Random.Range(0, enemy.Count - 1);
                //Instantiate(enemy[random_enemy], GetRandomNavMeshPosition(transform.position, rangeSpawn), Quaternion.identity);
                GameObject enemy_spawn = ObjectPooler.Instance.GetGameObject();
                enemy_spawn.transform.position = GetRandomNavMeshPosition(transform.position, rangeSpawn);
            }
        }
    }
    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {
                randomPoint = hit.position;
                if (Vector3.Distance(randomPoint, playerController.gameObject.transform.position) < 10f) {
                    if (randomPoint.y > 0f) { continue; }
                    continue;
                }
                return hit.position;
            }
        }
        return origin;
    }
    public void StopSpawn() {
        CancelInvoke(nameof(SpawnEnemy));
    }
    public void ContinueSpawn() {
        InvokeRepeating(nameof(SpawnEnemy), 0, 2.5f);
    }
    #endregion

    #region Revive Player
    public void RevivePlayer() {
        playerController.RevivePlayer();
    }
    public void CanRevive() {
        loseGame.SetActive(true);
    }
    public void DeadPlayer() {
        playerController.DeadPlayer();
    }
    private bool CheckReviveCondition(Vector3 posCheck, float distance) {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in enemy) {
            float dis = Vector3.Distance(item.transform.position, posCheck);
            if (dis < distance) {
                return false;
            }
        }
        return true;
    }
    public Vector3 GetRandomPositionRevivePlayer(float radius) {
        Vector3 origin = transform.position;
        for (int i = 0; i < 30; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {
                randomPoint = hit.position;
                if (CheckReviveCondition(randomPoint, 12f)) {
                    if (randomPoint.y > -1f) { continue; }
                    return hit.position;
                }

            }
        }
        return origin;
    }
    #endregion

    #region Setting Abilities and Cloth
    public void SettingEnemyMaxCount(float num) {
        enemy_remain = num;
        enemy_not_spawn_num = enemy_remain;
    }
    public void ChooseAbilities() {
        randomAbilities.ChooseAbilities();
        generatePress.ShowAndHiddenGameObject();
    }

    public void ShowAndHidden() {
        reviveAds.ShowAndHiddenGameObject();
    }
    public void CheckAdsCloth() {
        if (PlayerPrefs.HasKey(ApplicationVariable.TYPE_ADD_ONE_TIME) && !PlayerPrefs.HasKey(ApplicationVariable.USE_ONE_TIME_IN_ZOMBIE)) {
            int type = int.Parse(PlayerPrefs.GetString(ApplicationVariable.TYPE_ADD_ONE_TIME));
            PlayerPrefs.SetInt(ApplicationVariable.USE_ONE_TIME_IN_ZOMBIE, type);
            playerController.characterPlayer.skinClother[type] = null;
        }
    }
    #endregion
}
