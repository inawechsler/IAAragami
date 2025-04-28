using TMPro;
using UnityEngine;

public class UIElement : MonoBehaviour, IUIElement
{
    [SerializeField] private string elementID;
    private Canvas canvas;

    public string ElementID => elementID;
    public bool IsVisible => canvas != null && canvas.enabled;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"No se encontró componente Canvas en {gameObject.name}");
        }
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
