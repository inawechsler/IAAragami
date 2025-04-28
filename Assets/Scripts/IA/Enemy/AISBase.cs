using UnityEngine;

public class AISBase<T> : State<T>
{
    protected ILook look;
    protected IMove move;
    protected LineOfSightMono LOS;
    protected AIController controller;
    protected IAttack attack;
    protected ObstacleAvoidance obs;
    public override void Initialize(params object[] args)
    {
        base.Initialize(args);
        if (args.Length > 0)
        {
            look = args[0] as ILook;
            move = args[1] as IMove;
            fsm = args[2] as FSM<T>;
            attack = args[3] as IAttack;
            LOS = args[4] as LineOfSightMono;
            controller = args[5] as AIController;
            obs = args[6] as ObstacleAvoidance;
            StateMachine = fsm;
        }
    }
}