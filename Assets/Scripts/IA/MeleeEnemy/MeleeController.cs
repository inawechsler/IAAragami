using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : AIController
{
    [Header("Melee Components")]
    private FSM<MAIEnum> fsm;
    ISteering pursuit;
    ISteering evade;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void InitTree()
    {
        var chaseAct = new ActionNode(() => fsm.Transition(MAIEnum.Chase));
        var idleAct = new ActionNode(() => fsm.Transition(MAIEnum.Idle));
        var attackAct = new ActionNode(() => fsm.Transition(MAIEnum.Attack));
        var evadeAct = new ActionNode(() => fsm.Transition(MAIEnum.Evade));
        var patrolAct = new ActionNode(() => fsm.Transition(MAIEnum.Patrol));

        var qHitTarget = new QuestionNode(QHitTarget, idleAct, attackAct);
        var qCanAttack = new QuestionNode(QPlayerInRange, qHitTarget, chaseAct);
        var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);
        var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, chaseAct, qHasToWaitOnPatrol);
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanAttack, qHasLostPlayerRecently);

        rootNode = qCanWatchPlayer;
    }

    protected override void InitFSM()
    {
        fsm = new FSM<MAIEnum>();

        var stateList = new List<AISBase<MAIEnum>>();
        var idleSt = new AISIdle<MAIEnum>(2f);
        var chaseSt = new AISSteering<MAIEnum>(pursuit);
        var evadeSt = new AISSteering<MAIEnum>(evade);
        var patrolSt = new AISPatrol<MAIEnum>(model.waypoints);
        var attackSt = new AISAttack<MAIEnum>(target);

        idleSt.AddTransition(MAIEnum.Chase, chaseSt);
        idleSt.AddTransition(MAIEnum.Attack, attackSt);
        idleSt.AddTransition(MAIEnum.Evade, evadeSt);
        idleSt.AddTransition(MAIEnum.Patrol, patrolSt);

        chaseSt.AddTransition(MAIEnum.Attack, attackSt);
        chaseSt.AddTransition(MAIEnum.Idle, idleSt);
        chaseSt.AddTransition(MAIEnum.Evade, evadeSt);
        chaseSt.AddTransition(MAIEnum.Patrol, patrolSt);

        evadeSt.AddTransition(MAIEnum.Idle, idleSt);
        evadeSt.AddTransition(MAIEnum.Chase, chaseSt);

        attackSt.AddTransition(MAIEnum.Idle, idleSt);
        attackSt.AddTransition(MAIEnum.Chase, chaseSt);
        attackSt.AddTransition(MAIEnum.Evade, evadeSt);
        attackSt.AddTransition(MAIEnum.Patrol, patrolSt);

        patrolSt.AddTransition(MAIEnum.Idle, idleSt);
        patrolSt.AddTransition(MAIEnum.Chase, chaseSt);
        patrolSt.AddTransition(MAIEnum.Attack, attackSt);

        stateList.Add(patrolSt);
        stateList.Add(idleSt);
        stateList.Add(chaseSt);
        stateList.Add(attackSt);
        stateList.Add(evadeSt);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight, this, obstacleAvoidance);
        }

        fsm.SetInit(idleSt);
    }
    private bool QAIHasToWait()
    {
        return model.GetHasToWaitOnIdle();
    }
    private bool QHasLostPlayer()
    {
        return model.hasLostRecently; //Hay un get en el model pero no me funciona
    }

    private bool QHitTarget()
    {
        Debug.Log(attack.LastAttackHit());
        return attack.LastAttackHit();
    }

    private bool QPlayerInRange()
    {
        return LineOfSight.CheckRange(target, model.attackRange);

    }
    private bool QLineOfSight()
    {
        return LineOfSight.LOS(transform, target, model.range, model.angle, model.obsMask, model);
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

        steering = pursuit;
    }
}
