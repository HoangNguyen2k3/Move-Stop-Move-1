using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform targetIndicator;

    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderInterval = 3f;

    public float attackRange = 10f;
    [SerializeField] private float timeCoolDown = 1.5f;
    [SerializeField] public GameObject weaponThrow;
    [SerializeField] private Transform posStartThrow;
    [SerializeField] private GameObject weapon;

    public bool iswinning = false;
    public GameObject indicatorPrefab;
    public Transform canvasTransform;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    private bool isAttacking = false;
    private Transform target;
    private EnemiesHealth health;
    private Vector3 randomPoint;
    private List<Transform> enemiesInRange = new List<Transform>();
    private GameObject indicator;
    private LevelManager levelManager;
    private bool dontMove = false;
    private bool isFirst = true;
    [Header("-------------Ultimate Weapon----------------")]
    public bool active_ultimate = true;
    private void Awake() {
        canvasTransform = GameObject.FindGameObjectWithTag("CanvasOverlay").transform;
        health = GetComponentInChildren<EnemiesHealth>();
        levelManager = GetComponent<LevelManager>();

    }
    private void Start() {
        //    InvokeRepeating(nameof(Wander), 0f, wanderInterval);
        Indicator();
        //Ultimate();
    }

    private void Indicator() {
        indicator = Instantiate(indicatorPrefab, canvasTransform);
        indicator.GetComponent<IndicatorObj>().arrow.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<IndicatorObj>().backGround.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<IndicatorObj>().numEnemyLevel.text = levelManager.current_level.ToString();
        //indicator.GetComponent<OffScreenIndicator>().target = enemy.transform;
        indicator.GetComponent<OffScreenIndicator>().enemy = gameObject;
        indicator.GetComponent<OffScreenIndicator>().target = posStartThrow;
        indicator.GetComponent<OffScreenIndicator>().mainCamera = Camera.main;
    }
    public void ProcessEnemy() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) {
            return;
        }
        if (GameStateManager.Instance.currentStateGame != ApplicationVariable.StateGame.InLobby && isFirst) {
            isFirst = false;
            InvokeRepeating(nameof(Wander), 0f, wanderInterval);
        }
        if (indicator)
            indicator.GetComponent<IndicatorObj>().numEnemyLevel.text = levelManager.current_level.ToString();
        if (!health.isAlive || iswinning) {
            enemy.isStopped = true;
            if (!dontMove) {
                dontMove = true;
                CancelInvoke(nameof(Wander));

            }
            return;
        }
        if (randomPoint == transform.position) {
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
        }
        if (target == null || target.gameObject.activeSelf == false) FindNearestTarget();
        if (target != null && target.root.gameObject.activeSelf == false) FindNearestTarget();
        if (target != null && target.gameObject.activeSelf && Vector3.Distance(transform.position, target.position) <= attackRange) {
            StopAndAttack();
            CancelInvoke(nameof(Wander));

        }
        else {
            if (!IsInvoking(nameof(Wander)) && enemy.isStopped == false) {
                InvokeRepeating(nameof(Wander), 0f, wanderInterval);
            }
        }
    }
    public void Ultimate() {
        active_ultimate = true;
        attackRange += 2.5f;
    }
    private void FindNearestTarget() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in colliders) {
            if ((col.CompareTag(ApplicationVariable.PLAYER_TAG) || col.CompareTag(ApplicationVariable.ENEMY_TAG)) && col.transform.gameObject != transform.GetChild(1).gameObject) {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist) {
                    minDist = dist;
                    nearest = col.transform;
                }
            }
        }

        target = nearest;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
    private void StopAndAttack() {
        enemy.SetDestination(transform.position);
        if (!isAttacking) {
            enemy.isStopped = true;
            StartCoroutine(Attack());
        }
        transform.LookAt(target);
    }

    private IEnumerator Attack() {
        isAttacking = true;
        weapon.SetActive(false);
        if (active_ultimate) {
            animator.SetBool(ApplicationVariable.ULTI, true);
        }
        animator.SetBool("IsAttack", true);

        yield return new WaitForSeconds(0.1f);
        GameObject throwWeapon = Instantiate(weaponThrow, posStartThrow.position, Quaternion.identity);
        throwWeapon.GetComponent<ThrowWeapon>().who_throw_obj = transform.GetChild(1).gameObject;
        throwWeapon.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeapon.GetComponent<ThrowWeapon>().who_throw = ApplicationVariable.ENEMY_TAG;
        if (active_ultimate && target != null) {
            Vector3 target_temp = target.transform.position;
            throwWeapon.GetComponent<ThrowWeapon>().isUltimate = true;
            throwWeapon.GetComponent<ThrowWeapon>().ChangeMaxScale(throwWeapon.transform.localScale.x * 4, 5);
            Vector3 currentPosAttack = posStartThrow.position;
            currentPosAttack.y = transform.position.y + 1.2f;
            target_temp.y = transform.position.y + 1.2f;
            Vector3 dir = (target_temp - currentPosAttack).normalized;
            throwWeapon.GetComponent<ThrowWeapon>().dir_ulti = dir;
            throwWeapon.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;
            active_ultimate = false;
            attackRange -= 2.5f;
            GamePlayController.Instance.isHoldGiftBox = false;
        }
        else {
            if (target)
                throwWeapon.GetComponent<ThrowWeapon>().target = target.GetComponentInChildren<TargetPos>().transform.position;
        }
        yield return new WaitForSeconds(0.55f);
        enemy.isStopped = false;
        weapon.SetActive(true);
        animator.SetBool("IsAttack", false);
        if (animator.GetBool(ApplicationVariable.ULTI)) {
            animator.SetBool(ApplicationVariable.ULTI, false);
        }
        animator.SetBool("IsIdle", true);
        yield return new WaitForSeconds(timeCoolDown / 3);
        animator.SetBool("IsIdle", false);
        target = null;
        FindNearestTarget();
        isAttacking = false;
    }

    private void Wander() {
        if (!health.isAlive || iswinning || GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) { return; }
        if (enemy.isStopped) { enemy.isStopped = false; }
        animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
        enemy.SetDestination(GetRandomNavMeshPosition(transform.position, wanderRadius));
    }
    private Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas)) {
                randomPoint = hit.position;
                return hit.position;
            }
        }
        return origin;
    }
    private void OnDisable() {
        if (GameStateManager.Instance.currentStateGame == ApplicationVariable.StateGame.InLobby) {
            return;
        }
        /*        if (indicator) {
                    Destroy(indicator);
                }*/
        if (active_ultimate) {
            GamePlayController.Instance.isHoldGiftBox = false;
            active_ultimate = false;
            attackRange -= 2.5f;
        }
        isAttacking = false;
        if (GamePlayController.Instance) {
            GamePlayController.Instance.MinusEnemy();
        }
    }
}
