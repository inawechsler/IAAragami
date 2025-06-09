using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class AIModel : MonoBehaviour, IMove, ILook, IAttack
{

    [Header("Movement")]
    [HideInInspector] public float moveSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;

    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;
    public LayerMask obsMask;

    [Header("Components")]
    Rigidbody rb;
    public Action onAttack { get; set; }
    public float attackRange;

    protected AIBehaviourManager behaviourManager;
    public Transform SelfPosition { get; set; }
    protected bool _lastAttackHit = false;
    public Action onHitPlayer { get; set; }
    private ObstacleAvoidance _obs;

    protected virtual void Awake()
    {
        _obs = GetComponent<ObstacleAvoidance>();
        rb = GetComponent<Rigidbody>();
        SelfPosition = transform;
        behaviourManager = GetComponent<AIBehaviourManager>();
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