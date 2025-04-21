using System;
using UnityEngine;

public class CameraModel : MonoBehaviour 
{
    [Header("Line Of Sight")]
    [Range(1, 360)]
    public float angle;
    public float range;
    public LayerMask obsMask;
    public Action<bool> onChangeIsDetecting = delegate { };

    bool _IsDetecting;

    [Space]
    [Header("Target")]
    [SerializeField]    
    Transform _target;

    public Transform CheckTarget()
    {
        return _target;
    }
    public bool IsDetecting
    {
        set
        {
            if (value != IsDetecting) onChangeIsDetecting(value);
            _IsDetecting = value;
        }
        get
        {
            return _IsDetecting;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0 ,angle/2 ,0) * transform.forward * range);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / -2, 0) * transform.forward * range);
    }
}
