using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class LineOfSightMono : MonoBehaviour 
{
    [SerializeField] private Transform headPos;

    private bool wasInRange = false;
    private bool eventFired = false;
    private bool gainSightEventFired = false;

    public bool CheckRange(Transform target, float rangle)
    {
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance <= rangle;
    }

    public bool CheckAngle(Transform target, float angle)
    {
        return CheckAngle(target, transform.forward, angle);
    }

    public bool CheckAngle(Transform target, Vector3 front, float angle)
    {
        Vector3 dir = target.position - transform.position;
        float angleToTarget = Vector3.Angle(front, dir);
        return angleToTarget < angle / 2;
    }
    public bool CheckView(Transform target, LayerMask obsMask)
    {
        Vector3 dir = target.position - transform.position;

        //Primero tira ray normal (altura de pies)
        if (!Physics.Raycast(transform.position, dir.normalized, dir.magnitude, obsMask)) //Lo niego porque el ray devuelve verdadero cuando choca, 
            return true;                                                                  //nosotros queremos que sea verdadero cuando nada lo bloquea.
        
        
        //Dibujar rayo desde cabeza del jugador para saber si no est� parado detr�s de un obst�culo bajo
        Vector3 headPlayer = headPos.transform.position; // Altura de la cabeza del PJ

        Vector3 headPosition = transform.position + Vector3.up * 1.8f; // Altura aproximada de una cabeza
        Vector3 dirFromHead = headPlayer - headPosition;
        return !Physics.Raycast(headPosition, dirFromHead.normalized, dirFromHead.magnitude, obsMask);
        

    }

    public bool LOS(Transform self, Transform target, float range, float angle, LayerMask obsMask, AIModel model)
    {
        bool inSight = CheckRange(target, range) && CheckAngle(target, angle) && CheckView(target, obsMask);

        if (wasInRange && !inSight && !eventFired)
        {
            model.onLostSight?.Invoke();
            eventFired = true; // evita que el evento se vuelva a ejecutar
        }
        if (!wasInRange && inSight && !gainSightEventFired)
        {
            model.onSightAcheived?.Invoke();
            gainSightEventFired = true; // evita que el evento se vuelva a ejecutar
        }

        // si vuelve a estar en rango, resetea el flag para que pueda dispararse de nuevo cuando salga del rango
        if (inSight)
        {
            wasInRange = true;
            eventFired = false;
        }
        else
        {
            wasInRange = false;
        }
        //Debug.Log("LOS:" + wasInRange);
        return inSight;
    }
}
