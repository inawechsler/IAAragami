using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPatrol : MonoBehaviour, ILook, IMove
{
    float moveSpeed = 2f;
    Rigidbody rb;

    float rotationSpeed = 0;
    private Coroutine rotationCoroutine;
    private Vector3 targetDirection;

    float stopDistance = 0.2f;
    bool isWaiting = false;
    float minWaitTime;
    float maxWaitTime;
    public Transform Position { get; set; }


    [SerializeField] List<Transform> waypoints;
    int currentWaypointIndex;

    //Update is called once per frame
    void Update()
    {
        WaypointsPatrol();
    }

    public void Move(Vector3 input)
    {
        input *= moveSpeed;
        input.y = rb.linearVelocity.y;

        rb.linearVelocity = input;
    }
    public Vector3 CalculateMovementDirection()
    {
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

    public void WaypointsPatrol() 
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        //Move(direction);

        // Rota hacia el waypoint
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            //LookDir(direction);
        }

        // Verifica si llegó al waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < stopDistance)
        {

            //WaitBeforeNextWaypoint();
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
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

    IEnumerator WaitBeforeNextWaypoint()
    {
        isWaiting = true;

        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        isWaiting = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                if (i + 1 < waypoints.Count && waypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }

}
