using UnityEngine;
using System.Collections.Generic;

public class Avoidance : FlockingBaseBehaviour
{
   
    public float personalArea = 0.5f;

    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        Vector3 avoidance = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 diff = self.Position - boids[i].Position;
            if (diff.magnitude > personalArea) continue;

            avoidance += diff.normalized * (personalArea - diff.magnitude);
        }

        return avoidance * multiplier;
    }
   
}
