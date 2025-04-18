using System.Collections;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void OnEnable()
    {
        StartCoroutine(LoadingText());
    }
    private IEnumerator LoadingText()
    {
        text.text = "Loading";
        for (int i = 0; i < 6; i++)
        {
            if (text.text == "Loading...")
            {
                text.text = "Loading";
            }
            text.text += '.';
            yield return new WaitForSeconds(0.3f);
        }
        gameObject.SetActive(false);
    }
}
