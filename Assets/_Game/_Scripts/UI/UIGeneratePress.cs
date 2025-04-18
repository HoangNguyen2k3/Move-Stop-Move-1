using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGeneratePress : MonoBehaviour
{
    [SerializeField] private GameObject[] showGameObject;
    [SerializeField] private GameObject[] hiddenGameObject;
    public void ShowGameObject()
    {
        for (int i = 0; i < showGameObject.Length; i++)
        {
            showGameObject[i].SetActive(true);
        }
    }
    public void HiddenGameObject()
    {
        for (int i = 0; i < hiddenGameObject.Length; i++)
        {
            hiddenGameObject[i].SetActive(false);
        }
    }
    public void ShowAndHiddenGameObject()
    {
        for (int i = 0; i < showGameObject.Length; i++)
        {
            if (showGameObject[i] != null) { showGameObject[i].SetActive(true); }
        }
        for (int i = 0; i < hiddenGameObject.Length; i++)
        {
            if (hiddenGameObject[i] != null) { hiddenGameObject[i].SetActive(false); }
        }
    }
    public async void ShowAndHiddenGameObj(float time)
    {
        int temp = (int)(time * 1000);
        await Task.Delay(temp);
        for (int i = 0; i < showGameObject.Length; i++)
        {
            if (showGameObject[i] != null) { showGameObject[i].SetActive(true); }
        }
        for (int i = 0; i < hiddenGameObject.Length; i++)
        {
            if (hiddenGameObject[i] != null) { hiddenGameObject[i].SetActive(false); }
        }
    }
    public void ReturnToHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
