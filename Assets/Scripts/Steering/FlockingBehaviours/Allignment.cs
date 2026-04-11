using UnityEngine;
using System.Collections.Generic;

public class Allignment : FlockingBaseBehaviour
{
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < boids.Count; i++)
        {
            var currBoid = boids[i];
            alignment += currBoid.Forward;
        }

        return alignment.normalized * multiplier;
        
       
    }
}
