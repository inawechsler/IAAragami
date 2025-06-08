using UnityEngine;
using System.Collections.Generic;

public class Cohesion : FlockingBaseBehaviour
{
    

    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector3 cohesion = Vector3.zero;
        Vector3 center = Vector3.zero;

        for (int i = 0; i < boids.Count; i++)
        {
            center += boids[i].Position;
        }
        if (boids.Count > 0)
        {
            center /= boids.Count;
            cohesion = center - self.Position;
        }


        return cohesion.normalized * multiplier;
        
    }
}
