using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove2
{
    void Move(Vector3 dir);
    void LookDir(Vector3 dir);
    void SetPosition(Vector3 pos);
}
