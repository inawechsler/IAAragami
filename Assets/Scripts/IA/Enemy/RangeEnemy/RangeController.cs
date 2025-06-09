using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : AIController
{
    [Header("Range Components")]
    private FSM<RAIEnum> fsm;
    ISteering evade;
    //No necesito que se vea, por eso lo pongo en HideInInspector
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


        var qCanDropMine = new QuestionNode(QTimeToDropMine, mineDropAct, evadeAct);//Si puede dropear la mina, lo hace, si no, evade
        var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);//Si debe esperar en el patrol, ejecuta idle, si no, patrol
        var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, qCanDropMine, qHasToWaitOnPatrol);//Si lo perdió hace poco lo sigue persiguiendo, si no, chequea si debe esperar en el patrol
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanDropMine, qHasLostPlayerRecently);//Si puede ver al jugador, intentará dropear la mina, si no chequeará que lo dejó de ver hace poco

        rootNode = qCanWatchPlayer;
    }

    protected override void InitFSM()
    {
        fsm = new FSM<RAIEnum>();


        var stateList = new List<AISBase<RAIEnum>>();


        var idleSt = new AISIdle<RAIEnum>();
        var evadeSt = new AISSteering<RAIEnum>(evade);
        var patrolSt = new AISPatrol<RAIEnum>(behaviourManager.waypoints, this, behaviourManager.patrolRandom);
        var mineDropSt = new AISAttack<RAIEnum>(target);

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
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight, this, obstacleAvoidance, path);
        }

        fsm.SetInit(idleSt);
    }

    private bool QTimeToDropMine()
    {
        return rangeModel.isTimeToDropMine;//La corrutina que maneja el tiempo de dropear la mina maneja el bool
    }
    protected override void ExecuteFSM()
    {
        fsm.OnExecute();
    }

    protected override void InitSteering()
    {
        evade = new Evade(transform, rbTarget, timePrediction);

        steering = evade;
    }
}
