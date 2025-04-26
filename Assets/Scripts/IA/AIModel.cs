using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AIModel : MonoBehaviour, IMove, ILook, IAttack
{

    [Header("Attack")]
    [SerializeField] private Collider attackCollider;
    private bool _lastAttackHit = false;
    public Action onAttack { get; set; }
    Coroutine lastAttackHitSCor;



    [Header("Movement")]
    public float moveSpeed;

    [Header("Waypoints/patrol")]
    public List<PatrolPoint> waypoints = new List<PatrolPoint>();
    Coroutine waitOnIdleCor;
    private bool _hasToWaitOnIdle;
    public float waitOnIdleTime = 3f;
    public Action waitOnIdleAction { get; set; }

    private ObstacleAvoidance _obs;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;

    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;
    public float attackRange;
    public LayerMask obsMask;
    public bool hasLostRecently;
    public Action onLostSight;
    private float lostSightDuration = 3f;
    Coroutine lostSightCor;


    [Header("Components")]
    Rigidbody rb;
    public Transform Position { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Position = transform;
        _obs = GetComponent<ObstacleAvoidance>();
        onLostSight += ManageLostSight;
        waitOnIdleAction += ManageWaitOnIdle;
    }
    public void Move(Vector3 input)
    {

        LookDir(input);
        input = _obs.GetDir(input);
        input *= moveSpeed;
        input.y = rb.linearVelocity.y;

        rb.linearVelocity = input;
    }

    public void EnableAttackCollider()
    {
        _lastAttackHit = false; // Reiniciar el estado del hit
        attackCollider.isTrigger = true;
        attackCollider.enabled = true;
    }

    // Este método se llama desde el Animation Event
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            if (lastAttackHitSCor == null)
                StopCoroutine(lastAttackHitSet());

            lastAttackHitSCor = StartCoroutine(lastAttackHitSet());
        }
    }

    private IEnumerator lastAttackHitSet()
    {
        _lastAttackHit = true;
        yield return new WaitForSeconds(.5f);
        _lastAttackHit = false;

        lastAttackHitSCor = null;
    }
    public bool LastAttackHit()
    {
        return _lastAttackHit;
    }

    private void ManageLostSight()
    {

        if (lostSightCor != null)
        {
            StopCoroutine(HasLostSightRecently());
        }
        lostSightCor = StartCoroutine(HasLostSightRecently());

        Debug.Log("Event:" + hasLostRecently);
    }

    private IEnumerator HasLostSightRecently()
    {
        //if (hasLostRecently) yield return null;
        hasLostRecently = true;
        yield return new WaitForSeconds(lostSightDuration);
        hasLostRecently = false;
        lostSightCor = null;
    }

    public bool GetHasLostSighRecently() { return hasLostRecently; }

    private void ManageWaitOnIdle()
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

    public void LookDirWithLerp(Vector3 target, float speed)
    {
        target.y = 0;
        // roto hacia la dirección objetivo
        transform.forward = Vector3.Slerp(
            transform.forward,
            target,
            Time.deltaTime * speed);
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

    public void Attack()
    {
        onAttack?.Invoke();
    }
}