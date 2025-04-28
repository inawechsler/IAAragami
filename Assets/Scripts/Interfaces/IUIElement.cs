using UnityEngine;

public interface IUIElement
{
    void Show();
    void Hide();
    void Toggle();
    bool IsVisible { get; }
    string ElementID { get; }

}
