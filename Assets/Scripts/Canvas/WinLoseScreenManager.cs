using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseScreenManager : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image background;
    [SerializeField] Sprite winSprite;
    [SerializeField] Sprite loseSprite;

    static bool didWin;

    void Awake()
    {
        if (button == null)
            button = GameObject.FindWithTag("RestartButton").GetComponent<Button>();

        if (background == null)
            background = GameObject.FindWithTag("Background").GetComponent<Image>();

        background.sprite = didWin ? winSprite : loseSprite;

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public static void SetText(bool value)
    {
        didWin = value;
    }
}
