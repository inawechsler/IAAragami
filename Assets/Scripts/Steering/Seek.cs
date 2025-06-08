using UnityEngine;

public class Seek : ISteering
{
    Transform _self;
    Transform _target;

    public Seek(Transform self)
    {
        _self = self;
    }
    public Seek(Transform self, Transform target)
    {
        _self = self;
        _target = target;
    }
    

    public virtual Vector3 GetDir()
    {
        return(_target.position - _self.position).normalized;
    }

    public Transform Target
    {
        set
        {
            _target = value;
        }
    }
}
