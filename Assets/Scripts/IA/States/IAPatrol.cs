//using NUnit.Framework;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class AISPatrol<T> : AISBase<T>
//{
//    //private List<> waypoints;
//    private AIController enemy;
//    private AIModel model;
//    private int currentWaypointIndex = 0;
//    private bool isWaiting = false;

//    private Coroutine waitCoroutine;

//    public IAPatrol(AIController enemyController, AIModel model)
//    {
//        this.enemy = enemyController;
//        this.model = model;
//    }

//    public void Update()
//    {
        
//    }

    

//    private IEnumerator WaitAndMoveToNext()
//    {
//        float waitTime = Random.Range(enemy.minWaitTime, enemy.maxWaitTime);
//        yield return new WaitForSeconds(waitTime);

//        currentWaypointIndex = (currentWaypointIndex + 1) % enemy.waypoints.Count;
//        isWaiting = false;
//    }
//    public override void Execute()
//    {
//        base.Execute();
//        isWaiting = false;
//        currentWaypointIndex = 0;
       

//        if (enemy.waypoints.Count == 0 || isWaiting) return;

//        Transform target = enemy.waypoints[currentWaypointIndex];
//        Vector3 direction = (target.position - enemy.transform.position).normalized;
//        move.Move(direction);


//        if (direction != Vector3.zero)
//        {
//            look.LookDir(direction);
//        }

//        if (Vector3.Distance(enemy.transform.position, target.position) < enemy.stopDistance)
//        {
//            isWaiting = true;
//            waitCoroutine = enemy.StartCoroutine(WaitAndMoveToNext());
//        }

//    }
//    public override void Exit()
//    {
//        if (waitCoroutine != null)
//        {
//            enemy.StopCoroutine(waitCoroutine);
//            waitCoroutine = null;
//        }
//    }
//}
