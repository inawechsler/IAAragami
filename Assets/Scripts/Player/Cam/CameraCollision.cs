using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private float _minDistance = 1f;
    private float _maxDistance = 5f;
    private float _movSmoothness = 10f;
    private float _distance;

    Vector3 dir;

    private void Start()
    {
        dir = transform.localPosition.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Linecast(transform.position, transform.parent.position, out hit))
        {
           _distance = Mathf.Clamp(hit.distance, _minDistance, _maxDistance);
        }
        else
        {
            _distance = _maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dir * _distance, _movSmoothness * Time.deltaTime);
    }
}
