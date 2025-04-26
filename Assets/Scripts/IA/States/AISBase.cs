using UnityEngine;

public class AISBase<T> : State<T>
{
    protected ILook look;
    protected IMove move;
    protected LineOfSight LOS;
    protected AIController controller;
    protected IAttack attack;
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);
        if (args.Length > 0)
        {
            look = args[0] as ILook;
            move = args[1] as IMove;
            fsm = args[2] as FSM<T>;
            attack = args[3] as IAttack;
            if (args.Length > 4)
                LOS = args[4] as LineOfSight;
            if(args.Length > 5)
                controller = args[5] as AIController;
            StateMachine = fsm;
        }
    }
}

