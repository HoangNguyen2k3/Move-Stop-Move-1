using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseType : MonoBehaviour
{
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] list_item;
    public static EventHandler<int> OnChangeTypeClothes;
    [SerializeField] private GameObject buttonPurchase;
    [SerializeField] private GameObject buttonAds;
    [SerializeField] private RectTransform center;
    [SerializeField] private ChooseClother[] chooseClother;
    [SerializeField] private Button button_Purchase;
    [SerializeField] private Button button_Ads;
    [SerializeField] private GameObject textWarning;
    [SerializeField] private PlayerController player;

    private Vector3 begin;
    private int num;

    private void OnEnable()
    {
        SetActiveChooseWeaponType(0);
        /*        SettingButton(0);
                SetActiveChooseWeaponType(0);
                OnChangeTypeClothes?.Invoke(null, 0);*/
    }
    private void Start()
    {
        OnChangeTypeClothes?.Invoke(null, 0);
        begin = buttonPurchase.GetComponent<RectTransform>().anchoredPosition;
        SetActiveChooseWeaponType(0);
        for (int i = 0; i < button.Length; i++)
        {
            int index = i;
            button[i].onClick.AddListener(() =>
            {
                SettingButton(index);
                SetActiveChooseWeaponType(index);
                PlayerSetup(index);
                OnChangeTypeClothes?.Invoke(null, index);
            });
        }
        button_Purchase.onClick.AddListener(() =>
        {
            PurchaseOrSelectWeapon();
        });
        button_Ads.onClick.AddListener(() =>
        {
            //ClickAds();
        });

    }
    public void UseAdsCloths()
    {
        if (!PlayerPrefs.HasKey(ApplicationVariable.TYPE_ADD_ONE_TIME)) { return; }
        int temp;
        temp = int.Parse(PlayerPrefs.GetString(ApplicationVariable.TYPE_ADD_ONE_TIME));
        chooseClother[temp].num_page = temp;
        chooseClother[temp].RemoveAdsTryClothes();
    }

    public void PlayerSetup(int index)
    {
        if (index != 3)
        {
            player.CheckPlayerCharacter();
        }
    }
    public void ClickAds()
    {
        if (PlayerPrefs.HasKey(ApplicationVariable.TYPE_ADD_ONE_TIME))
        {
            UseAdsCloths();
        }
        chooseClother[num].num_page = num;
        chooseClother[num].AddAdsTryClothes();
    }
    public void PurchaseOrSelectWeapon()
    {
        chooseClother[num].num_page = num;
        chooseClother[num].PurchaseOrSelectWeapon();
    }
    public void SetActiveChooseWeaponType(int index)
    {
        num = index;
        for (int i = 0; i < button.Length; i++)
        {
            if (i == index)
            {
                button[i].interactable = false;
                list_item[i].SetActive(true);
            }
            else
            {
                button[i].interactable = true;
                list_item[i].SetActive(false);
            }
        }
    }
    public void SettingButton(int index)
    {
        if (index == 3)
        {
            buttonPurchase.GetComponent<RectTransform>().anchoredPosition = center.anchoredPosition;
            buttonAds.SetActive(false);
        }
        else
        {
            buttonPurchase.GetComponent<RectTransform>().anchoredPosition = begin;
            buttonAds.SetActive(true);
        }
    }
}
