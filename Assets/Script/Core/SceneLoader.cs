using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GoMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}