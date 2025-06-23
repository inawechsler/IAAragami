using Unity.VisualScripting;
using UnityEngine;

public class CameraPostProcess : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float distortionRadius;
    public Shader _shader;
    public Texture texture;
    //[Range(0, 1)]
    [SerializeField] private float intensity, threshhold;
    //[SerializeField] private Color miColor;
    private float oldIntensity, oldthreshold, distortionIntensity;
    //private Color oldColor;

    private Collider[] detectedEnemies = new Collider[8];
    private Material mat;
    void Start()
    {
        mat = new Material(_shader);
        mat.SetTexture("_MiTextura", texture);
        oldIntensity = intensity;
    }
    private void Update()
    {
        CheckDistortion();

        if (oldIntensity != intensity || oldthreshold != threshhold)
            SetValues();
    }
    private void SetValues()
    {
        mat.SetFloat("_Intensity", intensity);
        mat.SetFloat("_Threshold", threshhold);
        
        oldIntensity = intensity;
        oldthreshold = threshhold;
        //oldColor = miColor;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.wrapMode = TextureWrapMode.Mirror;
        Graphics.Blit(source, destination, mat);
    }

    private void CheckDistortion()
    {
       detectedEnemies = Physics.OverlapSphere(player.transform.position, distortionRadius);
        float closestEnemyDistance = Mathf.Infinity;
        foreach (Collider col in detectedEnemies)
        {
            if(!col.CompareTag("Melee") && !col.CompareTag("Range")) continue;

            //Me guardo la distancia al enemigo mas cercano
            float distance = Vector3.Distance(player.transform.position, col.transform.position);
            if (distance < closestEnemyDistance)
            { 
                closestEnemyDistance = distance; 
            }
        }
        distortionIntensity = (distortionRadius - closestEnemyDistance) / 100;
        if (distortionIntensity < 0) distortionIntensity = 0;

        mat.SetFloat("_DistortionIntensity", distortionIntensity);
        Debug.Log(distortionIntensity);
    }
}
