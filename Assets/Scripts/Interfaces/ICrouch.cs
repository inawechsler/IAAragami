using System;
using UnityEngine;

public interface ICrouch 
{
    Action onCrouch { get; set; }
    void ToggleCrouch();
}
