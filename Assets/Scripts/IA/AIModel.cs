using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AIModel : MonoBehaviour, IMove, ILook, IAttack
{

    [Header("Animation")]
    [SerializeField] private Collider attackCollider;
    private bool _lastAttackHit = false;


    [Header("Movement")]
    public float moveSpeed;

    [Header("Waypoints/patrol")]
    public List<PatrolPoint> waypoints = new List<PatrolPoint>();



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

    [Header("Components")]
    Rigidbody rb;
    Coroutine lastAttackHitSCor;
    public Transform Position { get; set; }

    public Action onAttack { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Position = transform;
    }
    public void Move(Vector3 input)
    {
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
