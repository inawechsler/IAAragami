using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : AIController
{
    [Header("Range Components")]
    private FSM<RAIEnum> fsm;
    ISteering evade;
    [HideInInspector] public RangeModel rangeModel;

    protected override void Awake()
    {
        base.Awake();
        rangeModel = GetComponent<RangeModel>();
    }
    protected override void InitTree()
    {
        var idleAct = new ActionNode(() => fsm.Transition(RAIEnum.Idle));
        var mineDropAct = new ActionNode(() => fsm.Transition(RAIEnum.MineDrop));
        var evadeAct = new ActionNode(() => fsm.Transition(RAIEnum.Evade));
        var patrolAct = new ActionNode(() => fsm.Transition(RAIEnum.Patrol));


        var qCanDropMine = new QuestionNode(QTimeToDropMine, mineDropAct, evadeAct);
        var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);
        var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, qCanDropMine, qHasToWaitOnPatrol);
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanDropMine, qHasLostPlayerRecently);

        rootNode = qCanWatchPlayer;
    }

    protected override void InitFSM()
    {
        fsm = new FSM<RAIEnum>();


        var stateList = new List<AISBase<RAIEnum>>();


        var idleSt = new RAISIdle<RAIEnum>();
        var evadeSt = new RAISSteering<RAIEnum>(evade);
        var patrolSt = new RAISPatrol<RAIEnum>(model.waypoints);
        var mineDropSt = new RAISAttack<RAIEnum>(target);

        idleSt.AddTransition(RAIEnum.Evade, evadeSt);
        idleSt.AddTransition(RAIEnum.MineDrop, mineDropSt);
        idleSt.AddTransition(RAIEnum.Patrol, patrolSt);

        evadeSt.AddTransition(RAIEnum.Idle, idleSt);
        evadeSt.AddTransition(RAIEnum.Patrol, patrolSt);
        evadeSt.AddTransition(RAIEnum.MineDrop, mineDropSt);

        mineDropSt.AddTransition(RAIEnum.Patrol, patrolSt);
        mineDropSt.AddTransition(RAIEnum.Idle, idleSt);
        mineDropSt.AddTransition(RAIEnum.Evade, evadeSt);

        patrolSt.AddTransition(RAIEnum.Idle, idleSt);
        patrolSt.AddTransition(RAIEnum.Evade, evadeSt);
        patrolSt.AddTransition(RAIEnum.MineDrop, mineDropSt);

        stateList.Add(patrolSt);
        stateList.Add(idleSt);
        stateList.Add(mineDropSt);
        stateList.Add(evadeSt);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight, this, obstacleAvoidance);
        }

        fsm.SetInit(idleSt);
    }

    private bool QTimeToDropMine()
    {
     
        return rangeModel.isTimeToDropMine;
    }
    protected override void ExecuteFSM()
    {
        fsm.OnExecute();
        Debug.Log("QTE: " + QTimeToDropMine());
    }

    protected override void InitSteering()
    {
        evade = new Evade(transform, rbTarget, timePrediction);

        steering = evade;
    }
}
