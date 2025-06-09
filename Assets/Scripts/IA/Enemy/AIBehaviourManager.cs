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

    [Header("Waypoints/patrol")]
    public List<PatrolPoint> waypoints = new List<PatrolPoint>();
    public float waitOnIdleTime = 3f;

    public PatrolRandom patrolRandom { get; private set; }
    public Action waitOnIdleAction { get; set; }

    [HideInInspector] public bool hasLostRecently;
    public Action onLostSight;
    public Action onSightAcheived;
    [SerializeField] protected float lostSightDuration;
    Coroutine lostSightCor;
    private void Awake()
    {
        onLostSight += ManageLostSight;
        waitOnIdleAction += ManageWaitOnIdle;
        patrolRandom = GetComponent<PatrolRandom>();
    }
    private void ManageLostSight()
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
        yield return new WaitForSeconds(lostSightDuration);
        isOnPathfinding = true;
        hasLostRecently = false;
        lostSightCor = null;
    }

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

}
