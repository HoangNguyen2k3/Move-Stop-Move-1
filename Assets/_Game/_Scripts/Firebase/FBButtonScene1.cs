using UnityEngine;
using UnityEngine.UI;

public class FBButtonScene1 : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button zombieModeBtn;
    [SerializeField] private Button weaponShop;
    [SerializeField] private Button clothShop;

    private void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            ClickPlayGame();
        });
        zombieModeBtn.onClick.AddListener(() =>
        {
            ClickZombieMode();
        });
        weaponShop.onClick.AddListener(() =>
        {
            ClickShopWeapon();
        });
        clothShop.onClick.AddListener(() =>
        {
            ClickShopClothes();
        });

    }

    private void ClickPlayGame()
    {
        FirebaseAnalyze.Instance?.LogEvent("PlayBaseGame");
    }
    private void ClickZombieMode()
    {
        FirebaseAnalyze.Instance?.LogEvent("EnterZombieMode");
    }
    private void ClickShopWeapon()
    {
        FirebaseAnalyze.Instance?.LogEvent("ClickWeaponShop");
    }
    private void ClickShopClothes()
    {
        FirebaseAnalyze.Instance?.LogEvent("ClickClothesShop");
    }
    public void WatchWeaponItem(string weapon)
    {
        FirebaseAnalyze.Instance?.LogEvent("WeaponShop", "Weapon", weapon);
    }
    public void WatchClothesItem(string cloth)
    {
        string[] name = cloth.Split('/');
        FirebaseAnalyze.Instance?.LogEvent("ClothesShop", name[0], name[1]);
    }
}
