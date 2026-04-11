using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class AISSteering<T> : AISBase<T>
{
    private ISteering _steering;
    public AISSteering(ISteering target)
    {
        _steering = target;
    }
    public override void Execute()
    {
        base.Execute();
        var dir = _steering.GetDir();
        move.Move(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
    }


}