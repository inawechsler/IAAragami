using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

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
    protected abstract void ExecuteFSM();
    //Estos tres se definen en las clases hijas
    protected abstract void InitFSM();
    protected abstract void InitTree();
    protected abstract void InitSteering();

}