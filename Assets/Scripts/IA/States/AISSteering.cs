using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AISSteering<T> : AISBase<T>
{
    private ISteering steering;
    public AISSteering(ISteering target)
    {
        steering = target;
    }
    public override void Execute()
    {
        base.Execute();
        var dir = steering.GetDir();
        move.Move(dir.normalized);
        //look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
    }


}