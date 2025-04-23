using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AIModel : MonoBehaviour, IMove, ILook
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Waypoints/patrol")]
    public List<Transform> waypoints;
    public float stopDistance = 0.2f;
    public float minWaitTime =5f;
    public float maxWaitTime = 10f;


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
    public Transform Position { get; set; }

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
        //    // crear el vector de movimiento usando el input
        //    // moveInput.y positivo (W) = adelante
        //    // moveInput.x positivo (D) = derecha
        //    Vector3 movement = Position.forward * -inputController.moveInput.y +
        //        Position.right * -inputController.moveInput.x;

          return Vector3.zero;
    }


    public void LookDir(Vector3 inputDir)
    {
        inputDir.Normalize();

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


}
