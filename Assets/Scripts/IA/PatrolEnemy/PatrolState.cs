//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PatrolState : IState<T>
//{
//    private AIController enemy;
//    private int currentWaypointIndex = 0;
//    private bool isWaiting = false;

//    private Coroutine waitCoroutine;

//    public PatrolState(AIController enemyController)
//    {
//        this.enemy = enemyController;
//    }

//    public void Enter()
//    {
//        isWaiting = false;
//        currentWaypointIndex = 0;
//    }

//    public void Update()
//    {
//        if (enemy.waypoints.Count == 0 || isWaiting) return;

//        Transform target = enemy.waypoints[currentWaypointIndex];
//        Vector3 direction = (target.position - enemy.transform.position).normalized;
//        enemy.transform.position += direction * AIModel.moveSpeed * Time.deltaTime;

//        // Rota hacia el waypoint
//        if (direction != Vector3.zero)
//        {
//            Quaternion lookRotation = Quaternion.LookRotation(direction);
//            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
//        }

//        if (Vector3.Distance(enemy.transform.position, target.position) < enemy.stopDistance)
//        {
//            isWaiting = true;
//            waitCoroutine = enemy.StartCoroutine(WaitAndMoveToNext());
//        }
//    }

//    public void Exit()
//    {
//        if (waitCoroutine != null)
//        {
//            enemy.StopCoroutine(waitCoroutine);
//            waitCoroutine = null;
//        }
//    }

//    private IEnumerator WaitAndMoveToNext()
//    {
//        float waitTime = Random.Range(enemy.minWaitTime, enemy.maxWaitTime);
//        yield return new WaitForSeconds(waitTime);

//        currentWaypointIndex = (currentWaypointIndex + 1) % enemy.waypoints.Count;
//        isWaiting = false;
//    }
//}
