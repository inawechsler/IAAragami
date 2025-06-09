using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIBehaviourManager : MonoBehaviour, IPath
{
    Coroutine waitOnIdleCor;
    private bool _hasToWaitOnIdle;
    public bool isFinishPath { get; set; } = true;
    public bool isOnPathfinding { get; set; } = false;
    public int lostSighDuration;

    [Header("Waypoints/patrol")]
    public List<PatrolPoint> waypoints = new List<PatrolPoint>();
    public float waitOnIdleTime = 3f;

    public PatrolRandom patrolRoute;
    public Action waitOnIdleAction { get; set; }

    [HideInInspector] public bool hasLostRecently;
    public Action onLostSight;
    public Action onSightAcheived;
    protected float lostSightDuration;
    Coroutine lostSightCor;
    private void Awake()
    {
        patrolRoute = GetComponent<PatrolRandom>();

        onLostSight += ManageLostSight;// esto no tiene q estar en el controller?
        waitOnIdleAction += ManageWaitOnIdle;
    }
    private void ManageLostSight()//Corrutina encargada de setear el bool que se lee desede la Question qHasLostRecently en Controller
    {

        if (lostSightCor != null)
        {
            StopCoroutine(HasLostSightRecently());
        }
        lostSightCor = StartCoroutine(HasLostSightRecently());
    }

    private IEnumerator HasLostSightRecently()
    {
        hasLostRecently = true;
        yield return new WaitForSeconds(lostSightDuration);// tiempo que tarda en volver a patrulla
        isOnPathfinding = true; // Marcar que está en pathfinding
        hasLostRecently = false;
        lostSightCor = null;
    }

    private void ManageWaitOnIdle() //Corrutina encargada de setear bool que lee la pregunta QHasToWait en Controller
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

}
