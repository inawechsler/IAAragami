using UnityEngine;

public class FAISSteering<T> : AISBase<T>
{
    ISteering steering;
    public FAISSteering(ISteering steering)
    {
        this.steering = steering;
    }

    public override void Execute()
    {
        var dir = obs.GetDir(steering.GetDir());
        move.Move(dir.normalized);
        look.LookDir(dir.normalized);
    }
}
