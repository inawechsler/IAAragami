using UnityEngine;

public class ObjectsColliders : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Start ejecutado en " + gameObject.name);

        var coll = GetComponent<Collider>();
        if (coll == null)
        {
            Debug.LogError("No se encontró collider en " + gameObject.name);
            return;
        }

        if (CollidersManager.instance == null)
        {
            Debug.LogError("CollidersManager.instance es null. ¿Está en la escena?");
            return;
        }

        CollidersManager.instance.AddColl(coll);
    }
    private void OnDestroy()// esto puede estar al pedo
    {
        var coll = GetComponent<Collider>();
        CollidersManager.instance.RemoveColl(coll);
    }
}
