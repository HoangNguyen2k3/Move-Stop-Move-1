using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] private GameObject enemy;
    [SerializeField] private SkinnedMeshRenderer current_Mesh;
    [SerializeField] private Transform pos_particle;
    public bool isAlive = true;

    private Collider currentCollider;
    private Animator animator;
    [Header("---------------Zombie Mode------------------")]
    public bool isZombie = false;
    public bool isScore = false;
    public bool isBoss = false;
    private int hpBoss = 10;
    private float scale_down = 0.05f;
    private void OnEnable() {
        currentCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        if (isAlive == false) {
            if (isZombie == true) {
                isScore = false;
            }
            currentCollider.enabled = true;
            isAlive = true;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!isZombie) {
            if ((other.GetComponent<ThrowWeapon>() && isAlive)) {
                if (other.GetComponent<ThrowWeapon>().who_throw == ApplicationVariable.ENEMY_TAG) { return; }
                ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
                isAlive = false;
                if (GamePlayController.Instance) {
                    GamePlayController.Instance.MinusEnemy();
                    if (GamePlayController.Instance.enemy_remain == 0) {
                        GamePlayController.Instance.CheckSpecialCase();
                    }
                }
                PlayerController player = FindFirstObjectByType<PlayerController>();
                if (player != null) {
                    player.RemoveEnemyFromList(transform);
                }
                temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
                Die();
            }
            return;
        }
        if ((other.CompareTag(ApplicationVariable.THROW_WEAPON_TAG) || other.GetComponent<ThrowWeapon>() || other.CompareTag(ApplicationVariable.EXPLOSION_TAG)) && isAlive) {

            //  if (other.gameObject == enemy) { return; }
            //   ParticleSystem temp = Instantiate(take_damage_FX, other.transform.position, Quaternion.identity);
            ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
            if (isBoss) {
                TakedDamageBoss();
            }
            else {
                currentCollider.enabled = false;
                isScore = true;
                isAlive = false;
                ZombieEnemy();
            }
            temp.GetComponentInChildren<ParticleSystemRenderer>().material = current_Mesh.material;
        }
    }
    private void TakedDamageBoss() {
        if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT) == 1) {
            Handheld.Vibrate();
        }
        transform.localScale -= new Vector3(scale_down, scale_down, scale_down);
        hpBoss -= 1;
        if (hpBoss <= 0) {
            isAlive = false;
            CircleRange player = FindFirstObjectByType<CircleRange>();
            if (player != null) {
                player.GetComponent<CircleRange>().RemoveEnemyFromList(transform);
            }
            PlayerZombie player_z = FindFirstObjectByType<PlayerZombie>();
            player_z.gameObject.GetComponent<LevelManager>().AddLevel();
            Destroy(currentCollider); Destroy(gameObject);
        }
    }
    public void TakeColorMaterial() {
        ParticleSystem temp = Instantiate(take_damage_FX, pos_particle.position, Quaternion.identity);
        isAlive = false;
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        //Die();
    }
    private void ZombieEnemy() {
        if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT) == 1) {
            Handheld.Vibrate();
        }
        CircleRange player = FindFirstObjectByType<CircleRange>();
        if (player != null) {
            player.GetComponent<CircleRange>().RemoveEnemyFromList(transform);
        }
        ObjectPooler.Instance.ReturnToPool(gameObject);
        //Destroy(currentCollider); Destroy(gameObject);
    }

    public void Die() {
        currentCollider.enabled = false;
        if (!take_damage_FX.isPlaying && !isAlive) {
            if (SoundManager.Instance) {
                SoundManager.Instance?.PlaySFXSound(SoundManager.Instance.dead);
            }
            animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
            Invoke(nameof(DestroyEnemy), 1.0f);
        }
    }

    private void DestroyEnemy() {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null) {
            player.GetComponent<PlayerController>().RemoveEnemyFromList(transform);
        }
        if (enemy.gameObject) {
            ObjectPooler.Instance.ReturnToPool(enemy);
            //Destroy(enemy.gameObject);
        }
    }
    /*    private void OnDisable() {
            if (isZombie && !isBoss && !ZombieGameController.Instance.currentInLobbyZombie) {
                //ObjectPooler.Instance.ReturnToPool(gameObject);
                ZombieGameController.Instance.MinusEnemy();
                currentCollider.enabled = true;
                isAlive = true;
                isScore = false;
            }
        }*/
}
