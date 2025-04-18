using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textlevel;
    [Header("---------------If is Player----------------")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerZombie playerZombie;
    [SerializeField] private float maxCam = 200;
    [SerializeField] private ParticleSystem levelup;
    [SerializeField] private GameObject textAdding;
    [SerializeField] private GameObject textAnnouceDistance;
    [SerializeField] private CinemachineCamera cam;

    public float startLevel = 0f;
    public float current_level;

    private float temp = 1;
    private float addingLevel = 1;
    private bool isPlayer = false;
    private float addingScale = 0.025f;
    [Header("---------------------If is enemy------------------")]
    private float addingRange = 0.25f;
    public bool isEnemy = false;

    [Header("---------------ZombieMode---------------")]

    [SerializeField] private bool zombieMode = false;
    [SerializeField] private PermParamAdd addingItem;
    public float current_num_weapon_throw = 1f;
    public GameObject circle;
    public ZombieGameController zombieManager;
    public bool isUpScale = false;

    private float addingOrbit = 0.15f;
    private void OnEnable() {
        if (zombieMode)
            LevelUpRangeSetUp();
    }
    private void Start() {

        if (zombieMode && playerZombie) {
            temp = 2f;
            isPlayer = true;
        }
        else {
            if (gameObject.GetComponent<PlayerController>()) {
                isPlayer = true;
            }

        }
        current_level = startLevel;
        textlevel.text = current_level.ToString();
        if (isUpScale) {
            LevelUpRange_1();
            isUpScale = false;
        }
    }
    /*    private void Update() {
            if (isPlayer && GamePlayController.Instance) {
                GamePlayController.Instance.num_coin = current_level;
            }
        }*/
    public void AddLevel() {
        if (isPlayer && textAdding) {
            textAdding.SetActive(true);
            if (playerZombie) {
                textAdding.GetComponent<TextMeshProUGUI>().text = "+" + 1;
            }
            else {
                textAdding.GetComponent<TextMeshProUGUI>().text = "+" + addingLevel;
            }
        }
        if (playerZombie) {
            current_level++;
        }
        else {
            current_level += addingLevel;
        }
        if (zombieManager) {
            zombieManager.num_coin = current_level;
        }
        textlevel.text = current_level.ToString();
        if (current_level >= 5 * temp && current_level != 0) {
            LevelUp();
        }

    }
    public void AddLevelLoop() {
        textlevel.text = current_level.ToString();
        if (current_level >= 5 * temp && current_level != 0) {
            LevelUp();
            AddLevelLoop();
        }

    }
    private void LevelUp() {
        if (playerController || playerZombie) {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.level_up);
            textAnnouceDistance.SetActive(true);
            if (playerController) {
                textAnnouceDistance.GetComponent<TextMeshProUGUI>().text = (transform.localScale.x * 10).ToString("F2") + " m";
            }
            else {
                textAnnouceDistance.GetComponent<TextMeshProUGUI>().text = "Level up";
            }
            textAnnouceDistance.GetComponent<Animator>().Play(ApplicationVariable.TEXT_ANNOUCE);
            if (cam.Lens.FieldOfView <= maxCam) {
                cam.Lens.FieldOfView += 2.5f;
            }
        }
        if (zombieMode && playerZombie) {
            if (zombieMode && current_num_weapon_throw < addingItem.num_max_throw) {
                current_num_weapon_throw++;
                playerZombie.num_throw_attack = current_num_weapon_throw;
                if (playerZombie.num_choose == 2) {
                    playerZombie.ChangeRangeOrbit(playerZombie.range.gameObject.GetComponent<CapsuleCollider>().radius + addingOrbit);
                    addingOrbit += 0.25f;
                }
            }
        }
        if (!levelup.isPlaying) {
            levelup.Play();
        }
        if (zombieMode) {
            temp += 4;
        }
        else {
            temp++;
        }
        addingLevel++;
        if (isEnemy) {
            gameObject.GetComponent<EnemyAI>().attackRange += addingRange;
            gameObject.transform.localScale += Vector3.one * addingScale;
        }
        if (playerController) {
            transform.localScale += Vector3.one * addingScale;
        }

        if (playerController) {

            playerController.addingScale += 2.5f;
        }
        if (zombieMode && playerZombie) {
            LevelUpRange();
        }

    }
    public void LevelUpRange() {
        if (cam.Lens.FieldOfView <= maxCam) {
            cam.Lens.FieldOfView += 2.5f / 2;
        }
        // circle.transform.localScale += new Vector3(0.025f * 2, 0.025f * 2, 0.025f * 2);
        transform.localScale += new Vector3(addingScale, addingScale, addingScale);
        playerZombie.speed += 0.3f;
    }
    public void LevelUpRange_1() {
        if (cam.Lens.FieldOfView <= maxCam) {
            cam.Lens.FieldOfView += 2.5f;
        }
        // circle.transform.localScale += new Vector3(0.025f * 2, 0.025f * 2, 0.025f * 2);
        transform.localScale += Vector3.one * 2 * addingScale;
        playerZombie.speed += 0.4f;
    }
    public void LevelUpRangeSetUp() {
        float temp = addingItem.num_add_range;
        if (cam.Lens.FieldOfView <= maxCam) {
            cam.Lens.FieldOfView += 2.5f * temp / 10;
        }
        float temp_1 = 0.025f * temp / 10 * 2;
        circle.transform.localScale += new Vector3(temp_1, temp_1, temp_1);
    }
}
