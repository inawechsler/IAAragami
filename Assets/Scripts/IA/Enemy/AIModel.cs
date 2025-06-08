using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class AIModel : MonoBehaviour, IMove, ILook, IAttack, IPath
{


    [Header("Movement")]
    [HideInInspector] public float moveSpeed = 2f;

    [Header("Waypoints/patrol")]
    public List<PatrolPoint> waypoints = new List<PatrolPoint>();
    Coroutine waitOnIdleCor;
    private bool _hasToWaitOnIdle;
    public float waitOnIdleTime = 3f;
    public Action onPatrolCompleted { get; set; }
    public Action waitOnIdleAction { get; set; }


    public PatrolRandom patrolRoute;

    private ObstacleAvoidance _obs;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;

    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;
    public LayerMask obsMask;
    [HideInInspector] public bool hasLostRecently;
    public Action onLostSight;
    public Action onSightAcheived;
    protected float lostSightDuration;
    public float attackRange;
    Coroutine lostSightCor;


    [Header("Components")]
    Rigidbody rb;
    public Action onAttack { get; set; }
    public Transform Position { get; set; }
    protected bool _lastAttackHit = false;
    public Action onHitPlayer { get; set; }
    public bool isFinishPath { get; set; } = true;
    public bool isOnPathfinding { get; set; } = false;

    protected virtual void Awake()
    {
 
        patrolRoute = GetComponent<PatrolRandom>();
        
        rb = GetComponent<Rigidbody>();
        Position = transform;
        _obs = GetComponent<ObstacleAvoidance>();
        onLostSight += ManageLostSight;
        waitOnIdleAction += ManageWaitOnIdle;
        onPatrolCompleted += SetNewPatrolRoute;
    }
    public void Move(Vector3 input)
    {

        LookDir(input);
        input = _obs.GetDir(input);
        input *= moveSpeed;
        input.y = rb.linearVelocity.y;

        rb.linearVelocity = input;
    }
    public void LookDir(Vector3 inputDir)
    {
        inputDir.Normalize();
        inputDir.y = 0;

        // verifico si la dirección cambió
        bool directionChanged = (targetDirection == Vector3.zero) ||
                                (Vector3.Dot(targetDirection, inputDir) < 0.966f); // 15 grados masomenos

        if (directionChanged)
        {
            //nueva dirección objetivo
            targetDirection = inputDir;


            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            rotationCoroutine = StartCoroutine(RotateToTarget());
        }

    }

    public void SetNewPatrolRoute()
    {
        if (patrolRoute != null)
        {
            waypoints = patrolRoute.SetRoutes();
        }
    }
    public void Attack()
    {
        onAttack?.Invoke();//Al ser llamado invoca onAttack
    }

    public bool LastAttackHit()
    {
        return _lastAttackHit;
    }
    private IEnumerator RotateToTarget()
    {
        // Mientras no estemos cerca de la dirección objetivo
        while (Vector3.Dot(transform.forward, targetDirection) < 0.996f)
        {
            // roto hacia la dirección objetivo
            transform.forward = Vector3.Slerp(
                transform.forward,
                targetDirection,
                Time.deltaTime * rotationSpeed);

            yield return null;
        }

        rotationCoroutine = null;
    }
    private void ManageLostSight()//Corrutina encargada de setear el bool que se lee desede la Question qHasLostRecently en Controller
    {

        if (lostSightCor != null)
        {
            StopCoroutine(HasLostSightRecently());
        }
        lostSightCor = StartCoroutine(HasLostSightRecently());
    }

    private IEnumerator HasLostSightRecently()
    {
        hasLostRecently = true;
        yield return new WaitForSeconds(lostSightDuration);// tiempo que tarda en volver a patrulla
        isOnPathfinding = true; // Marcar que está en pathfinding
        hasLostRecently = false;
        lostSightCor = null;
    }

    private void ManageWaitOnIdle() //Corrutina encargada de setear bool que lee la pregunta QHasToWait en Controller
    {
        if (waitOnIdleCor != null)
        {
            StopCoroutine(WaitOnIdle());
        }
        waitOnIdleCor = StartCoroutine(WaitOnIdle());
    }

    private IEnumerator WaitOnIdle()
    {
        _hasToWaitOnIdle = true;
        yield return new WaitForSeconds(waitOnIdleTime);
        _hasToWaitOnIdle = false;
        waitOnIdleCor = null;
    }

    public bool GetHasToWaitOnIdle() { return _hasToWaitOnIdle; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / -2, 0) * transform.forward * range);

    }

    public Vector3 CalculateMovementDirection()
    {
        return Vector3.zero;
    }
}