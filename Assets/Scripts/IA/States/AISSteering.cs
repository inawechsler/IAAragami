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
        move.Move(_obs.GetDir(dir.normalized));
        look.LookDir(_obs.GetDir(dir.normalized));
    }
}
