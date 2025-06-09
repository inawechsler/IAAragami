using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public float splashDuration = 3f;

    void Start()
    {
        Invoke("LoadMainMenu", splashDuration);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
