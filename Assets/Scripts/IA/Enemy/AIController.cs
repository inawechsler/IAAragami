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


    [Header("References")]
    [SerializeField] public Transform target;
    [SerializeField] public Rigidbody rbTarget;

    [Header("Attributes")]
    protected float timePrediction = 2f;
    protected virtual void Awake()
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
        if (target != null && rootNode != null)
        {
            ExecuteFSM(); 
            rootNode.Execute();
        }

    }
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
    protected abstract void InitFSM(); //Son abstractas porque cada AI tiene su propio FSM, ac� declaro la funci�n pero no la implemento y lo mismo en las tres de abajo
    protected abstract void ExecuteFSM();
    protected abstract void InitTree();
    protected abstract void InitSteering();

}