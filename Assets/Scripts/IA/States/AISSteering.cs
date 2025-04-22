using UnityEngine;
using UnityEngine.UIElements;

public class AISSteering<T> : PSBase<T>
{
    private ISteering steering;
    private T IdleInput;
    public AISSteering(ISteering target, T idleInput)
    {
        steering = target;
        IdleInput = idleInput;
    }
    public override void Execute()
    {
        base.Execute();
        var dir = steering.GetDir();
        move.Move(dir.normalized);
        look.LookDir(dir.normalized);

        if (Input.GetKeyDown(KeyCode.L))
        {
            fsm.Transition(IdleInput);
        }
    }
}
