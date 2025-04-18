using UnityEngine;
using UnityEngine.UI;

public class ShowMRECOnClick : MonoBehaviour
{
    private const string MREC_AD_UNIT_ID = "163f6d1603f93b2a";
    private bool isMrecLoaded = false;
    private bool isMrecVisible = false;
    private bool isMrecInitialized = false;

    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration config) =>
        {
            //           Debug.Log("SDK ?ã kh?i t?o!");
            InitializeMREC();
        };
    }

    void InitializeMREC()
    {
        if (isMrecInitialized) return;

        isMrecInitialized = true;
        MaxSdk.CreateMRec(MREC_AD_UNIT_ID, MaxSdkBase.AdViewPosition.BottomCenter);
        MaxSdk.StopMRecAutoRefresh(MREC_AD_UNIT_ID);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += (adUnitId, adInfo) =>
        {
            //           Debug.Log("MREC t?i thành công!");
            isMrecLoaded = true;
            // Không t? ??ng g?i ShowMREC() ? ?ây
        };

        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += (adUnitId, error) =>
        {
            //       Debug.Log("MREC t?i th?t b?i: " + error.Message);
            isMrecLoaded = false;
        };

        MaxSdk.LoadMRec(MREC_AD_UNIT_ID);
    }

    public void ShowMREC()
    {
        if (!isMrecInitialized)
        {
            InitializeMREC();
            return;
        }

        if (!isMrecLoaded)
        {
            //   Debug.Log("MREC ch?a s?n sàng, ?ang t?i l?i...");
            MaxSdk.LoadMRec(MREC_AD_UNIT_ID);
            return;
        }

        if (!isMrecVisible)
        {
            MaxSdk.ShowMRec(MREC_AD_UNIT_ID);
            //    Debug.Log("MREC hi?n th?!");
            isMrecVisible = true;
        }
    }

    public void HideMREC()
    {
        if (isMrecVisible)
        {
            MaxSdk.HideMRec(MREC_AD_UNIT_ID);
            //  Debug.Log("MREC ?n!");
            isMrecVisible = false;
        }
    }

    public void ToggleMrec()
    {
        if (isMrecVisible)
        {
            HideMREC();
        }
        else
        {
            ShowMREC();
        }
    }
}
