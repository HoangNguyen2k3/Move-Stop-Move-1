using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform posStartThrow;
    private NavMeshAgent agent;
    private ZombieGameController manager;
    private bool dance = false;
    private bool isExistPlayer = true; //check player exist

    private Transform target;
    public GameObject indicatorPrefab;
    private GameObject indicator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Transform canvasTransform;

    public float num_alive_player = 1;
    public bool canAttack = true;
    private void Awake() {
        if (target == null) {
            if (GameObject.FindGameObjectWithTag(ApplicationVariable.PLAYER_TAG)) {
                target = GameObject.FindGameObjectWithTag(ApplicationVariable.PLAYER_TAG).transform;
                isExistPlayer = false;
            }
        }
        manager = FindFirstObjectByType<ZombieGameController>();
    }
    private void Start() {
        canvasTransform = GameObject.FindGameObjectWithTag("CanvasOverlay").transform;
        agent = GetComponent<NavMeshAgent>();
        Indicator();
    }
    /*    private void Update() {
            ZombieProcess();
        }*/

    public void ZombieProcess() {
        if (target == null && isExistPlayer) {
            if (GameObject.FindGameObjectWithTag("Player")) {
                animator.SetBool(ApplicationVariable.ZOMBIE_WALK, false);
                target = GameObject.FindGameObjectWithTag("Player").transform;
                isExistPlayer = false;
            }
            else {
                animator.SetBool(ApplicationVariable.ZOMBIE_WALK, true);
            }
        }
        if (!target && dance == false && !isExistPlayer) {
            dance = true;
            agent.isStopped = true;
            animator.SetBool(ApplicationVariable.ZOMBIE_WIN, true);
            return;
        }
        if (!target) { return; }
        agent.SetDestination(target.position);
    }

    private void Indicator() {
        indicator = Instantiate(indicatorPrefab, canvasTransform);
        indicator.GetComponent<OffScreenIndicatorZombie>().zombie = gameObject;
        indicator.GetComponent<OffScreenIndicatorZombie>().arrow.color = skinnedMeshRenderer.material.color;
        indicator.GetComponent<OffScreenIndicatorZombie>().target = posStartThrow;
        indicator.GetComponent<OffScreenIndicatorZombie>().mainCamera = Camera.main;
    }
    public void WinningZombie() {
        dance = true;
        agent.isStopped = true;
        animator.SetBool(ApplicationVariable.ZOMBIE_WIN, true);
        return;
    }
    private void OnDisable() {
        if (!ZombieGameController.Instance.currentInLobbyZombie)
            manager.MinusEnemy();
    }
    /*    private void OnDestroy() {
            manager.MinusEnemy();
        }*/
}
