using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("----------------Player Base----------------")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float angle = 5f;
    [SerializeField] private Joystick my_joyStick;
    [SerializeField] private GameObject hold_weapon;
    [SerializeField] private Transform posStart;
    [SerializeField] private GameObject circleTarget;
    [SerializeField] private CinemachineCamera cam_end;
    [SerializeField] private ParticleSystem take_damage_FX;
    [SerializeField] public SkinnedMeshRenderer current_Mesh;
    [SerializeField] private Material begin_Mat;
    [Header("--------------------Player Skin----------------------")]
    public CharaterObj characterPlayer;
    public GameObject[] skinPlayerObject;
    public GameObject[] fullSkinPlayObject;
    private Material begin_Material;

    public float addingScale = 0;
    private bool isCoolDown = false;
    private bool isEnemyInRange = false;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 direct;
    private Transform firstEnemy = null;
    private List<Transform> enemiesInRange = new List<Transform>();
    public bool isDead = false;
    public bool isRealDead = false;
    private bool isAnimationDead = false;
    [HideInInspector] public bool isWinning = false;
    public bool canRevive = true;
    public bool inRevive = false;
    public UIManager uiManager;
    //[SerializeField] private TextMeshProUGUI name_text;
    //[SerializeField] private Image level_bg;

    [SerializeField] private TextMeshPro name_text;
    [SerializeField] private SpriteRenderer level_bg;

    private GameObject temp_target;
    [Header("-----------------Init weapon---------------")]
    [SerializeField] private WeaponShop weaponShops;
    [SerializeField] private WeaponShopUI customWeapon;
    [SerializeField] private ChooseType checkAds;

    private bool checking = false;
    [Header("-------------Ultimate Weapon----------------")]
    public bool active_ultimate = false;
    public CapsuleCollider col;
    public GameObject RangeCircle;

    private void Awake() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.CURRENT_WEAPON_EQUIP)) {
            PlayerPrefs.SetString("EquippedWeapon", weaponShops.nameWeapon);
            PlayerPrefs.SetString("WeaponStatus_" + weaponShops.nameWeapon, "Equipped");
            PlayerPrefs.SetString(ApplicationVariable.CURRENT_WEAPON_EQUIP, weaponShops.nameWeapon);
            PlayerPrefs.SetString(weaponShops.nameWeapon + " select_button" + 2, "Equip");
            PlayerPrefs.SetString(weaponShops.nameWeapon + " select_button" + 1, "UnEquip");
            PlayerPrefs.SetString(weaponShops.nameWeapon + " select_button" + 0, "UnEquip");
            customWeapon.SelectWeapon(weaponShops, 2);
        }
        if (PlayerPrefs.HasKey(ApplicationVariable.USE_ONE_TIME_IN_ZOMBIE)) {
            checkAds.UseAdsCloths();
            PlayerPrefs.DeleteKey(ApplicationVariable.USE_ONE_TIME_IN_ZOMBIE);
        }

    }
    private void Start() {
        begin_Material = begin_Mat;
        transform.localScale = new Vector3(characterPlayer.beginRange, characterPlayer.beginRange, characterPlayer.beginRange);
        animator = GetComponent<Animator>();
        circleTarget.SetActive(false);
        TakeInfoHoldWeapon();
        if (CheckHaveCloth() || characterPlayer.fullSkinPlayer) {
            if (characterPlayer.fullSkinPlayer) {
                TakeInfoFullSkin();
            }
            else {
                TakeInfoCloth();
            }
        }
        name_text.text = PlayerPrefs.GetString(ApplicationVariable.NAME_PLAYER, "You");
        name_text.color = current_Mesh.material.color;
        level_bg.color = current_Mesh.material.color;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG)) {
            Transform enemy = other.transform;
            if (!enemiesInRange.Contains(enemy))
                enemiesInRange.Add(enemy);

            if (firstEnemy == null) {
                firstEnemy = enemy;
                UpdateCircleTarget();
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG)) {
            Transform enemy = other.transform;
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy) {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(ApplicationVariable.ENEMY_TAG) && !isCoolDown && firstEnemy == other.transform && direct == Vector3.zero && GameStateManager.Instance.currentStateGame != ApplicationVariable.StateGame.InLobby && !isDead) {
            StartCoroutine(Attack());
            temp_target = other.gameObject;
        }
    }

    public void Ultimate() {
        active_ultimate = true;
        GamePlayController.Instance.isHoldGiftBox = true;
        col.radius += 1.5f;
        RangeCircle.transform.localScale += Vector3.one * 0.7f;
    }

    public void ProcessPlayer() {
        if (isWinning) {
            return;
        }
        if (isDead && !isAnimationDead) {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.dead);
            if (PlayerPrefs.GetInt(ApplicationVariable.VIBRANT) == 1) {
                Handheld.Vibrate();
            }
            StartCoroutine(DiePlayer());
        }
        if (isDead) { return; }
        direct.x = my_joyStick.Horizontal;
        direct.z = my_joyStick.Vertical;
        Movement();
        RotateCharacter();
        if (isEnemyInRange) {
            if (firstEnemy) {
                circleTarget.transform.position = firstEnemy.position;
            }

        }
    }

    #region Setup Clothes
    public bool CheckHaveCloth() {
        for (int i = 0; i < characterPlayer.skinClother.Length; i++) {
            if (characterPlayer.skinClother[i] != null) {
                return true;
            }
        }
        return false;
    }
    public void TakeInfoCloth() {
        if (!CheckHaveCloth()) {
            current_Mesh.material = begin_Material;
            name_text.color = current_Mesh.material.color;
            level_bg.color = current_Mesh.material.color;
            SetActiveOnSmt(skinPlayerObject, true);
            SetActiveOffSmt(fullSkinPlayObject);
            for (int i = 0; i < skinPlayerObject.Length; i++) {
                switch (i) {
                    case 0:
                        foreach (Transform child in skinPlayerObject[i].transform) {
                            Destroy(child.gameObject);
                        }
                        break;
                    case 1:
                        skinPlayerObject[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = begin_Material;
                        break;
                    case 2:
                        goto case 0;
                    case 3: break;

                }
            }
            return;
        }
        current_Mesh.material = begin_Material;
        name_text.color = current_Mesh.material.color;
        level_bg.color = current_Mesh.material.color;
        for (int i = 0; i < skinPlayerObject.Length; i++) {
            if (characterPlayer.skinClother[i] != null) {
                SettingSkin(characterPlayer.skinClother[i], i);
            }
            else {
                switch (i) {
                    case 0:
                        foreach (Transform child in skinPlayerObject[i].transform) {
                            Destroy(child.gameObject);
                        }
                        break;
                    case 1:
                        skinPlayerObject[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = begin_Material;
                        break;
                    case 2:
                        goto case 0;
                    case 3: break;
                }
            }
        }
    }
    public void SettingSkin(ClotherShop skin, int index) {
        current_Mesh.material = begin_Material;
        name_text.color = current_Mesh.material.color;
        level_bg.color = current_Mesh.material.color;
        SetActiveOnSmt(skinPlayerObject, true);
        SetActiveOffSmt(fullSkinPlayObject);
        switch (index) {
            case 0:
                foreach (Transform child in skinPlayerObject[index].transform) {
                    Destroy(child.gameObject);
                }
                Instantiate(skin.skin, skinPlayerObject[index].transform);
                break;
            case 1:
                skinPlayerObject[index].GetComponent<SkinnedMeshRenderer>().materials = skin.skin.GetComponentInChildren<MeshRenderer>().sharedMaterials;
                break;
            case 2:
                goto case 0;
            case 3: break;

        }
    }
    public void SetupSkinWhenChooseFullSkin() {
        for (int i = 0; i <= 2; i++) {
            if (characterPlayer.skinClother[i] != null) {
                characterPlayer.skinClother[i].status = "Purchase";
                characterPlayer.skinClother[i] = null;
            }
        }
        characterPlayer.skinClother[0] = null;
        characterPlayer.skinClother[1] = null;
        characterPlayer.skinClother[2] = null;
        foreach (Transform child in skinPlayerObject[0].transform) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < skinPlayerObject[1].GetComponent<SkinnedMeshRenderer>().materials.Length; i++) {
            skinPlayerObject[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[i] = begin_Material;
        }

        foreach (Transform child in skinPlayerObject[2].transform) {
            Destroy(child.gameObject);
        }
    }
    public void CheckPlayerCharacter() {
        for (int i = 0; i <= 2; i++) {
            if (characterPlayer.skinClother[i] && characterPlayer.skinClother[i].status == "Purchase") {
                characterPlayer.skinClother[i] = null;
            }
        }
    }
    #endregion

    #region Setup Full Skin
    public void SettingFullSkin(FullSkinObject skin) {
        SetActiveOffSmt(skinPlayerObject, true);
        SetActiveOnSmt(fullSkinPlayObject);
        fullSkinPlayObject[0].GetComponent<SkinnedMeshRenderer>().material = skin.skin;
        name_text.color = skin.skin.color;
        level_bg.color = skin.skin.color;
        setupSomething(skin.accessories, 1);
        setupSomething(skin.head, 2);
        setupSomething(skin.weaponSkin, 3);
        setupSomething(skin.tail, 4);
    }
    private void setupSomething(GameObject obj, int index) {

        foreach (Transform child in fullSkinPlayObject[index].transform) {
            Destroy(child.gameObject);
        }
        if (obj == null) { return; }
        Instantiate(obj, fullSkinPlayObject[index].transform);

    }
    public void TakeInfoFullSkin() {
        if (characterPlayer.fullSkinPlayer)
            SettingFullSkin(characterPlayer.fullSkinPlayer);
    }
    #endregion

    #region misc
    public void Ahaha() {
        SetActiveOnSmt(skinPlayerObject, true);
        SetActiveOffSmt(fullSkinPlayObject);
    }
    private void SetActiveOffSmt(GameObject[] gameObject, bool ignore_first = false) {
        if (ignore_first == true) { gameObject[0].SetActive(false); }
        for (int i = 1; i < gameObject.Length; i++) {
            gameObject[i].SetActive(false);
        }
    }
    private void SetActiveOnSmt(GameObject[] gameObject, bool ignore_first = false) {
        if (ignore_first == true) { gameObject[0].SetActive(true); }
        for (int i = 1; i < gameObject.Length; i++) {
            gameObject[i].SetActive(true);
        }
    }
    #endregion

    #region Setup Weapon
    public void TakeInfoHoldWeapon() {
        if (!checking) {
            checking = true;
            customWeapon.InitWeaponPlayer();
        }
        for (int i = 0; i < characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
            characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials[i].color = characterPlayer.skin_current_weapon.material[i].color;
            characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials[i].color = characterPlayer.skin_current_weapon.material[i].color;

        }
        hold_weapon.GetComponent<MeshFilter>().mesh = characterPlayer.current_Weapon.weaponHold.GetComponent<MeshFilter>().sharedMesh;
        hold_weapon.GetComponent<MeshRenderer>().materials = characterPlayer.current_Weapon.weaponHold.GetComponent<MeshRenderer>().sharedMaterials;
    }
    private IEnumerator DiePlayer() {
        ReturnEnvironmentMat();
        ParticleSystem temp = Instantiate(take_damage_FX, transform.position, Quaternion.identity);
        temp.GetComponent<ParticleSystemRenderer>().material = current_Mesh.material;
        isAnimationDead = true;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, true);
        GamePlayController.Instance.CheckLose();
        inRevive = true;
        if (canRevive && GameStateManager.Instance.currentStateGame != ApplicationVariable.StateGame.Winning) {
            GamePlayController.Instance.CheckNewNumSmt();
            if (GamePlayController.Instance.enemy_remain > 1) {
                yield return new WaitForSeconds(0.5f);
                uiManager.SetReviveActive();
                gameObject.SetActive(false);
                canRevive = false;
            }
            else {
                isDead = true;
                isRealDead = true;
                yield return new WaitForSeconds(0.5f);
                Destroy(gameObject);
            }
        }
        else {
            isDead = true;
            isRealDead = true;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
    public void ReturnEnvironmentMat() {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(ApplicationVariable.OBSTACLE);
        foreach (GameObject obj2 in obj) {
            obj2.GetComponentInChildren<TouchToObjectEnv>().ReturnForceColorObj();
        }
    }
    public void RevivePlayer() {
        inRevive = false;
        GamePlayController.Instance.CheckNewNumSmt();
        isDead = false;
        gameObject.transform.position = GamePlayController.Instance.GetRandomNavMeshPositionPlayer(30f);
        gameObject.SetActive(true);
        isAnimationDead = false;
        enemiesInRange.Clear();
        firstEnemy = null;
        animator.SetBool(ApplicationVariable.IS_DEAD_STATE, false);
        animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
    }
    private void Movement() {
        if (direct != Vector3.zero) {
            if (hold_weapon.activeSelf == false) {
                hold_weapon.SetActive(true);
            }
            animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
            transform.position += direct.normalized * speed * Time.deltaTime;
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, false);
            isCoolDown = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if (!active_ultimate && animator.GetBool(ApplicationVariable.ULTI)) {
                animator.SetBool(ApplicationVariable.ULTI, false);
            }
        }
        else {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            animator.SetBool(ApplicationVariable.IDLE_PLAYER_STATE, true);
        }
    }

    private void RotateCharacter() {
        if (direct == Vector3.zero)
            return;
        Quaternion rot = Quaternion.LookRotation(direct.normalized);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot, angle);
    }

    private void UpdateCircleTarget() {
        if (firstEnemy != null) {
            if (firstEnemy.GetComponent<EnemiesHealth>().isAlive == false) {
                isEnemyInRange = false;
                circleTarget.SetActive(false);
                return;
            }
            circleTarget.SetActive(true);
            isEnemyInRange = true;
            circleTarget.transform.position = firstEnemy.position;
        }
        else {
            isEnemyInRange = false;
            circleTarget.SetActive(false);
        }
    }
    private IEnumerator Attack() {
        if (active_ultimate) {
            animator.SetBool(ApplicationVariable.ULTI, true);
            animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, true);
        }
        else {
            animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, true);
        }
        yield return null;
    }
    #endregion

    #region Attack
    public void StartAttack() {
        if (firstEnemy != null) {
            /*            if (SoundManager.Instance)
                            SoundManager.Instance.PlaySFXSound(SoundManager.Instance.throwWeapon);*/
            Vector3 directionToEnemy = firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 20);
        }
        isCoolDown = true;
    }
    public void StopAttack() {
        if (animator.GetBool(ApplicationVariable.ULTI)) { animator.SetBool(ApplicationVariable.ULTI, false); }
        isCoolDown = false;
        hold_weapon.SetActive(true);
        animator.SetBool(ApplicationVariable.ATTACK_PLAYER_STATE, false);
    }
    public void StartThrow() {
        if (temp_target && temp_target.GetComponentInChildren<TargetPos>()) {
            if (SoundManager.Instance)
                SoundManager.Instance.PlaySFXSound(SoundManager.Instance.throwWeapon);
            hold_weapon.SetActive(false);
            ThrowWeapon(temp_target.GetComponentInChildren<TargetPos>().transform.position, posStart.position);
        }
    }
    public void ThrowWeapon(Vector3 target, Vector3 startPos) {

        if (firstEnemy != null) {
            Vector3 directionToEnemy = firstEnemy.position - transform.position;
            directionToEnemy.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * 20);
        }
        GameObject throwWeaponPrefab = Instantiate(characterPlayer.current_Weapon.weaponThrow, startPos, Quaternion.identity);
        throwWeaponPrefab.transform.localScale += Vector3.one * addingScale;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().currentlevelObject = GetComponent<LevelManager>();
        throwWeaponPrefab.GetComponent<ThrowWeapon>().target = target;
        throwWeaponPrefab.GetComponent<ThrowWeapon>().who_throw_obj = gameObject;
        if (active_ultimate) {
            throwWeaponPrefab.GetComponent<ThrowWeapon>().isUltimate = true;
            throwWeaponPrefab.GetComponent<ThrowWeapon>().ChangeMaxScale(throwWeaponPrefab.transform.localScale.x * 4, 5);
            Vector3 currentPosAttack = posStart.position;
            currentPosAttack.y = transform.position.y + 1f;
            target.y = transform.position.y + 1f;
            Vector3 dir = (target - currentPosAttack).normalized;
            throwWeaponPrefab.GetComponent<ThrowWeapon>().dir_ulti = dir;
            active_ultimate = false;
            GamePlayController.Instance.isHoldGiftBox = false;
            col.radius -= 1.5f;
            RangeCircle.transform.localScale -= Vector3.one * 0.7f;
        }
    }
    public void RemoveEnemyFromList(Transform enemy) {
        if (enemiesInRange.Contains(enemy)) {
            enemiesInRange.Remove(enemy);

            if (enemy == firstEnemy) {
                firstEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
                UpdateCircleTarget();
            }
        }
    }
    #endregion
    private void OnDisable() {
        if (active_ultimate) {
            GamePlayController.Instance.isHoldGiftBox = false;
            active_ultimate = false;
            col.radius -= 1.5f;
            RangeCircle.transform.localScale -= Vector3.one * 0.7f;
        }
    }
    private void OnDestroy() {
        Destroy(circleTarget);
    }
}
