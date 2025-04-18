using System.Collections;
using UnityEngine;

public class SDKInit : MonoBehaviour
{
    public GameObject firebaseAnalyzePrefab;
    public GameObject firebaseDatabasePrefab;
    public GameObject appsFlyerInitPrefab;
    public GameObject adsPrefab;
    public GameObject Facebook;
    public GameObject AOA_Mediation;

    private void Awake()
    {
        StartCoroutine(InitializeAllSDKs());
    }

    private IEnumerator InitializeAllSDKs()
    {
        adsPrefab.SetActive(true);
        //adsPrefab.GetComponent<ADS>().InitAppLovin();
        yield return new WaitForSeconds(0.15f);

        //Instantiate(appsFlyerInitPrefab);
        appsFlyerInitPrefab.SetActive(true);
        appsFlyerInitPrefab.GetComponent<AppsFlyerInit>().Init();
        yield return new WaitForSeconds(0.15f);

        //Instantiate(firebaseAnalyzePrefab);
        firebaseAnalyzePrefab.SetActive(true);
        firebaseAnalyzePrefab.GetComponent<FirebaseAnalyze>().Init();
        yield return new WaitForSeconds(0.15f);

        //Instantiate(firebaseDatabasePrefab);
        firebaseDatabasePrefab.SetActive(true);
        firebaseDatabasePrefab.GetComponent<Firebasedatabase>().InitFb();
        yield return new WaitForSeconds(0.15f);

        Facebook.SetActive(true);
        Facebook.GetComponent<FacebookInterstitial>().LoadInterstitial();
        yield return new WaitForSeconds(0.15f);

        AOA_Mediation.SetActive(true);
        AOA_Mediation.GetComponent<AOA_Mediation>().LoadAd();
    }
}
