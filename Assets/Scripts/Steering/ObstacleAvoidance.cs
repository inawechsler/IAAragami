using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleAvoidance : MonoBehaviour
{
    [Min(1)]
    public int maxObs = 2;

    [Min(0)]
    public float radius;
    public LayerMask obsMask;
    Collider[] _colls;

    private void Awake()
    {
        _colls = new Collider[maxObs];
    }
    public Vector3 GetDir()
    {
        var count = Physics.OverlapSphereNonAlloc(transform.position, radius, _colls, obsMask);

        Collider nearColl = null;
        for(int i = 0; i < count; i++)
        {
            Collider currCol = _colls[i];
            if(nearColl == null)
            {
                nearColl = currCol;
            }

        }
        return Vector3.zero;
    }
}
