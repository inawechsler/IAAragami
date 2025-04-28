using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Dictionary<string, IUIElement> uiElements = new Dictionary<string, IUIElement>(); //Diccionario para linkear las keyWords con los elementos UI

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

        IUIElement[] elements = FindObjectsByType<UIElement>(FindObjectsSortMode.None); //Obtengo todos los elementos UI

        foreach (IUIElement element in elements)
        {
            RegisterUIElement(element); //Recorro todos los elementos del array y los registro en el diccionario
        }
    }

    public void RegisterUIElement(IUIElement element)
    {
        if (!string.IsNullOrEmpty(element.ElementID) && !uiElements.ContainsKey(element.ElementID))
        {
            uiElements.Add(element.ElementID, element); //Si el ID no está vacío o no existe en el diccionario, lo añado
        }
    }
    public void ShowUI(string elementID) //Recibe ID y ejecuta show en el elemento con ese ID de key
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Show();
        }
    }

    public void HideUI(string elementID)//Recibe ID y ejecuta hide en el elemento con ese ID de key
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Hide();
        }
    }

    public void ToggleUI(string elementID)//Recibe ID y ejecuta toggle en el elemento con ese ID de key
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element))
        {
            element.Toggle();
        }
    }

    public void UpdateUIText(string elementID, string message)//Recibe ID y mensaje a mostrar en el elemento con ese ID de key
    {
        if (uiElements.TryGetValue(elementID, out IUIElement element) &&
            element is UIElement canvasElement)
        {
            canvasElement.UpdateText(message);
        }
    }
}
