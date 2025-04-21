using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    // Cache para evitar cálculos repetidos
    private Vector3 targetDirection;
    private Transform cachedTransform;
    private Coroutine rotationCoroutine;


    [Header("Components")]
    private InputController inputController;
    private Rigidbody rb;
    [SerializeField] private Camera mainCamera { get; set; }
    [SerializeField] private Animator animator;


    private void Awake()
    {
        // cache el transform
        cachedTransform = transform;

        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController>();
    }
    private void Update()
    {
        OnMoveAnim();
    }

    public void LookDir(Vector3 inputDir)
    {
        // Verificamos si la dirección tiene magnitud significativa
        if (inputDir.sqrMagnitude > 0.01f)
        {
            inputDir.Normalize();

            // verifico si la dirección cambió
            bool directionChanged = (targetDirection == Vector3.zero) ||
                                    (Vector3.Dot(targetDirection, inputDir) < 0.966f); // 15 grados masomenos

            if (directionChanged)
            {
                //nueva dirección objetivo
                targetDirection = inputDir;

           
                if (rotationCoroutine != null)
                {
                    StopCoroutine(rotationCoroutine);
                }

                rotationCoroutine = StartCoroutine(RotateToTarget());
            }
        }
    }


    void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);  
    }

    private IEnumerator RotateToTarget()
    {
        // Mientras no estemos cerca de la dirección objetivo
        while (Vector3.Dot(cachedTransform.forward, targetDirection) < 0.996f) // ~5 grados
        {
            // roto hacia la dirección objetivo
            cachedTransform.forward = Vector3.Slerp(
                cachedTransform.forward,
                targetDirection,
                Time.deltaTime * rotationSpeed);

            yield return null;
        }

        rotationCoroutine = null;
    }
}