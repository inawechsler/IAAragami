using System.Collections.Generic;
using UnityEngine;

public class PredatorBehaviour : FlockingBaseBehaviour
{
    [Min(0.25f)]
    public float radius = 2;
    [Min(1)]
    public int maxPredators;
    Collider[] _colls;
    public LayerMask predatorMask;

    private void Awake()
    {
        _colls = new Collider[maxPredators];
    }
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)//Colocar aqui logica(moscas que esquiven con llave en mano)
    {
        int count = Physics.OverlapSphereNonAlloc(self.Position, radius, _colls, predatorMask);//colocar layermask en la entidad predator
        Vector3 avoidance = Vector3.zero;

        for (int i = 0; i < count; i++) 
        {
          var predator = _colls[i];
            Vector3 diff = self.Position - predator.transform.position;

            avoidance += diff.normalized * (radius - diff.magnitude);
        }
        return avoidance.normalized * multiplier;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
