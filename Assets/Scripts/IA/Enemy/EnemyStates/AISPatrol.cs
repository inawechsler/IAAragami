using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISPatrol<T> : AISBase<T>
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
    private MonoBehaviour monoRef;
    private PatrolRandom patrolRandom;

    public AISPatrol(List<PatrolPoint> waypoints, MonoBehaviour monoBehaviourRef, PatrolRandom patrolRandom)
    {
        monoRef = monoBehaviourRef;
        waypoints = _waypoints;
        this.patrolRandom = patrolRandom;
    }

    public override void Enter()
    {
        base.Enter();

        if (patrolRandom != null)
        {
            patrolRandom.MarkLastPathCompleted(out _waypoints);
        }
        _isWaiting = false;
        _currentWaypointIndex = 0;
    }

    public override void Execute()
    {
        base.Execute();

        if (_waypoints.Count == 0 || _isWaiting) return;

        Vector3 target = _waypoints[_currentWaypointIndex].Position; //Posición del waypoint
        Vector3 direction = (target - controller.transform.position).normalized; //Dirección hacia el waypoint
        move.Move(direction);

        if (Vector3.Distance(controller.transform.position, target) < _stopDistance)//Si la dist es menor a la stopDistance
        {
            _isWaiting = true; //Esperando en el wp true
            monoRef.StartCoroutine(WaitAndMoveToNext()); //Inicia la corrutina para esperar  
        }
    }

    public IEnumerator WaitAndMoveToNext()
    {
        float waitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime); //Random en espera
        yield return new WaitForSeconds(waitTime); //Esperar el tiempo random

        if (_currentWaypointIndex == _waypoints.Count - 1) //Si el index es igual al ultimo waypoint
        {
            if(patrolRandom != null)
            {
                patrolRandom.MarkLastPathCompleted(out _waypoints);
            }
            _lapsCompleted++;
            if (_lapsCompleted == _lapsToWaitOnIdle) //Si está en la vuelta para esperar
            {
                _lapsCompleted = 0;
                controller.behaviourManager.waitOnIdleAction?.Invoke(); //Espera en idle
            }
        }
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        _isWaiting = false;
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
