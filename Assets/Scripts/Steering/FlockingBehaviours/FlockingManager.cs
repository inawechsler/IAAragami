using UnityEngine;
using System.Collections.Generic;

public class FlockingManager : MonoBehaviour, ISteering
{
    IFlocking[] _behaviours;
    IBoid _self;
    Collider[] _colls;
    [Min(1)]
    public int maxBoids;
    [Min(1)]
    public int radius = 10;
    public LayerMask boidMask;

    List<IBoid> _boids;
    private void Awake()
    {
        _behaviours = GetComponents<IFlocking>();
        _self = GetComponent<IBoid>();
        _colls = new Collider[maxBoids];
        _boids = new List<IBoid>();
    }
  public Vector3 GetDir()
    {
        _boids.Clear();
        int count = Physics.OverlapSphereNonAlloc(_self.Position, radius, _colls,boidMask );

        for (int i = 0; i < count; i++)
        {
            var boid = _colls[i].GetComponent<IBoid>();
            if (boid == null || boid == _self) continue;
            _boids.Add(boid);
        }

        Vector3 dir = Vector3.zero;

        for (int i = 0; i < _behaviours.Length; i++) 
        {
         var curr = _behaviours[i];
         dir += curr.GetDir(_boids, _self);
        }
            
        return dir.normalized;
    }

    public IBoid SetSelf { set => _self = value; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
