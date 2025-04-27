using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RAISSteering<T> : AISBase<T>
{
    private ISteering steering;
    public RAISSteering(ISteering target)
    {
        steering = target;
    }
    public override void Execute()
    {
        base.Execute();
        var dir = steering.GetDir();
        move.Move(dir.normalized);

        //Debug.Log(steering.GetType().Name);
        //look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
    }


}