using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour
{
    [Header("Special Shop")]
    public WeaponShop[] specialShop;
    [SerializeField] private GameObject specialShopObj1;
    [SerializeField] private GameObject specialShopObj2;
    [SerializeField] private GameObject normalshopObj;
    [Header("Normal Shop")]
    public WeaponShop[] weaponShops;
    //Text
    [SerializeField] private TextMeshProUGUI name_weapon;
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private TextMeshProUGUI paramWeapon;
    [SerializeField] private TextMeshProUGUI price;
    //button and image weapon
    [SerializeField] private GameObject image_Weapon;
    [SerializeField] private Button btn_purchase;
    [SerializeField] private Image icon_coin;
    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject purchasedWeapon;
    [SerializeField] private GameObject notPurchaseWeapon;
    [SerializeField] private PurchaseCustomWeapon customWeapon;
    //Ads 
    [SerializeField] private GameObject button_ads;
    [SerializeField] private TextMeshProUGUI text_ads;

    private int current_page;
    private int start_page;
    private int max_page;


    public static event EventHandler<WeaponObject> OnWeaponPurchase;
    /*    private void Start()
        {
            if (!PlayerPrefs.HasKey("EquippedWeapon"))
            {
                PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
                PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Equipped");
            }
            max_page = weaponShops.Length;
            current_page = 0;
            LoadWeaponStatus();
            for (int i = 0; i < max_page; i++)
            {
                if (weaponShops[i].status == ApplicationVariable.notPurchase_status)
                {
                    current_page = i; break;
                }
            }
            SettingShopUI();
        }*/
    private void OnEnable() {
        int enemyLayer = LayerMask.GetMask("Enemy");
        int weaponshop = LayerMask.GetMask("WeaponShop");
        Camera.main.cullingMask &= ~enemyLayer;
        Camera.main.cullingMask &= ~weaponshop;
        if (!PlayerPrefs.HasKey(ApplicationVariable.CURRENT_WEAPON_EQUIP)) {
            PlayerPrefs.SetString("EquippedWeapon", weaponShops[0].nameWeapon);
            PlayerPrefs.SetString("WeaponStatus_" + weaponShops[0].nameWeapon, "Equipped");
            PlayerPrefs.SetString(ApplicationVariable.CURRENT_WEAPON_EQUIP, weaponShops[0].nameWeapon);
        }

        max_page = weaponShops.Length;
        current_page = 0;
        LoadWeaponStatus();
        CheckCurrentPage(true);
        SettingShopUI();

        int temp = PlayerPrefs.GetInt("AddAds" + weaponShops[current_page].nameWeapon, 0);
        text_ads.text = temp.ToString() + "/2";
    }
    public void InitWeaponPlayer() {
        string temp = PlayerPrefs.GetString(ApplicationVariable.CURRENT_WEAPON_EQUIP);
        foreach (var weapon in weaponShops) {
            if (weapon.nameWeapon == temp) {
                for (int i = 0; i < 5; i++) {
                    string temp_str = PlayerPrefs.GetString(weapon.nameWeapon + " select_button" + i);
                    if (temp_str == "Equip") {
                        SelectWeapon(weapon, i);
                        return;
                    }
                }
                break;
            }
        }
    }
    public void AddOneTimeAds() {
        int temp = PlayerPrefs.GetInt("AddAds" + weaponShops[current_page].nameWeapon, 0);
        if (temp == 1) {
            var currentWeapon = weaponShops[current_page];
            foreach (var weapon_check in weaponShops) {
                if (weapon_check.status == ApplicationVariable.eqquipped_status) {
                    weapon_check.status = ApplicationVariable.purchase_status;
                    PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
                }
            }
            PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
            currentWeapon.status = ApplicationVariable.eqquipped_status;
            OnWeaponPurchase?.Invoke(this, currentWeapon.weapon);
            /*            player.characterPlayer.current_Weapon = currentWeapon.weapon;
                        player.TakeInfoHoldWeapon();*/
            SelectWeapon(currentWeapon, 2);
            //player.characterPlayer.current_Weapon = currentWeapon.weapon;
            //player.TakeInfoHoldWeapon();

            PlayerPrefs.SetString(weaponShops[current_page].nameWeapon + " select_button" + 2, "Equip");
            SetAgain(weaponShops[current_page].nameWeapon + " select_button" + 2);
            CheckCurrentPage();
            LeftArrowPressed();
            SettingShopUI();
            return;
        }
        PlayerPrefs.SetInt("AddAds" + weaponShops[current_page].nameWeapon, ++temp);
        text_ads.text = temp.ToString() + "/2";
    }

    private void CheckCurrentPage(bool first_open = false) {
        for (int i = 0; i < max_page; i++) {
            if (weaponShops[i].status == ApplicationVariable.notPurchase_status) {
                current_page = i; break;
            }
        }
        if (current_page == 0) {
            start_page = -1;
            current_page = max_page - 1;
        }
        else {
            start_page = current_page;
        }
        if (first_open) {
            string weapon_choose = PlayerPrefs.GetString(ApplicationVariable.CURRENT_WEAPON_EQUIP);
            for (int i = 0; i < max_page; i++) {
                if (weaponShops[i].nameWeapon == weapon_choose) {
                    current_page = i; break;
                }
            }
        }
    }

    private void LoadWeaponStatus() {
        foreach (var weapon in weaponShops) {
            string status = PlayerPrefs.GetString("WeaponStatus_" + weapon.nameWeapon, ApplicationVariable.notPurchase_status);
            weapon.status = status;
        }
    }

    public void SettingShopUI() {
        var currentWeapon = weaponShops[current_page];
        FirebaseAnalyze.Instance?.LogEvent("WeaponShop", "Weapon", currentWeapon.nameWeapon);
        if (currentWeapon.status == ApplicationVariable.notPurchase_status) {
            if (start_page == current_page) {
                button_ads.SetActive(true);
                btn_purchase.interactable = true;

            }
            else {
                btn_purchase.interactable = false;
                button_ads.SetActive(false);
            }
            purchasedWeapon.SetActive(false);
            notPurchaseWeapon.SetActive(true);
            LockWeapon();
        }
        else {
            customWeapon.weapon = currentWeapon;
            customWeapon.ChangeWeapon();
            notPurchaseWeapon.SetActive(false);
            purchasedWeapon.SetActive(true);
        }
        int temp = PlayerPrefs.GetInt("AddAds" + weaponShops[current_page].nameWeapon, 0);
        text_ads.text = temp.ToString() + "/2";
    }

    private void LockWeapon() {
        var currentWeapon = weaponShops[current_page];

        name_weapon.text = currentWeapon.nameWeapon;
        image_Weapon.GetComponent<MeshFilter>().mesh = currentWeapon.imageWeapon.GetComponent<MeshFilter>().sharedMesh;
        image_Weapon.GetComponent<MeshRenderer>().materials = currentWeapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials;
        paramWeapon.text = currentWeapon.param_Attack;

        if (currentWeapon.status == ApplicationVariable.purchase_status) {
            status.text = "(Unlock)";
            price.text = "EQUIP";
            icon_coin.gameObject.SetActive(false);
        }
        else if (currentWeapon.status == ApplicationVariable.notPurchase_status) {
            status.text = "(Lock)";
            price.text = currentWeapon.price.ToString();
            icon_coin.gameObject.SetActive(true);
        }
        else if (currentWeapon.status == ApplicationVariable.eqquipped_status) {
            status.text = "(Unlock)";
            price.text = "EQUIPPED";
            icon_coin.gameObject.SetActive(false);
        }
        if (start_page != current_page) {
            status.text = "(Unlock " + weaponShops[current_page - 1].nameWeapon + " First)";
        }
    }

    public void LeftArrowPressed() {
        if (current_page > 0) {
            current_page--;
            SettingShopUI();
        }
    }

    public void RightArrowPressed() {
        if (current_page < max_page - 1 && current_page >= 0) {
            current_page++;
            SettingShopUI();
        }
    }
    public void OnPurchaseOrEqquip() {
        var currentWeapon = weaponShops[current_page];
        float coin = PlayerPrefs.GetFloat(ApplicationVariable.COIN, 0f);
        if (currentWeapon.status == ApplicationVariable.notPurchase_status) {
            if (coin >= weaponShops[current_page].price) {
                coin -= weaponShops[current_page].price;
                PlayerPrefs.SetFloat(ApplicationVariable.COIN, coin);
                foreach (var weapon_check in weaponShops) {
                    if (weapon_check.status == ApplicationVariable.eqquipped_status) {
                        weapon_check.status = ApplicationVariable.purchase_status;
                        PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
                    }
                }
                PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
                currentWeapon.status = ApplicationVariable.eqquipped_status;
                OnWeaponPurchase?.Invoke(this, currentWeapon.weapon);

                SelectWeapon(currentWeapon, 2);
                //player.characterPlayer.current_Weapon = currentWeapon.weapon;
                //player.TakeInfoHoldWeapon();

                PlayerPrefs.SetString(weaponShops[current_page].nameWeapon + " select_button" + 2, "Equip");
                SetAgain(weaponShops[current_page].nameWeapon + " select_button" + 2);
                if (current_page != max_page - 1) {
                    CheckCurrentPage();
                    LeftArrowPressed();
                }
            }
            else {
                //Lam tiep canh bao khong du tien
            }
            SettingShopUI();
        }
        else if (currentWeapon.status == ApplicationVariable.purchase_status) {

            foreach (var weapon_check in weaponShops) {
                if (weapon_check.status == ApplicationVariable.eqquipped_status) {
                    weapon_check.status = ApplicationVariable.purchase_status;
                    PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
                }
            }
            PlayerPrefs.SetString("WeaponStatus_" + currentWeapon.nameWeapon, "Equipped");
            currentWeapon.status = ApplicationVariable.eqquipped_status;
            OnWeaponPurchase?.Invoke(this, currentWeapon.weapon);
            //player.characterPlayer.current_Weapon = currentWeapon.weapon;
            //player.TakeInfoHoldWeapon();
            SelectWeapon(currentWeapon, 2);
            PlayerPrefs.SetString(weaponShops[current_page].nameWeapon + " select_button" + 2, "Equip");
            SetAgain(weaponShops[current_page].nameWeapon + " select_button" + 2);
            if (current_page != max_page - 1) {
                CheckCurrentPage();
                LeftArrowPressed();
            }
            SettingShopUI();
        }

    }
    public void SelectWeapon(WeaponShop weapon, float num) {
        foreach (var weapon_check in weaponShops) {
            if (weapon_check.status == ApplicationVariable.eqquipped_status) {
                weapon_check.status = ApplicationVariable.purchase_status;
                PlayerPrefs.SetString("WeaponStatus_" + weapon_check.nameWeapon, ApplicationVariable.purchase_status);
            }
        }
        //PlayerPrefs.SetString("EquipCurrentWeapon", PurchaseCustomWeapon.lastWeaponShop.nameWeapon);
        PlayerPrefs.SetString(ApplicationVariable.CURRENT_WEAPON_EQUIP, weapon.nameWeapon);


        player.characterPlayer.current_Weapon = weapon.weapon;
        if (num == 0) { TakeColorWeaponFromDatabase(); }
        else {
            if (num == 3 || num == 4) {
                for (int i = 0; i < player.characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
                    player.characterPlayer.skin_current_weapon.material[i].mainTexture = weapon.skinWeapon[(int)num].GetComponent<MeshRenderer>().sharedMaterials[i].mainTexture;
                    player.characterPlayer.skin_current_weapon.material[i].color = Color.white;
                }
            }
            else {
                for (int i = 0; i < player.characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
                    player.characterPlayer.skin_current_weapon.material[i].mainTexture = null;
                    player.characterPlayer.skin_current_weapon.material[i].color = weapon.skinWeapon[(int)num].GetComponent<MeshRenderer>().sharedMaterials[i].color;
                }
            }

        }
        player.TakeInfoHoldWeapon();
    }
    public void TakeColorWeaponFromDatabase() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.CURRENT_WEAPON_EQUIP)) { return; }
        string name_weapon = PlayerPrefs.GetString(ApplicationVariable.CURRENT_WEAPON_EQUIP);
        for (int i = 0; i < player.characterPlayer.current_Weapon.weaponThrow.GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
            player.characterPlayer.skin_current_weapon.material[i].mainTexture = null;
            string key = "Color_" + name_weapon + "_custom_" + i;
            string hexColor = PlayerPrefs.GetString(key);

            if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor)) {
                player.characterPlayer.skin_current_weapon.material[i].color = newColor;
            }
        }

    }
    public void SetAgain(string a) {
        foreach (var weapon_check in weaponShops) {
            for (int i = 0; i < 5; i++) {
                if (PlayerPrefs.HasKey(weapon_check.nameWeapon + " select_button" + i)) {
                    if (PlayerPrefs.GetString(weapon_check.nameWeapon + " select_button" + i) == "Equip" && a != (weapon_check.nameWeapon + " select_button" + i)) {
                        PlayerPrefs.SetString(weapon_check.nameWeapon + " select_button" + i, "UnEquip");
                    }
                }
                else {
                    if (i == 3 || i == 4) { continue; }
                    PlayerPrefs.SetString(weapon_check.nameWeapon + " select_button" + i, "UnEquip");
                }
            }
        }
    }
    public void GainAdsCustomWeapon() {
        customWeapon.GainAdsCustomWeapon();
    }
    private void OnDisable() {
        if (!gameObject) { return; }
        int enemyLayer = LayerMask.GetMask("Enemy");
        int weaponshop = LayerMask.GetMask("WeaponShop");
        Camera.main.cullingMask |= enemyLayer;
        Camera.main.cullingMask |= weaponshop;
    }
}
