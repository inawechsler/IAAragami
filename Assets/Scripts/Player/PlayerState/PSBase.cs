using UnityEngine;

public class PSBase<T> : State<T>
{
    protected ILook look;
    protected IMove move;
    protected ICrouch crouch;
    protected FSM<T> fsm;
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);
        if (args.Length > 0)
        {
            look = args[0] as ILook;
            move = args[1] as IMove;
            fsm = args[2] as FSM<T>;
            if(args.Length > 3)
                crouch = args[3] as ICrouch;
            StateMachine = fsm;
        }
    }
}

