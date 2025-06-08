using System.Collections.Generic;
using UnityEngine;

public class FlyController : AIController
{
    private FSM<MAIEnum> fsm;
    private FlyModel flyModel;
    protected override void Awake()
    {
        base.Awake();
        flyModel = GetComponent<FlyModel>();
    }
    protected override void InitTree()
    {
        var chaseAct = new ActionNode(() => fsm.Transition(MAIEnum.Chase));
        var idleAct = new ActionNode(() => fsm.Transition(MAIEnum.Idle));
        var evadeAct = new ActionNode(() => fsm.Transition(MAIEnum.Evade));
        var runAwayAct = new ActionNode(() => fsm.Transition(MAIEnum.RunAway));

        var qPlayerHasKey = new QuestionNode(QPlayerHasKey, evadeAct, chaseAct);//Si tiene la llave, idle, si no, persigue al jugador 
        var qIsOnPathFinding = new QuestionNode(QIsOnPathFinding, runAwayAct, idleAct);//Si esta en pathfinding, persigue al jugador, si no, idle
        var qCanMove = new QuestionNode(QIsOnDeadZone, qPlayerHasKey, qIsOnPathFinding);//Si puede moverse, lo persigue, si no, se queda idle

        rootNode = qCanMove;//CanWatchPlayer
    }

    protected override void InitFSM()
    {
        fsm = new FSM<MAIEnum>();


        var leaderBehaviour = GetComponent<LeaderBehaviour>();
        var flocking = GetComponent<FlockingManager>();
        var predatorBehaviour = GetComponent<PredatorBehaviour>();
        var pathFindingBehaviour = GetComponent<PathfindingBehaviour>();

        var stateList = new List<AISBase<MAIEnum>>();

        var idle = new AISIdle<MAIEnum>();
        var chase = new FAISChase<MAIEnum>(flocking, flyModel.Target, leaderBehaviour);
        var evade = new FAISEvade<MAIEnum>(flocking, flyModel.Target, leaderBehaviour, predatorBehaviour);
        var runAway = new FAISRunAway<MAIEnum>(flocking, flyModel.safeSpot.position, leaderBehaviour, pathFindingBehaviour);

        idle.AddTransition(MAIEnum.Chase, chase);
        idle.AddTransition(MAIEnum.Evade, evade);
        idle.AddTransition(MAIEnum.RunAway, runAway);

        evade.AddTransition(MAIEnum.Idle, idle);
        evade.AddTransition(MAIEnum.Chase, chase);
        evade.AddTransition(MAIEnum.RunAway, runAway);


        chase.AddTransition(MAIEnum.RunAway, runAway);
        chase.AddTransition(MAIEnum.Idle, idle);
        chase.AddTransition(MAIEnum.Evade, evade);

        runAway.AddTransition(MAIEnum.Idle, idle);
        runAway.AddTransition(MAIEnum.Chase, chase);
        


        fsm.SetInit(idle);


        stateList.Add(idle);
        stateList.Add(chase);
        stateList.Add(evade);
        stateList.Add(runAway);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight, this, obstacleAvoidance, path);
        }
    }

    private bool QIsOnDeadZone()
    {
        return GameManager.Instance.playerIsOnDeathZone;
    }
    
    private bool QPlayerHasKey()
    {
        return GameManager.Instance.playerHasKey;
    }   

    private bool QIsOnPathFinding()
    {
        //print(path.isOnPathfinding);
        //return path.isOnPathfinding;
        return (transform.position - flyModel.safeSpot.position).magnitude > 3f;
    }
    protected override void InitSteering()
    {
        
    }

    protected override void ExecuteFSM()
    {
        fsm.OnExecute();
    }
}
