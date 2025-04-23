using TMPro;
using UnityEngine;

public class PurchaseCustomWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] five_type_weapon;
    [SerializeField] private GameObject image_weapon;
    [SerializeField] private GameObject[] active_weapon;
    [SerializeField] private TextMeshProUGUI param_Weapon;
    [SerializeField] private TextMeshProUGUI nameWeapon;

    [SerializeField] private ComponentOptColor component;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject TableColor;

    [HideInInspector] public WeaponShop weapon;

    [SerializeField] private RectTransform begin_Pos;
    public static WeaponShop lastWeaponShop;

    [SerializeField] private FirstPageShop custom;
    [SerializeField] private WeaponShopUI weaponUI;
    public static float num_weap = 2;

    [SerializeField] private TextMeshProUGUI text_purchaseButton;
    [SerializeField] private UIGeneratePress ui_show;

    [SerializeField] private FirstPageShop[] firstPage;

    [SerializeField] private RewardAdsButton rewardBtn;
    public void ChangeWeapon() {
        lastWeaponShop = weapon;

        for (int i = 0; i < five_type_weapon.Length; i++) {
            five_type_weapon[i].GetComponent<MeshFilter>().mesh = weapon.skinWeapon[i].GetComponent<MeshFilter>().sharedMesh;
            five_type_weapon[i].GetComponent<MeshRenderer>().materials = weapon.skinWeapon[i].GetComponent<MeshRenderer>().sharedMaterials;
        }
        param_Weapon.text = weapon.param_Attack.ToString();
        nameWeapon.text = weapon.nameWeapon.ToString();
        component.ChangeComponent(weapon.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials.Length);
        custom.GetColorCustom(lastWeaponShop.imageWeapon.GetComponent<MeshRenderer>().sharedMaterials.Length);

        CheckCurrentWeaponCustom();
        CheckInitCustom();
    }
    public void CheckInitCustom() {
        if (!PlayerPrefs.HasKey("Color_" + PurchaseCustomWeapon.lastWeaponShop.nameWeapon + "_custom_0")) {
            for (int i = 0; i < lastWeaponShop.skinWeapon[0].GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
                Color selectedColor = lastWeaponShop.skinWeapon[0].GetComponent<MeshRenderer>().sharedMaterials[i].color;
                string hexColor = "#" + ColorUtility.ToHtmlStringRGB(selectedColor);
                PlayerPrefs.SetString("Color_" + PurchaseCustomWeapon.lastWeaponShop.nameWeapon + "_custom_" + i.ToString(), hexColor);
            }
        }
    }
    private void CheckCurrentWeaponCustom() {
        int temp = 2;
        for (int i = 0; i < 5; i++) {
            if (PlayerPrefs.HasKey(lastWeaponShop.nameWeapon + " select_button" + i)) {
                if (PlayerPrefs.GetString(lastWeaponShop.nameWeapon + " select_button" + i) == "Equip") {
                    temp = i; break;
                }
            }
        }
        for (int i = 0; i < five_type_weapon.Length; i++) {
            if (i == temp) { active_weapon[i].SetActive(true); }
            else { active_weapon[i].SetActive(false); }
        }
        num_weap = temp;
        firstPage[temp].num_weapon = temp;
        firstPage[temp].OnChangeType();
        firstPage[temp].CheckLock();
        if (temp == 0) {
            TableColor.SetActive(true);
        }
        else {
            TableColor.SetActive(false);
        }
        if (begin_Pos && temp != 0)
            purchaseButton.GetComponent<RectTransform>().anchoredPosition = begin_Pos.anchoredPosition;
        else {
            purchaseButton.GetComponent<RectTransform>().anchoredPosition = custom.custom_Pos.anchoredPosition;
        }
    }

    public void SelectWeapon() {
        if (num_weap == 4 || num_weap == 3) {
            if (!PlayerPrefs.HasKey(lastWeaponShop.nameWeapon + " select_button" + num_weap)) {
                rewardBtn?.ShowAds();
                return;
            }
        }
        weaponUI.SelectWeapon(lastWeaponShop, num_weap);
        PlayerPrefs.SetString(lastWeaponShop.nameWeapon + " select_button" + num_weap, ApplicationVariable.eqquip_status);
        weaponUI.SetAgain(lastWeaponShop.nameWeapon + " select_button" + num_weap);
        CheckEqippedWeapon((int)num_weap);
        ui_show.ShowAndHiddenGameObject();
    }
    public void GainAdsCustomWeapon() {
        weaponUI.SelectWeapon(lastWeaponShop, num_weap);
        PlayerPrefs.SetString(lastWeaponShop.nameWeapon + " select_button" + num_weap, ApplicationVariable.eqquip_status);
        weaponUI.SetAgain(lastWeaponShop.nameWeapon + " select_button" + num_weap);
        CheckEqippedWeapon((int)num_weap);
        ui_show.ShowAndHiddenGameObject();
    }
    public void CheckEqippedWeapon(int i) {

        if (PlayerPrefs.HasKey(lastWeaponShop.nameWeapon + " select_button" + i)) {
            if (PlayerPrefs.GetString(lastWeaponShop.nameWeapon + " select_button" + i) == ApplicationVariable.eqquip_status) {
                text_purchaseButton.text = ApplicationVariable.SELECTED_STATUS;
                return;
            }
            else {
                text_purchaseButton.text = ApplicationVariable.SELECT_STATUS;
                return;
            }
        }

    }

}
