using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private WeaponObject weapon;
    private bool boomerangActive = false;
    [Header("--------------Gift Box-----------------")]

    [Header("------------Zombie Mode---------------")]
    public bool isZombieMode = false;

    [HideInInspector] public GameObject who_throw_obj;
    [HideInInspector] public string who_throw = "Player";
    [HideInInspector] public LevelManager currentlevelObject;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Transform target_transform;
    [SerializeField] private GameObject explosion;
    private Vector3 startDir;
    private Vector3 startPosition;
    private bool check = false;
    [Header("---------------Special Weapon---------------")]
    public bool upScaleWeapon = false;
    private float scaleSpeed = 3f;
    private float maxScale = 9f;
    public bool throwEnemy = false;
    public bool throwWall = false;
    private float range_attack;
    public bool explosion_Attack = false;
    [Header("---------------Ultimate Weapon--------------")]
    public bool isUltimate = false;
    public Vector3 dir_ulti;

    private void Start() {
        //Ultimate
        //startPosition = transform.position;
        //range_attack = weapon.range;
        if (isUltimate) {
            transform.LookAt(target);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            scaleSpeed *= transform.localScale.x;
            startPosition = transform.position;
            //startPosition.y = who_throw_obj.transform.position.y + 0.5f;
            range_attack = weapon.range;
            return;
        }
        //Normal
        scaleSpeed *= transform.localScale.x;
        maxScale *= transform.localScale.x;
        if (!weapon.isTurning) {
            if (!isZombieMode)
                transform.LookAt(target);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        startPosition = transform.position;
        startDir = (target - transform.position).normalized;
        range_attack = weapon.range;
        if (upScaleWeapon) {
            range_attack += 10f;
        }
    }
    private void Update() {
        if (target_transform != null) {
            check = true;
        }
        if (currentlevelObject == null) {
            Destroy(gameObject);
        }
        if (isUltimate) {
            IncreaseScale();
            UltimateWeapon();
            //MainWeapon();
            return;
        }
        if (upScaleWeapon) {
            MainWeapon();
            IncreaseScale();
            return;
        }
        if (target_transform == null && !check) {
            if (!weapon.isBoomerang) {
                MainWeapon();
            }
            else {
                BoomerangWeapon();
            }
        }
        else {
            ChaseWeapon();
        }

    }
    public void ChangeMaxScale(float scale, float scale_speed) {
        maxScale = scale;
        scaleSpeed = scale_speed;
    }
    private void BoomerangWeapon() {
        if (weapon.isTurning) {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
        }
        if (!boomerangActive) {
            Vector3 newPosition = transform.position + dir * weapon.speedMove * Time.deltaTime;
            if (!isZombieMode) {
                newPosition = transform.position + startDir * weapon.speedMove * Time.deltaTime;
            }
            transform.position = newPosition;
            if (Vector3.Distance(startPosition, transform.position) >= range_attack) {
                startPosition = transform.position;
                boomerangActive = true;
            }
        }
        else {
            dir = (who_throw_obj.transform.position - startPosition).normalized;
            startDir = (who_throw_obj.transform.position - startPosition).normalized;
            Vector3 newPosition = transform.position + dir * weapon.speedMove * Time.deltaTime;
            if (!isZombieMode) {
                newPosition = transform.position + startDir * weapon.speedMove * Time.deltaTime;
            }
            transform.position = newPosition;
            if (Vector3.Distance(startPosition, transform.position) >= range_attack || Vector3.Distance(transform.position, who_throw_obj.transform.position) < 1f) {
                Destroy(gameObject);
            }
        }
    }
    private void ChaseWeapon() {
        if (target_transform == null) {
            FindNearestTarget();
            if (target_transform == null) return;
        }
        if (!weapon.isTurning) {
            transform.LookAt(target_transform);
            transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        if (weapon.isTurning) {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
        }
        Vector3 targetPosition = target_transform.position;
        targetPosition.y = who_throw_obj.transform.position.y + 0.6f;
        float step = weapon.speedMove * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (Vector3.Distance(startPosition, transform.position) >= range_attack) {
            Destroy(gameObject);
        }
    }
    private void IncreaseScale() {
        if (transform.localScale.x < maxScale) {
            float newScale = transform.localScale.x + (scaleSpeed * Time.deltaTime);
            newScale = Mathf.Min(newScale, maxScale);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
    private void FindNearestTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(ApplicationVariable.ENEMY_TAG);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target_transform = closestEnemy;
    }
    private void UltimateWeapon() {
        Vector3 newPosition = transform.position + 1.2f * dir_ulti * weapon.speedMove * Time.deltaTime;
        transform.position = newPosition;
        if (Vector3.Distance(startPosition, transform.position) >= (1.5f * range_attack)) {
            Destroy(gameObject);
        }
    }
    private void MainWeapon() {
        Vector3 newPosition = FindNewPosition();
        transform.position = newPosition;
        if (Vector3.Distance(startPosition, transform.position) >= range_attack) {
            Destroy(gameObject);
        }
    }

    private Vector3 FindNewPosition() {
        if (weapon.isTurning) {
            transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y + (weapon.speedRotate * Time.deltaTime), 0f);
        }
        Vector3 newPosition = transform.position + dir * weapon.speedMove * Time.deltaTime;
        if (!isZombieMode) {
            newPosition = transform.position + startDir * weapon.speedMove * Time.deltaTime;
        }
        return newPosition;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<ThrowWeapon>() || other.CompareTag("ThrowWeapon") || other.gameObject.layer == LayerMask.NameToLayer("Road")) { return; }
        if (who_throw == ApplicationVariable.PLAYER_TAG) {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<EnemiesHealth>() &&
                other.gameObject.GetComponentInChildren<EnemiesHealth>().isBoss == false
                && other.gameObject.GetComponentInChildren<EnemiesHealth>().isScore == false) {
                ExplosionMethod();
                if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT) == 0) {
                    Handheld.Vibrate();
                }
                currentlevelObject.AddLevel();
                if (!throwEnemy && !isUltimate) {
                    Destroy(gameObject);
                }
            }
            else if (other.gameObject.GetComponentInChildren<EnemiesHealth>() &&
                other.gameObject.GetComponentInChildren<EnemiesHealth>().isBoss == true
                && other.gameObject.GetComponentInChildren<EnemiesHealth>().isScore == false) {
                if (!throwEnemy && !isUltimate) {
                    Destroy(gameObject);
                }
                ExplosionMethod();
            }
            else {
                /*                if (!other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG) && !other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG) && !throwWall) {
                                    ExplosionMethod();
                                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                                    Destroy(gameObject);
                                }*/
                // else
                if (!other.gameObject.GetComponentInChildren<EnemiesHealth>() && !other.gameObject.CompareTag(ApplicationVariable.PLAYER_TAG) && !throwWall) {
                    ExplosionMethod();
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
        else {
            if (other.gameObject.CompareTag(ApplicationVariable.IGNORE_TAG)) { return; }
            if (other.gameObject.GetComponentInChildren<PlayerController>()) {
                if (other.isTrigger) { return; }
                currentlevelObject.AddLevel();
                if (who_throw_obj != null) {
                    GamePlayController.Instance.name_enemy_win = who_throw_obj.GetComponentInParent<GenerateEnemyType>().nameEnemy.text;
                    other.gameObject.GetComponentInChildren<PlayerController>().isDead = true;
                }
                if (!isUltimate)
                    Destroy(gameObject);
            }
            else if (other.gameObject != who_throw_obj && other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG) && who_throw_obj != null) {
                other.gameObject.GetComponent<EnemiesHealth>().isAlive = false;
                currentlevelObject.AddLevel();
                other.gameObject.GetComponent<EnemiesHealth>().TakeColorMaterial();
                other.gameObject.GetComponent<EnemiesHealth>().Die();
                if (!isUltimate)
                    Destroy(gameObject);
            }
            else {
                if (!other.gameObject.GetComponentInChildren<PlayerController>() && !other.gameObject.CompareTag(ApplicationVariable.ENEMY_TAG)) {
                    Instantiate(weapon.touchSomething, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

    }

    private void ExplosionMethod() {

        if (explosion_Attack) {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    private void OnDisable() {
        if (SoundManager.Instance)
            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.hit_something);
    }
}
