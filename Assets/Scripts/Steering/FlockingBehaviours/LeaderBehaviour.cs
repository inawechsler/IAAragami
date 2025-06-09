using UnityEngine;
using System.Collections.Generic;

public class LeaderBehaviour : FlockingBaseBehaviour 
{
    public float timePrediction;
    Seek _seek;
    Pursuit _pursuit;
    bool _isPursuit;

    private void Awake()
    {
        _pursuit = new Pursuit(transform, timePrediction);
        _seek = new Seek(transform);
    }
    protected override Vector3 GetRealDir(List<IBoid> boids, IBoid self)
    {
        if (_isPursuit)
        {
            return _pursuit.GetDir() * multiplier;
        }
        return _seek.GetDir() * multiplier;
    }
    public Transform Leader  
    {
        set
        {
            var rb = value.GetComponent<Rigidbody>();

            if (rb)
            {
                _pursuit.Target = rb;
                _isPursuit = true;
            }
            else
            {
                _seek.Target = value;
                _isPursuit = false;
            }

        }
    }

    public Rigidbody LeaderRb 
    {
        set
        {
              _pursuit.Target = value;
              _isPursuit = true;
          
        }
    }
}
