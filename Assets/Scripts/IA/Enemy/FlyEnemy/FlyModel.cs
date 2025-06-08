using UnityEngine;

public class FlyModel : AIModel, IBoid
{
    public Rigidbody Target;

    public Transform safeSpot;
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
}

