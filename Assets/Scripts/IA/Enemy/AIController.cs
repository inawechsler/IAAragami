using UnityEngine;

public abstract class AIController : MonoBehaviour
{
    [Header("Base Components")]
    protected ILook look;
    protected IMove move;
    protected IAttack attack;
    protected ITreeNode rootNode;
    public ISteering steering;
    protected LineOfSightMono LineOfSight;
    protected ObstacleAvoidance obstacleAvoidance;
    [HideInInspector] public AIModel model;

    //public MeleeController controller;

    //public AISPathfindind<Vector3> pathF;

    [Header("References")]
    [HideInInspector] public Transform target;
    [HideInInspector] public Rigidbody rbTarget;

    [Header("Attributes")]
    protected float timePrediction = 1f;
    protected virtual void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        rbTarget = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        obstacleAvoidance = GetComponent<ObstacleAvoidance>();
        look = GetComponent<ILook>();
        attack = GetComponent<IAttack>();
        move = GetComponent<IMove>();
        LineOfSight = GetComponent<LineOfSightMono>();
        model = GetComponent<AIModel>();

        //controller = GetComponent<MeleeController>();
        //pathF = GetComponent<AISPathfindind<Vector3>>();
    }
    private void Start()
    {
        InitSteering();
        InitFSM();
        InitTree();
    }
    private void Update()
    {
        if (target != null && rootNode != null)
        {
            ExecuteFSM(); 
            rootNode.Execute();
        }

    }

    protected bool QIsFarFromInitialWaypoint()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = model.waypoints[0].position;

        return Vector3.Distance(currentPos, targetPos) > 1.25f;
    }
    //protected bool QIAInPath()
    //{
    //    if (target.transform.position - transform.position <= pathF._distanceToPoint)
    //    return;
    //}

    protected bool QAIHasToWait()
    {
        return model.GetHasToWaitOnIdle();
    }
    protected bool QHasLostPlayer()
    {
        return model.hasLostRecently; //Hay un get en el model pero no me funciona
    }
    protected bool QPlayerInRange()
    {
        return LineOfSight.CheckRange(target, model.attackRange);

    }
    protected bool QLineOfSight()
    {
        return LineOfSight.LOS(transform, target, model.range, model.angle, model.obsMask, model);
    }
    protected abstract void InitFSM(); //Son abstractas porque cada AI tiene su propio FSM, acá declaro la función pero no la implemento y lo mismo en las tres de abajo
    protected abstract void ExecuteFSM();
    protected abstract void InitTree();
    protected abstract void InitSteering();

}