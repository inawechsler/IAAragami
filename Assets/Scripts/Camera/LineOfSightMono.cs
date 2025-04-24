using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class LineOfSightMono : MonoBehaviour 
{
    public float range;
    public float angle;
    public LayerMask obsMask;

    public bool CheckRange(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance <= range;
    }

    public bool CheckAngle(Transform target)
    {
        return CheckAngle(target, transform.forward);
    }

    public bool CheckAngle(Transform target, Vector3 front)
    {
        Vector3 dir = target.position - transform.position;
        float angleToTarget = Vector3.Angle(front, dir);
        return angleToTarget < angle / 2;
    }
    public bool CheckView(Transform target, float enemyHeight = 10)
    {
        Vector3 dir = target.position - transform.position;

        var isLOSfromFeet = !Physics.Raycast(transform.position, dir.normalized, dir.magnitude, obsMask);
        // Primer intento: Desde la posición original
        if (isLOSfromFeet)
        {
            return true;
        } else
        {
            // Dibujar rayo desde posición original al jugador
            Vector3 headPlayer = target.position + Vector3.up * 1.8f; // Altura aproximada de una cabeza

            Vector3 headPosition = transform.position + Vector3.up * 1.8f; // Altura aproximada de una cabeza
            Vector3 dirFromHead = headPlayer - headPosition;
            print(Physics.Raycast(headPosition, dirFromHead.normalized, dirFromHead.magnitude, obsMask));
            return Physics.Raycast(headPosition, dirFromHead.normalized, dirFromHead.magnitude, obsMask);
        }

    }

    public bool LOS(Transform self, Transform target, float range, float angle, LayerMask obsMask)
    {
        return CheckRange(target)
            && CheckAngle(target)
            && CheckView(target);
    }
}
