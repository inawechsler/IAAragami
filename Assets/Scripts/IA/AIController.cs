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
    private LineOfSight LineOfSight;
    public AIModel model { get; private set; }
    [SerializeField] public Transform target;
    [SerializeField] public Rigidbody rbTarget;

    [Header("Attributes")]
    [SerializeField] private float timePrediction;


    private void Awake()
    {
        look = GetComponent<ILook>();
        attack = GetComponent<IAttack>();
        move = GetComponent<IMove>();
        InitSteering();
        InitTree();
        InitFSM();
        LineOfSight = new LineOfSight();
        model = GetComponent<AIModel>();

    }
    private void Update()
    {
        fsm.OnExecute();
        rootNode.Execute();


        if (Input.GetKeyDown(KeyCode.J))
        {
            steering = steering == pursuit ? evade : pursuit;
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


        var qHitTarget = new QuestionNode(QHitTarget, idleAct, chaseAct);
        var attackAndCheckHit = new SequenceNode(new List<ITreeNode>
        {
            attackAct,
            new WaitNode(1f),
            qHitTarget
        });
        var qCanAttack = new QuestionNode(QPlayerInRange, attackAndCheckHit, chaseAct);
        var qCanWatchPlayer = new QuestionNode(QLineOfSight, qCanAttack, idleAct);
        var qStayOnIdle = new SequenceNode(new List<ITreeNode>
        {
            idleAct,
            new WaitNode(2f),
            qCanWatchPlayer
        });

        rootNode = qCanWatchPlayer;
    }
    void InitFSM()
    {
        fsm = new FSM<AIEnum>();

        var stateList = new List<AISBase<AIEnum>>();
        var idleSt = new AISIdle<AIEnum>(2f);
        var chaseSt = new AISSteering<AIEnum>(steering);
        var attackSt = new AISAttack<AIEnum>();

        idleSt.AddTransition(AIEnum.Chase, chaseSt);
        idleSt.AddTransition(AIEnum.Attack, attackSt);

        chaseSt.AddTransition(AIEnum.Attack, attackSt);
        chaseSt.AddTransition(AIEnum.Idle, idleSt);

        attackSt.AddTransition(AIEnum.Idle, idleSt);
        attackSt.AddTransition(AIEnum.Chase, chaseSt);



        stateList.Add(idleSt);
        stateList.Add(chaseSt);
        stateList.Add(attackSt);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, attack, LineOfSight);
        }

        fsm.SetInit(idleSt);
    }

    private bool QHitTarget()
    {
        if (attack.LastAttackHit())
        {
            return true;
        }
        else
        {

            return false;
        }
    }

    //private bool qKeepResting()
    //{

    //    return idleState.isResting;

    //} 
    private bool QPlayerInRange()
    {
        if (LineOfSight.CheckRange(transform, target, model.attackRange))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool QLineOfSight()
    {
        if (LineOfSight.LOS(transform, target, model.range, model.angle, model.obsMask))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

}