using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour
{
    [Header("Mine Settings")]
    [SerializeField] private float activationDelay = 1.5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask targetLayers;

    [Header("Visual")]
    [SerializeField] private GameObject activationIndicator;
    [SerializeField] private GameObject explosionEffectPrefab;

    private IAttack attack;
    private bool isActive = false;

    private void Awake()
    {
        attack = GameObject.FindWithTag("Range").GetComponent<IAttack>(); //Enemigo que lanza la mina   
    }
    private void Start()
    {
        if (activationIndicator != null)
            activationIndicator.SetActive(false);

        StartCoroutine(ActivationSequence());
    }

    private IEnumerator ActivationSequence()
    {
        yield return new WaitForSeconds(activationDelay);

        if (activationIndicator != null)
            activationIndicator.SetActive(true);

        // Ahora la mina está activa
        isActive = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isActive || !IsInLayerMask(collider.gameObject.layer, targetLayers)) //Si la bomba no está activa o no está en la capa de los targets, no hace nada
            return;

        Explode();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!isActive || !IsInLayerMask(collider.gameObject.layer, targetLayers)) //Si la bomba no está activa o no está en la capa de los targets, no hace nada
            return;

        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayers);//Agarro todos los colliders que están dentro del radio de la mina

        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Player"))//Chequeo por cada uno si tiene tag del player
            {
                attack.onHitPlayer?.Invoke(); //Invoco el evento hiteo al player
                PlayerModel.RegisterEnemyHit();

            }
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity); //Instancio el prefab de la explosión
        }

        Destroy(gameObject); 
    }

    

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));//Compara el layer del GO que haya chocado con la mina sea del layerMask deseado: Player
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}


