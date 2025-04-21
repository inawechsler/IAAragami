using UnityEngine;

public class Pursuit : ISteering
{

    Transform _self;
    Rigidbody _target;
    float _timePrediction;

    public Pursuit(Transform self, Rigidbody target, float timePrediction)
    {
        _self = self;
        _target = target;
        _timePrediction = timePrediction;
    }

    public Pursuit(Transform self, Rigidbody target)
    {
        _self = self;
        _target = target;
    }
    public virtual Vector3 GetDir()
    {
        Vector3 point = _target.position + _target.transform.forward * _target.linearVelocity.magnitude * _timePrediction;
        Vector3 dirToPoint = (point - _self.position).normalized;
        Vector3 dirToTarget = (_target.position - _self.position).normalized;

        if(Vector3.Dot(dirToPoint, dirToTarget) < 0)
        {
            return dirToTarget;
        }
        else
        {
            return dirToPoint;
        }
       
    }

    public float TimePrediction
    {
        get
        {
            return _timePrediction;
        }
        set
        {
            _timePrediction = value;
        }
    }
}
