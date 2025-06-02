using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    Vector3 position => transform.position;
    public Vector3 Position
    {
        get { return position; }
    }

}
