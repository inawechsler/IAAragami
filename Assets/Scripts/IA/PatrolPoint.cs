using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    //public int id;
    Vector3 position => transform.position;
    public Vector3 Position
    {
        get { return position; }
    }


    //public PatrolPoint nextPoint;



}
