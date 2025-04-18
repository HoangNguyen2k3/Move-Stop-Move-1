using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardAdsButton : MonoBehaviour
{
    public UnityEvent OnReward;
    public UnityEvent OnFail;
    public bool isPause = false;
    public ApplicationVariable.ConditionAds conAds;
    private Button button;

    public bool isTakeOnClick = true;
    private void Awake() {
        button = GetComponent<Button>();
        if (isTakeOnClick)
            button.onClick.AddListener(() => { ShowAds(); });
    }
    public void ShowAds() {
        RewardAds.Instance.ShowRewardAds(
        () => { OnReward.Invoke(); },
        () => { OnFail.Invoke(); },
        isPause,
        conAds
        );
    }
}
