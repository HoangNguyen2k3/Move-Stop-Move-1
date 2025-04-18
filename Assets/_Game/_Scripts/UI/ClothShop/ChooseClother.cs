using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseClother : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] activeButton;
    [SerializeField] private ClotherShop[] clothShop;
    [SerializeField] private GameObject[] lock_item;
    [SerializeField] private PlayerController player;

    [SerializeField] private Button button_Purchase;
    [SerializeField] private RectTransform positionStart;
    [SerializeField] private RectTransform positionEnd;
    [Header("Button Ads")]
    // [SerializeField] private Button button_Ads;
    [SerializeField] private GameObject button_Ads;

    [SerializeField] private Button exit_button;
    [SerializeField] private TextMeshProUGUI text_button;
    [SerializeField] private Image coinImage;

    [SerializeField] private CoinManager coinManager;
    [SerializeField] private TextMeshProUGUI paramText;
    [SerializeField] private FullSkinObject[] fullSkinShop;
    public int num_page = 0;
    public int current_num_page;

    private int current_index = 0;
    [Header("Background Image")]
    [SerializeField] private Image[] bgImage;
    [SerializeField] private Color color_choose;
    [SerializeField] private Color color_base;
    [SerializeField] private GameObject ads_Notice;
    private void Awake()
    {
        /*        CheckButtonStatus(0);
                SetActiveCurrentClothes(0);
                SetTempPlayerSkin(0);*/
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SetActiveCurrentClothes(index);
                SetTempPlayerSkin(index);
                CheckButtonStatus(index);
                SetParamSkin(index);
            });
        }
        exit_button.onClick.AddListener(() =>
        {
            ExitBtn();
        });
    }
    private void OnEnable()
    {
        ChooseType.OnChangeTypeClothes += ChangeTyeClother;
        if (current_num_page == 0)
            ChangeTyeClother(null, 0);
    }

    private void ChangeTyeClother(object sender, int e)
    {
        num_page = e;
        if (num_page == 3)
        {
            player.TakeInfoFullSkin();
        }
        else
        {
            player.TakeInfoCloth();
        }
        for (int i = 0; i < lock_item.Length; i++)
        {
            CheckButtonStatus(i);

        }
        CheckFullSkinActive();
        int temp = CheckCurrentActiveWeaponPage();
        if (temp == -1) { temp = 0; }
        if (current_num_page == e)
        {
            button[temp].onClick.Invoke();
        }
        SetActiveCurrentClothes(temp);
        if (!CheckCurrentFullSkin())
        {
            SetTempPlayerSkin(temp);
        }
        SetParamSkin(temp);
    }
    private void CheckFullSkinActive()
    {
        if (player.characterPlayer.fullSkinPlayer != null && num_page != 3)
        {
            foreach (var item in clothShop)
            {
                if (item.status == "Selected")
                {
                    item.status = "Purchase";
                }
            }
        }
    }
    private int CheckCurrentActiveWeaponPage()
    {
        int temp = -1;
        if (num_page != 3)
        {
            for (int i = 0; i < clothShop.Length; i++)
            {
                if (clothShop[i].status == "Selected")
                {
                    temp = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < fullSkinShop.Length; i++)
            {
                if (fullSkinShop[i].status == "Selected")
                {
                    temp = i;
                    break;
                }
            }
        }
        return temp;
    }

    private void CheckChooseSkin()
    {
        int temp = -1;
        if (num_page != 3)
        {
            for (int i = 0; i < clothShop.Length; i++)
            {
                if (clothShop[i].status == "Selected")
                {
                    temp = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < fullSkinShop.Length; i++)
            {
                if (fullSkinShop[i].status == "Selected")
                {
                    temp = i;
                    break;
                }
            }
        }
        for (int i = 0; i < bgImage.Length; i++)
        {
            if (i == temp)
            {
                bgImage[i].color = color_choose;
            }
            else
            {
                bgImage[i].color = color_base;
            }
        }
    }
    public bool CheckCurrentFullSkin()
    {
        if (player.characterPlayer.fullSkinPlayer == null)
        {
            return false;
        }
        return true;
    }
    public void ExitBtn()
    {
        if (CheckCurrentFullSkin())
        {
            player.TakeInfoFullSkin();
        }
        else
        {
            player.TakeInfoCloth();
        }
    }
    public void PurchaseOrSelectWeapon()
    {
        /*        if (num_page != 3 && CheckCurrentFullSkin())
                {
                    return false;
                }*/
        if (num_page != 3)
        {
            if (clothShop[current_index].status == "NotPurchase")
            {
                if (coinManager.PurchaseSomething(clothShop[current_index].price))
                {
                    SetRemainClothes("Purchase");
                    clothShop[current_index].status = "Selected";
                    player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                    if (player.characterPlayer.fullSkinPlayer != null)
                    {
                        player.characterPlayer.fullSkinPlayer.status = "Purchase";
                        player.characterPlayer.fullSkinPlayer = null;
                        player.GetComponent<PlayerController>().Ahaha();
                    }
                    player.TakeInfoCloth();
                    CheckButtonStatus(current_index);
                    SetClotherStatus("Selected");
                }
            }
            else if (clothShop[current_index].status == "Purchase")
            {
                SetRemainClothes("Purchase");
                clothShop[current_index].status = "Selected";
                player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                if (player.characterPlayer.fullSkinPlayer != null)
                {
                    player.characterPlayer.fullSkinPlayer.status = "Purchase";
                    player.characterPlayer.fullSkinPlayer = null;
                    player.GetComponent<PlayerController>().Ahaha();
                }
                player.TakeInfoCloth();
                CheckButtonStatus(current_index);
                SetClotherStatus("Selected");
            }
            //Adding
            else if (clothShop[current_index].status == "Selected")
            {
                SetRemainClothes("Purchase");
                clothShop[current_index].status = "Selected";
                player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
                if (player.characterPlayer.fullSkinPlayer != null)
                {
                    player.characterPlayer.fullSkinPlayer.status = "Purchase";
                    player.characterPlayer.fullSkinPlayer = null;
                    player.GetComponent<PlayerController>().Ahaha();
                }
                player.TakeInfoCloth();
                CheckButtonStatus(current_index);
                SetClotherStatus("Selected");
            }

        }
        else if (num_page == 3)
        {
            PurchaseOrSelectFullSkin();
        }
        //  return true;
    }
    public void PurchaseOrSelectFullSkin()
    {
        if (fullSkinShop[current_index].status == "NotPurchase")
        {
            if (coinManager.PurchaseSomething(fullSkinShop[current_index].price))
            {
                SetRemainClothes("Purchase");
                fullSkinShop[current_index].status = "Selected";
                player.characterPlayer.fullSkinPlayer = fullSkinShop[current_index];
                player.SetupSkinWhenChooseFullSkin();
                player.TakeInfoFullSkin();
                CheckButtonStatus(current_index);
                SetClotherStatus("Selected");
            }
        }
        else if (fullSkinShop[current_index].status == "Purchase")
        {
            SetRemainClothes("Purchase");
            fullSkinShop[current_index].status = "Selected";
            player.characterPlayer.fullSkinPlayer = fullSkinShop[current_index];
            player.SetupSkinWhenChooseFullSkin();
            player.TakeInfoFullSkin();
            CheckButtonStatus(current_index);
            SetClotherStatus("Selected");
        }
        /*        else if (fullSkinShop[current_index].status == "Selected" && CheckCurrentFullSkin())
                {
                    SetRemainClothes("Purchase");
                    fullSkinShop[current_index].status = "Purchase";
                    player.characterPlayer.fullSkinPlayer = null;
                    player.TakeInfoCloth();
                    CheckButtonStatus(current_index);
                    SetRemainClothes("Purchase");
                }*/
    }
    public void SetRemainClothes(string _status)
    {
        if (num_page != 3)
        {
            if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
                player.characterPlayer.skinClother[clothShop[current_index].clothType].status = _status;
            //            SavingData.SaveData(player.characterPlayer.skinClother[clothShop[current_index].clothType], ApplicationVariable.PATH_CLOTHES_PLAYER);
        }
        else
        {
            if (player.characterPlayer.fullSkinPlayer != null)
                player.characterPlayer.fullSkinPlayer.status = _status;
            //            SavingData.SaveData(player.characterPlayer.fullSkinPlayer, ApplicationVariable.PATH_CLOTHES_PLAYER);
        }
    }
    public void SetClotherStatus(string _status)
    {
        if (num_page != 3)
        {
            if (player.characterPlayer.skinClother[clothShop[current_index].clothType] != null)
                player.characterPlayer.skinClother[clothShop[current_index].clothType].status = _status;
            //            SavingData.SaveData(player.characterPlayer.skinClother[clothShop[current_index].clothType], ApplicationVariable.PATH_CLOTHES_PLAYER);
        }
        else
        {
            if (player.characterPlayer.fullSkinPlayer != null)
                player.characterPlayer.fullSkinPlayer.status = _status;
            //           SavingData.SaveData(player.characterPlayer.fullSkinPlayer, ApplicationVariable.PATH_CLOTHES_PLAYER);
        }
    }
    public void SetActiveCurrentClothes(int index)
    {
        current_index = index;
        for (int i = 0; i < button.Length; i++)
        {
            if (i == index)
            {
                activeButton[i].SetActive(true);
            }
            else
            {
                activeButton[i].SetActive(false);
            }
        }
    }
    private void CheckButtonStatus(int index)
    {
        if (num_page != 3)
        {
            if (clothShop[index].status == "NotPurchase")
            {
                SetButtonStatus(true, clothShop[index].price.ToString(), true);
                lock_item[index].SetActive(true);
                button_Ads.SetActive(true);
                button_Purchase.gameObject.transform.position = positionStart.position;
            }
            else if (clothShop[index].status == "Purchase")
            {
                SetButtonStatus(true, "SELECT", false);
                lock_item[index].SetActive(false);
                button_Ads.SetActive(false);
                button_Purchase.gameObject.transform.position = positionEnd.position;
            }
            else if (clothShop[index].status == "Selected")
            {
                SetButtonStatus(true, "SELECTED", false);
                lock_item[index].SetActive(false);
                button_Ads.SetActive(false);
                button_Purchase.gameObject.transform.position = positionEnd.position;
            }
            CheckAdsCloth();
            CheckChooseSkin();
        }
        else
        {
            CheckButtonStatusFullSkin(index);
            CheckChooseSkin();
        }
    }
    public void CheckAdsCloth()
    {
        if (PlayerPrefs.HasKey("TypeAdsOneTime"))
        {
            string temp = PlayerPrefs.GetString("NameAdsOneTime");
            if (temp == clothShop[current_index].nameClothShop)
            {
                ads_Notice?.SetActive(true);
                return;
            }
        }
        ads_Notice?.SetActive(false);
    }
    private void CheckButtonStatusFullSkin(int index)
    {
        string status = fullSkinShop[index].status;
        if (status == "NotPurchase")
        {
            SetButtonStatus(true, fullSkinShop[index].price.ToString(), true);
            lock_item[index].SetActive(true);
        }
        else if (status == "Purchase")
        {
            SetButtonStatus(true, "SELECT", false);
            lock_item[index].SetActive(false);
        }
        else if (status == "Selected")
        {
            SetButtonStatus(false, "SELECTED", false);
            lock_item[index].SetActive(false);
        }
    }
    private void SetButtonStatus(bool status_btn, string status, bool coin_status)
    {
        button_Purchase.interactable = status_btn;
        text_button.text = status;
        coinImage.gameObject.SetActive(coin_status);
    }
    public void SetTempPlayerSkin(int index)
    {
        if (num_page != 3)
        {
            ClotherShop skin = clothShop[index];
            player.SettingSkin(skin, skin.clothType);
        }
        else
        {
            SetTempPlayerFullSkin(index);
        }
    }
    public void SetParamSkin(int index)
    {
        if (num_page != 3)
        {
            ClotherShop skin = clothShop[index];
            paramText.text = skin.paramCloth;
        }
        else
        {
            SetParamFullSkin(index);
        }
    }
    public void SetTempPlayerFullSkin(int index)
    {
        FullSkinObject skin = fullSkinShop[index];
        player.SettingFullSkin(skin);
    }
    public void SetParamFullSkin(int index)
    {
        FullSkinObject skin = fullSkinShop[index];
        paramText.text = skin.param;
    }
    public void AddAdsTryClothes()
    {
        PlayerPrefs.SetString(ApplicationVariable.NAME_ADD_ONE_TIME, clothShop[current_index].nameClothShop);
        PlayerPrefs.SetString(ApplicationVariable.TYPE_ADD_ONE_TIME, clothShop[current_index].clothType.ToString());
        SetRemainClothes("Purchase");

        clothShop[current_index].status = "Selected";
        player.characterPlayer.skinClother[clothShop[current_index].clothType] = clothShop[current_index];
        if (player.characterPlayer.fullSkinPlayer != null)
        {
            player.characterPlayer.fullSkinPlayer.status = "Purchase";
            player.characterPlayer.fullSkinPlayer = null;
            player.GetComponent<PlayerController>().Ahaha();
        }
        player.TakeInfoCloth();
        CheckButtonStatus(current_index);
        SetClotherStatus("Selected");
    }
    public void RemoveAdsTryClothes()
    {
        string nameAdsClothes = PlayerPrefs.GetString(ApplicationVariable.NAME_ADD_ONE_TIME);
        if (!CheckPlayerCloth(nameAdsClothes)) { return; }
        for (int i = 0; i < clothShop.Length; i++)
        {
            if (clothShop[i].nameClothShop == nameAdsClothes)
            {
                clothShop[i].status = "NotPurchase";
                player.characterPlayer.skinClother[clothShop[i].clothType] = null;
                CheckButtonStatus(i);
                break;
            }
        }
        PlayerPrefs.DeleteKey(ApplicationVariable.NAME_ADD_ONE_TIME);
        PlayerPrefs.DeleteKey(ApplicationVariable.TYPE_ADD_ONE_TIME);
    }
    private bool CheckPlayerCloth(string a)
    {
        for (int i = 0; i < player.characterPlayer.skinClother.Length; i++)
        {
            if (player.characterPlayer.skinClother[i] != null && a == player.characterPlayer.skinClother[i].nameClothShop)
            {
                return true;
            }
        }
        return false;
    }
    private void OnDisable()
    {
        ChooseType.OnChangeTypeClothes -= ChangeTyeClother;
    }
}
