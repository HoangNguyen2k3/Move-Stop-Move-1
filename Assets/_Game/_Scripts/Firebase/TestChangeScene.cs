using UnityEngine;
using UnityEngine.SceneManagement;

public class TestChangeScene : MonoBehaviour
{
    public void GoToScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToScene2()
    {
        SceneManager.LoadScene(2);
    }
}
