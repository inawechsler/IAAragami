using UnityEngine;

public class ObjectsColliders : MonoBehaviour
{
    private void Start()
    {
        var coll = GetComponent<Collider>();
        CollidersManager.instance.AddColl(coll);
    }
    private void OnDestroy()// esto puede estar al pedo
    {
        var coll = GetComponent<Collider>();
        CollidersManager.instance.RemoveColl(coll);
    }
}
