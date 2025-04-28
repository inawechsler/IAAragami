using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    static string sceneText;
    [SerializeField] Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        text = GameObject.FindWithTag("WinText").GetComponent<TextMeshProUGUI>();
        button = GameObject.FindWithTag("RestartButton").GetComponent<Button>();

        text.text = sceneText;
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public static void SetText(bool value)
    {

        if (value)
        {
             sceneText = "You escaped successfully!";
        }
        else
        {
            sceneText = "You got caught!";
        }
    }
}
