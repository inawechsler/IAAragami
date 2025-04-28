using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
//MAISAttack = Melee AI State Attack 
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

        Debug.Log(_steering.GetType().Name);
        //look.LookDir(dir.normalized);
    }

    public override void Exit()
    {
        base.Exit();
    }


}