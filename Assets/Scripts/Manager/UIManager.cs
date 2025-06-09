using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Dictionary<string, IUIElement> uiElements = new Dictionary<string, IUIElement>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void RegisterAllUIElements()
    {
        uiElements.Clear();

        IUIElement[] elements = FindObjectsByType<UIElement>(FindObjectsSortMode.None); 

        foreach (IUIElement element in elements)
        {
            RegisterUIElement(element); 
        }
    }

    public void RegisterUIElement(IUIElement element)
    {
        if (!string.IsNullOrEmpty(element.ElementID) && !uiElements.ContainsKey(element.ElementID))
        {
            uiElements.Add(element.ElementID, element); 
        }
    }
    public void ShowUI(string elementID) 
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Show();
        }
    }

    public void HideUI(string elementID)
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Hide();
        }
    }

    public void ToggleUI(string elementID)
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Toggle();
        }
    }

    public void UpdateUIText(string elementID, string message)
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element) &&
            element is UIElement canvasElement)
        {
            canvasElement.UpdateText(message);
        }
    }
}
