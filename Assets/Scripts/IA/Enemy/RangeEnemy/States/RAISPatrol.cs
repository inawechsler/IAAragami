using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//RAISAttack = Range AI State Attack 

public class RAISPatrol<T> : AISBase<T>
{
    private List<PatrolPoint> waypoints = new();
    public int _currentWaypointIndex = 0;
    private bool _isWaiting = false;
    private float stopDistance = 1f;
    private float minWaitTime = 1f;
    private float maxWaitTime = 1.3f;
    private int lapsCompleted = 0;
    private int lapsToWaitOnIdle = 2;
    private Coroutine waitCoroutine;
    public RAISPatrol(List<PatrolPoint> patrolPoints)
    {   
        waypoints = patrolPoints;
    }
    
    public override void Enter()
    {
        base.Enter();
        _isWaiting = false;
        _currentWaypointIndex = 0;
    }

    public override void Execute()
    {
        base.Execute();

        if (waypoints.Count == 0 || _isWaiting) return;

        Vector3 target = waypoints[_currentWaypointIndex].Position;
        Vector3 direction = (target - controller.transform.position).normalized;
        move.Move(direction);


        if (Vector3.Distance(controller.transform.position, target) < stopDistance)
        {
            _isWaiting = true;
            controller.StartCoroutine(WaitAndMoveToNext());
        }
    }
    public IEnumerator WaitAndMoveToNext()
    {
        float waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);


        if (_currentWaypointIndex == waypoints.Capacity - 1)
        {
            lapsCompleted++;
            if (lapsCompleted == lapsToWaitOnIdle)
            {
                lapsCompleted = 0;
                controller.model.waitOnIdleAction?.Invoke();
            }
        }
        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
        _isWaiting = false;
        //Debug.Log("Laps Complete: " + lapsCompleted);
    }

    public override void Exit()
    {
        if (waitCoroutine != null)
        {
            controller.StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }
}
