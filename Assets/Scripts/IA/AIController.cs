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
    private ITreeNode rootNode;
    public ISteering steering;
    private LineOfSight LineOfSight;
    public AIModel model { get; private set; }
    public float timePrediction;
    [SerializeField] public Transform target;
    [SerializeField] public Rigidbody rbTarget;


    private void Awake()
    {
        look = GetComponent<ILook>();
        move = GetComponent<IMove>();
        InitSteering();
        InitFSM();
        InitTree();
        LineOfSight = new LineOfSight();
        model = GetComponent<AIModel>();

    }
    private void Update()
    {
        fsm.OnExecute();
        rootNode.Execute();
    }
    
    void InitSteering()
    {
        var pursuit = new Pursuit(transform, rbTarget, timePrediction);
        var evade = new Evade(transform, rbTarget, timePrediction);

        steering = pursuit;

    }

    void InitTree()
    {
        var chase = new ActionNode(() => fsm.Transition(AIEnum.Chase));
        var idle = new ActionNode(() => fsm.Transition(AIEnum.Idle));

        var qCanWatchPlayer = new QuestionNode(QLineOfSight, chase, idle);

        rootNode = qCanWatchPlayer;
        //Desde el root llegamos a los demas nodos 
    }
    void InitFSM()
    {
        fsm = new FSM<AIEnum>();

        var stateList = new List<AISBase<AIEnum>>();
        var idle = new AISIdle<AIEnum>(this);
        var walk = new AISSteering<AIEnum>(steering);

        idle.AddTransition(AIEnum.Chase, walk);
        walk.AddTransition(AIEnum.Idle, idle);


        stateList.Add(idle);
        stateList.Add(walk);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, LineOfSight);
        }

        fsm.SetInit(idle);
    }
    private bool QLineOfSight()
    {
        if (LineOfSight.LOS(transform, target, model.range, model.angle, model.obsMask))
        {
            return true;
        } else
        {

            return false;
        }
    }

}