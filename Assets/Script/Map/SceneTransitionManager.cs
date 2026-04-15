using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GoToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}
