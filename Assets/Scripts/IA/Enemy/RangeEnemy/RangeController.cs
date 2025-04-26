using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : AIController
{
    [Header("Range Components")]
    private FSM<RAIEnum> fsm;
    ISteering pursuit;
    ISteering evade;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void InitTree()
    {
        //var chaseAct = new ActionNode(() => fsm.Transition(MAIEnum.Chase));
        //var idleAct = new ActionNode(() => fsm.Transition(MAIEnum.Idle));
        //var attackAct = new ActionNode(() => fsm.Transition(MAIEnum.Attack));
        //var evadeAct = new ActionNode(() => fsm.Transition(MAIEnum.Evade));
        //var patrolAct = new ActionNode(() => fsm.Transition(MAIEnum.Patrol));

        //var qHitTarget = new QuestionNode(QHitTarget, idleAct, attackAct);
        //var qCanAttack = new QuestionNode(QPlayerInRange, qHitTarget, chaseAct);
        //var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);
        //var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, chaseAct, qHasToWaitOnPatrol);
        //var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanAttack, qHasLostPlayerRecently);

        //rootNode = qCanWatchPlayer;
    }

    protected override void InitFSM()
    {
        fsm = new FSM<RAIEnum>();


    }


    protected override void ExecuteFSM()
    {
        fsm.OnExecute();
    }

    protected override void InitSteering()
    {
        // Che, acá inicializamos los comportamientos de steering para el melee
        pursuit = new Pursuit(transform, rbTarget, timePrediction);
        evade = new Evade(transform, rbTarget, timePrediction);

        steering = evade;
    }
}
