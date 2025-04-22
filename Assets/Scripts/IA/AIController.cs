using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class AIController : MonoBehaviour
{

    private FSM<AIEnum> fsm;
    private ILook look;
    private IMove move;
    private ISteering steering;
    public float timePrediction;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rbTarget;
    private void Awake()
    {
        look = GetComponent<ILook>();
        move = GetComponent<IMove>();
        InitSteering();
        InitFSM();
    }

    void InitSteering()
    {
        var pursuit = new Pursuit(transform, rbTarget, timePrediction);
        var evade = new Evade(transform, rbTarget, timePrediction);


        steering = evade;
    }
    
    void InitFSM()
    {
        fsm = new FSM<AIEnum>();

        var stateList = new List<PSBase<AIEnum>>();
        var idle = new AISIdle<AIEnum>(AIEnum.Chase);
        var walk = new AISSteering<AIEnum>(steering, AIEnum.Idle);

        idle.AddTransition(AIEnum.Chase, walk);
        walk.AddTransition(AIEnum.Idle, idle);

        stateList.Add(idle);
        stateList.Add(walk);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm);
        }

        fsm.SetInit(idle);
    }

    private void Update()
    {
        fsm.OnExecute();
    }
}