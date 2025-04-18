using UnityEngine;
using UnityEngine.UI;

public class FBbuttonScene2 : MonoBehaviour
{
    [SerializeField] private Button chooseAbilities;
    [SerializeField] private Button revive_ads;
    [SerializeField] private Button x3Money_ads;
    [SerializeField] private Button x3Money_ads_2;
    [SerializeField] private Button ReturnToHome;

    private void Awake()
    {
        chooseAbilities.onClick.AddListener(() =>
        {
            ChooseAbilities();
        });
        revive_ads.onClick.AddListener(() =>
        {
            ReviveAds();
        });
        x3Money_ads.onClick.AddListener(() =>
        {
            TripleMoney();
        });
        x3Money_ads_2.onClick.AddListener(() =>
        {
            TripleMoney();
        });
        ReturnToHome.onClick.AddListener(() =>
        {
            ReturnTohome();
        });

    }

    private void ChooseAbilities()
    {
        FirebaseAnalyze.Instance?.LogEvent("ChooseAbilities_ZombieMode");
    }
    private void ReviveAds()
    {
        FirebaseAnalyze.Instance?.LogEvent("Revive");
    }
    private void TripleMoney()
    {
        FirebaseAnalyze.Instance?.LogEvent("TripleMoney");
    }
    private void ReturnTohome()
    {
        FirebaseAnalyze.Instance?.LogEvent("Return To Home");
    }
    public void ChoosePermAbilities(string a)
    {
        FirebaseAnalyze.Instance?.LogEvent("PermAbilities", "Type", a);
    }
}
