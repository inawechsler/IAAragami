using UnityEngine;

public class AISBase<T> : State<T>
{
    protected ILook look;
    protected IMove move;
    protected LineOfSight LOS;
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);
        if (args.Length > 0)
        {
            look = args[0] as ILook;
            move = args[1] as IMove;
            fsm = args[2] as FSM<T>;
            if(args.Length > 3)
                LOS = args[3] as LineOfSight;
            StateMachine = fsm;
        }
    }
}

