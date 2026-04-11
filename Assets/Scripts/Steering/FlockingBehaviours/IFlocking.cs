using UnityEngine;
using System.Collections.Generic;

public interface IFlocking 
{
    Vector3 GetDir(List<IBoid> boids, IBoid self);
    bool IsActive { get; set; }
}

