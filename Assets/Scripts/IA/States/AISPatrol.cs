using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AISPatrol<T> : AISBase<T>
{
    private List<PatrolPoint> waypoints = new();
    private AIController _enemy;
    private AIModel _model;
    public int _currentWaypointIndex = 0;
    private bool _isWaiting = false;
    private float stopDistance = 1f;
    private float minWaitTime = .2f;
    private float maxWaitTime = 1f;
    private Coroutine waitCoroutine;
    public AISPatrol(List<PatrolPoint> patrolPoints)
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

        Debug.Log(_currentWaypointIndex + "WP: " + waypoints[_currentWaypointIndex].gameObject.name);

        look.LookDir(direction);


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

        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
        _isWaiting = false;
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
