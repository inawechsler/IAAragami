using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// MAISPatrol = Melee AI State Patrol
public class MAISPatrol<T> : AISBase<T>
{
    private List<PatrolPoint> _waypoints = new();
    private int _currentWaypointIndex = 0;
    private bool _isWaiting = false;
    private float _stopDistance = 1f;
    private float _minWaitTime = 1f;
    private float _maxWaitTime = 1.3f;
    private int _lapsCompleted = 0;
    private int _lapsToWaitOnIdle = 2;
    private Coroutine _waitCoroutine;

    public MAISPatrol(List<PatrolPoint> waypoints)
    {
        _waypoints = waypoints;
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

        if (_waypoints.Count == 0 || _isWaiting) return;

        Vector3 target = _waypoints[_currentWaypointIndex].Position;
        Vector3 direction = (target - controller.transform.position).normalized;
        move.Move(direction);

        look.LookDir(direction);

        if (Vector3.Distance(controller.transform.position, target) < _stopDistance)
        {
            _isWaiting = true;
            controller.StartCoroutine(WaitAndMoveToNext());
        }
    }

    public IEnumerator WaitAndMoveToNext()
    {
        float waitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        if (_currentWaypointIndex == _waypoints.Count - 1)
        {
            _lapsCompleted++;
            controller.model.onPatrolCompleted?.Invoke();
            _waypoints = controller.model.waypoints;
            if (_lapsCompleted == _lapsToWaitOnIdle)
            {
                _lapsCompleted = 0;
                controller.model.waitOnIdleAction?.Invoke();
            }
        }
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        _isWaiting = false;
        Debug.Log("Laps Complete: " + _lapsCompleted);
    }

    public override void Exit()
    {
        if (_waitCoroutine != null)
        {
            controller.StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
        }
    }
}
