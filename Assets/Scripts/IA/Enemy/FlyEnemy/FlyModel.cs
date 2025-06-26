using UnityEngine;
using System.Collections.Generic;

public class FlyModel : AIModel, IBoid
{
    public Rigidbody Target;

    public Transform safeSpot;
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;

    [SerializeField] private GameObject[] GOWings;

    private List<Material> matWings = new List<Material>();

    private int SpeedPropertyID = Shader.PropertyToID("_Speed");

    void Start()
    {
        foreach (var go in GOWings)
        {
            if (go.TryGetComponent<Renderer>(out var renderer))
            {
                foreach (var mat in renderer.materials)
                {
                    matWings.Add(mat);
                }
            }
        }

        float newSpeed = NewSpeed();
        foreach (var mat in matWings)
        {
            mat.SetFloat(SpeedPropertyID, mat.GetFloat(SpeedPropertyID) + newSpeed);
        }

    }
    public float NewSpeed()
    {
        return Random.Range(-1.5f, 1.5f);
    }



}

