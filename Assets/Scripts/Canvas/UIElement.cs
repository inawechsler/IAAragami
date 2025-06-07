using TMPro;
using UnityEngine;

public class UIElement : MonoBehaviour, IUIElement
{
    [SerializeField] private string elementID;
    private Canvas canvas;

    public string ElementID => elementID;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Show()
    {
        if (canvas != null) canvas.enabled = true;
    }

    public void Hide()
    {
        if (canvas != null) canvas.enabled = false;
    }

    public void Toggle()
    {
        if (canvas != null) canvas.enabled = !canvas.enabled;
    }

    public bool UpdateText(string message)
    {
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = message;
            return true;
        }
        return false;
    }
}
