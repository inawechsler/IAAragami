using UnityEngine;

public interface IUIElement
{
    void Show();
    void Hide();
    void Toggle();
    string ElementID { get; }

}
