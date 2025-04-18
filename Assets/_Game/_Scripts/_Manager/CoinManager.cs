using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numCoinUI;

    public float numCurrentCoin = 0;
    public float addCurrentCoin = 0;

    private void Awake() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.COIN)) {
            //  PlayerPrefs.SetFloat("Coin", 0);
            PlayerPrefs.SetFloat(ApplicationVariable.COIN, 10000);
        }
        //PlayerPrefs.SetFloat(ApplicationVariable.COIN, 10000);
    }
    private void OnEnable() {
        WeaponShopUI.OnWeaponPurchase += WeaponShopUI_OnWeaponPurchase;
    }
    private void Start() {
        if (!PlayerPrefs.HasKey(ApplicationVariable.COIN)) {
            PlayerPrefs.SetFloat(ApplicationVariable.COIN, 0);
        }
        numCurrentCoin = PlayerPrefs.GetFloat(ApplicationVariable.COIN);
        numCoinUI.text = numCurrentCoin.ToString();
    }

    private void WeaponShopUI_OnWeaponPurchase(object sender, WeaponObject weapon) {
        numCurrentCoin = PlayerPrefs.GetFloat(ApplicationVariable.COIN);
        numCoinUI.text = numCurrentCoin.ToString();
    }

    public void AddingCoin() {
        numCurrentCoin = PlayerPrefs.GetFloat(ApplicationVariable.COIN);
        if (ZombieGameController.Instance != null) {
            addCurrentCoin = ZombieGameController.Instance.num_coin;

        }
        else {
            addCurrentCoin = GamePlayController.Instance.num_coin;
        }
        numCurrentCoin += addCurrentCoin;
        numCoinUI.text = numCurrentCoin.ToString();
        PlayerPrefs.SetFloat(ApplicationVariable.COIN, numCurrentCoin);
    }
    public void AddingCoinXn(int times) {
        numCurrentCoin = PlayerPrefs.GetFloat(ApplicationVariable.COIN);
        if (ZombieGameController.Instance != null) {
            addCurrentCoin = ZombieGameController.Instance.num_coin * times;

        }
        else {
            addCurrentCoin = GamePlayController.Instance.num_coin * times;
        }
        numCurrentCoin += addCurrentCoin;
        numCoinUI.text = numCurrentCoin.ToString();
        PlayerPrefs.SetFloat(ApplicationVariable.COIN, numCurrentCoin);
    }
    public bool PurchaseSomething(float price) {
        if (numCurrentCoin >= price) {
            numCurrentCoin -= price;
            PlayerPrefs.SetFloat(ApplicationVariable.COIN, numCurrentCoin);
            numCoinUI.text = numCurrentCoin.ToString();
            return true;
        }
        else {
            return false;
        }
    }
    public void MinusCoin(float price) {
        numCurrentCoin -= price;
        PlayerPrefs.SetFloat(ApplicationVariable.COIN, numCurrentCoin);
        numCoinUI.text = numCurrentCoin.ToString();
    }
    private void OnDisable() {
        WeaponShopUI.OnWeaponPurchase -= WeaponShopUI_OnWeaponPurchase;
    }
}
