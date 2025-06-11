using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : AIController
{
    [Header("Melee Components")]
    private FSM<MAIEnum> fsm;
    ISteering pursuit;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitTree()
    {
        var chaseAct = new ActionNode(() => fsm.Transition(MAIEnum.Chase));
        var idleAct = new ActionNode(() => fsm.Transition(MAIEnum.Idle));
        var attackAct = new ActionNode(() => fsm.Transition(MAIEnum.Attack));
        var patrolAct = new ActionNode(() => fsm.Transition(MAIEnum.Patrol));
        var pathfindingAct = new ActionNode(() =>fsm.Transition(MAIEnum.Pathfinding));

        var qHitTarget = new QuestionNode(QHitTarget, idleAct, attackAct);//Si le pegó al jugador, vuelve a idle, si no, ataca
        var qCanAttack = new QuestionNode(QPlayerInRange, qHitTarget, chaseAct);//Si puede atacar, chequea si le pegó, si no, lo persigue
        var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);//Si tiene que esperar ejecuta idle, si no, patrol
        var qIsOnPathfinding = new QuestionNode(QIsOnPathfinding, pathfindingAct, qHasToWaitOnPatrol);//Si está en pathfinding, ejecuta el pathfinding, si no, chequea si tiene que esperar en el patrol
        var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, chaseAct, qIsOnPathfinding);//Si lo perdió hace poco lo sigue persiguiendo, si no, chequea si debe esperar en el patrol
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanAttack, qHasLostPlayerRecently);//Si puede ver al jugador, intentará atacarlo, si no, chequeará que lo dejó de ver hace poco


        rootNode = qCanWatchPlayer;
    }

    protected override void InitFSM()
    {
        fsm = new FSM<MAIEnum>();

        var stateList = new List<AISBase<MAIEnum>>();
        var idleSt = new AISIdle<MAIEnum>();
        var chaseSt = new AISSteering<MAIEnum>(pursuit);
        var patrolSt = new AISPatrol<MAIEnum>(behaviourManager.waypoints, this, behaviourManager.patrolRoute);
        var attackSt = new AISAttack<MAIEnum>(target);
        var goToSt = new AISPathfinding<MAIEnum>(behaviourManager.waypoints[0].transform.position);

        idleSt.AddTransition(MAIEnum.Chase, chaseSt);
        idleSt.AddTransition(MAIEnum.Attack, attackSt);
        idleSt.AddTransition(MAIEnum.Patrol, patrolSt);
        idleSt.AddTransition(MAIEnum.Pathfinding, goToSt);

        chaseSt.AddTransition(MAIEnum.Attack, attackSt);
        chaseSt.AddTransition(MAIEnum.Idle, idleSt);
        chaseSt.AddTransition(MAIEnum.Patrol, patrolSt);
        chaseSt.AddTransition(MAIEnum.Pathfinding, goToSt);

        attackSt.AddTransition(MAIEnum.Idle, idleSt);
        attackSt.AddTransition(MAIEnum.Chase, chaseSt);
        attackSt.AddTransition(MAIEnum.Patrol, patrolSt);


        goToSt.AddTransition(MAIEnum.Patrol, patrolSt);
        goToSt.AddTransition(MAIEnum.Idle, idleSt);


        patrolSt.AddTransition(MAIEnum.Idle, idleSt);
        patrolSt.AddTransition(MAIEnum.Chase, chaseSt);
        patrolSt.AddTransition(MAIEnum.Attack, attackSt);
        patrolSt.AddTransition(MAIEnum.Pathfinding, goToSt);

        stateList.Add(patrolSt);
        stateList.Add(idleSt);
        stateList.Add(chaseSt);
        stateList.Add(attackSt);
        stateList.Add(goToSt);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight, this, obstacleAvoidance, path);
        }

        fsm.SetInit(idleSt);
    }

    private bool QHitTarget()
    {
        return attack.LastAttackHit();
    }

    private bool QIsOnPathfinding()
    {
        return path.isOnPathfinding;
    }

    protected override void ExecuteFSM()
    {
        fsm.OnExecute();
    }

    protected override void InitSteering()
    {
        pursuit = new Pursuit(transform, rbTarget, timePrediction);
    }
}
