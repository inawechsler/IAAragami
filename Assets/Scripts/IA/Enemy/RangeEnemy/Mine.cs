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
        attack = GameObject.FindWithTag("Range").GetComponent<IAttack>();
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

        // Ahora la mina est� activa
        isActive = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isActive || !IsInLayerMask(collider.gameObject.layer, targetLayers))
            return;

        // Explotamos
        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayers);

        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Player"))
            {
                Debug.Log("jdpaosda");
                attack.onHitPlayer?.Invoke();
            }
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}


