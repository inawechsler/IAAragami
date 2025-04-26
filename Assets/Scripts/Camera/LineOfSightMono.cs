using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class LineOfSightMono : MonoBehaviour 
{
    public float range;
    public float angle;
    [SerializeField] private Transform headPos;
    public LayerMask obsMask;
    public AIModel model;

        private bool wasInRange = false;
    private bool eventFired = false;
    
    private void Awake()
    {
        model = GetComponent<AIModel>();
    }

    public bool CheckRange(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance <= range;
    }

    public bool CheckRange(Transform target, float attackRange)
    {
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance <= attackRange;
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

        //Primero tira ray normal (altura de pies)
        if (!Physics.Raycast(transform.position, dir.normalized, dir.magnitude, obsMask)) //Lo niego porque el ray devuelve verdadero cuando choca, 
            return true;                                                                  //nosotros queremos que sea verdadero cuando nada lo bloquea.
        
        
        //Dibujar rayo desde cabeza del jugador para saber si no est� parado detr�s de un obst�culo bajo
        Vector3 headPlayer = headPos.transform.position; // Altura de la cabeza del PJ

        Vector3 headPosition = transform.position + Vector3.up * 1.8f; // Altura aproximada de una cabeza
        Vector3 dirFromHead = headPlayer - headPosition;
        return !Physics.Raycast(headPosition, dirFromHead.normalized, dirFromHead.magnitude, obsMask);
        

    }
    public void CheckLostSight(Transform target)
    {
        bool isInRange = CheckRange(target);

        // Si estaba en rango pero ahora no lo est�, dispara el evento onLostSight
        if (wasInRange && !isInRange && !eventFired)
        {
        Debug.Log("LOS:" + wasInRange);
            model.onLostSight?.Invoke();
            eventFired = true; // evita que el evento se vuelva a ejecutar
        }
        // si vuelve a estar en rango, resetea el flag para que pueda dispararse de nuevo cuando salga del rango
        if (isInRange)
        {
            wasInRange = true;
            eventFired = false;
        }
        else
        {
            wasInRange = false;
        }
    }

    public bool LOS(Transform self, Transform target, float range, float angle, LayerMask obsMask)
    {
        bool inSight = CheckRange(target) && CheckAngle(target) && CheckView(target);

        CheckLostSight(target);

        return inSight;
    }
}
