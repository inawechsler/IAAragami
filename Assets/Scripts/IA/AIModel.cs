using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class AIModel : MonoBehaviour, IMove
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    float stopDistance = 0.2f;
    [SerializeField] List<Transform> waypoints;
    int currentWaypointIndex;

    [Header("Components")]
    Rigidbody rb;
    public Transform Position { get; set; }

    // Start is called before the first frame update
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

    public Vector3 CalculateMovementDirection()
    {
        //    // crear el vector de movimiento usando el input
        //    // moveInput.y positivo (W) = adelante
        //    // moveInput.x positivo (D) = derecha
        //    Vector3 movement = Position.forward * -inputController.moveInput.y +
        //        Position.right * -inputController.moveInput.x;

          return Vector3.zero;
    }

    //public void WaypointsPatrol()
    //{
    //    if (waypoints.Count == 0) return;

    //    Transform targetWaypoint = waypoints[currentWaypointIndex];
    //    Vector3 direction = (targetWaypoint.position - transform.position).normalized;
    //    transform.position += direction * moveSpeed * Time.deltaTime;

    //    // Rota hacia el waypoint
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    //    }

    //    // Verifica si llegó al waypoint
    //    if (Vector3.Distance(transform.position, targetWaypoint.position) < stopDistance)
    //    {
    //        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    for (int i = 0; i < waypoints.Count; i++)
    //    {
    //        if (waypoints[i] != null)
    //        {
    //            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

    //            if (i + 1 < waypoints.Count && waypoints[i + 1] != null)
    //            {
    //                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
    //            }
    //        }
    //    }
    //}
}
