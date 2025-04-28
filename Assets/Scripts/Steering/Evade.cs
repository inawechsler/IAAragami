using UnityEngine;

public class Evade : Pursuit
{
    public Evade(Transform self, Rigidbody target, float timePrediction = 0) : base(self, target, timePrediction)
    {

    }

    public Evade(Transform self, Rigidbody target) : base(self, target)
    {

    }
    public override Vector3 GetDir()
    {
        return -base.GetDir();
    }
}
