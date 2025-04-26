using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIController : MonoBehaviour
{
    [Header("Components")]
    private FSM<AIEnum> fsm;
    private ILook look;
    private IMove move;
    private IAttack attack;
    private ITreeNode rootNode;
    public ISteering steering;
    ISteering pursuit;
    ISteering evade;
    private LineOfSightMono LineOfSight;
    private ObstacleAvoidance obstacleAvoidance;
    public AIModel model { get; private set; }
    [SerializeField] public Transform target;
    [SerializeField] public Rigidbody rbTarget;

    [Header("Attributes")]
    private float timePrediction = 2f;


    private void Awake()
    {
        obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        look = GetComponent<ILook>();
        attack = GetComponent<IAttack>();
        move = GetComponent<IMove>();
        LineOfSight = GetComponent<LineOfSightMono>();
        model = GetComponent<AIModel>();

    }
    private void Start()
    {
        InitSteering();
        InitFSM();
        InitTree();
    }
    private void Update()
    {
        if (target != null && fsm != null && rootNode != null)
        {
            fsm.OnExecute();
            rootNode.Execute();
        }

    }

    void InitSteering()
    {
        pursuit = new Pursuit(transform, rbTarget, timePrediction);
        evade = new Evade(transform, rbTarget, timePrediction);

        steering = pursuit;

    }

    void InitTree()
    {
        var chaseAct = new ActionNode(() => fsm.Transition(AIEnum.Chase));
        var idleAct = new ActionNode(() => fsm.Transition(AIEnum.Idle));
        var attackAct = new ActionNode(() => fsm.Transition(AIEnum.Attack));
        var evadeAct = new ActionNode(() => fsm.Transition(AIEnum.Evade));
        var patrolAct = new ActionNode(() => fsm.Transition(AIEnum.Patrol));
        //var qStayOnIdle = new SequenceNode(new List<ITreeNode>
        //{
        //    idleAct,
        //    new WaitNode(1f),
        //    new ActionNode(()=> Debug.Log("False 2"))
        //});
        //var attackAndCheckHit = new SequenceNode(new List<ITreeNode>
        //{
        //    attackAct,
        //    new WaitNode(1f),
        //    qHitTarget
        //});


        var qHitTarget = new QuestionNode(QHitTarget, idleAct, attackAct);//Cambiar para entrega
        var qCanAttack = new QuestionNode(QPlayerInRange, qHitTarget, chaseAct);//Chequea si puede atacar o no, true chequea si en el frame anterior conectó con el PJ
        var qHasToWaitOnPatrol = new QuestionNode(QAIHasToWait, idleAct, patrolAct);//Chequea si no debe estar en idle luego de x rondas de patrol
        var qHasLostPlayerRecently = new QuestionNode(QHasLostPlayer, chaseAct, qHasToWaitOnPatrol);//Esta pregunta funciona para que luego de perder al PJ, el enemigo siga buscando por unos segundos
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanAttack, qHasLostPlayerRecently); //Si LOS true chequea si puede atacar o no,si false, verifica si lo perdió 
                                                                                                    //de vista hace poco


        rootNode = qCanWatchPlayer;
    }
    void InitFSM()
    {
        fsm = new FSM<AIEnum>();

        var stateList = new List<AISBase<AIEnum>>();
        var idleSt = new AISIdle<AIEnum>(2f);
        var chaseSt = new AISSteering<AIEnum>(pursuit);
        var evadeSt = new AISSteering<AIEnum>(evade);
        var patrolSt = new AISPatrol<AIEnum>(model.waypoints);
        var attackSt = new AISAttack<AIEnum>(target);

        idleSt.AddTransition(AIEnum.Chase, chaseSt);
        idleSt.AddTransition(AIEnum.Attack, attackSt);
        idleSt.AddTransition(AIEnum.Evade, evadeSt);
        idleSt.AddTransition(AIEnum.Patrol, patrolSt);

        chaseSt.AddTransition(AIEnum.Attack, attackSt);
        chaseSt.AddTransition(AIEnum.Idle, idleSt);
        chaseSt.AddTransition(AIEnum.Evade, evadeSt);
        chaseSt.AddTransition(AIEnum.Patrol, patrolSt);

        evadeSt.AddTransition(AIEnum.Idle, idleSt);
        evadeSt.AddTransition(AIEnum.Chase, chaseSt);

        attackSt.AddTransition(AIEnum.Idle, idleSt);
        attackSt.AddTransition(AIEnum.Chase, chaseSt);
        attackSt.AddTransition(AIEnum.Evade, evadeSt);
        attackSt.AddTransition(AIEnum.Patrol, patrolSt);

        patrolSt.AddTransition(AIEnum.Idle, idleSt);
        patrolSt.AddTransition(AIEnum.Chase, chaseSt);
        patrolSt.AddTransition(AIEnum.Attack, attackSt);



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
        return attack.LastAttackHit();
    }

    private bool QPlayerInRange()
    {
        return LineOfSight.CheckRange(target, model.attackRange);

    }
    private bool QLineOfSight()
    {
        return LineOfSight.LOS(transform, target, model.range, model.angle, model.obsMask);
    }

}