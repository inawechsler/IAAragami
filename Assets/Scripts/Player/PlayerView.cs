using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour, ILook
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    // Cache para evitar cálculos repetidos
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;


    [Header("Components")]
    private InputController inputController;
    private Rigidbody rb;
    [SerializeField] private Camera mainCamera { get; set; }
    [SerializeField] private Animator animator;


    private void Awake()
    {
       
        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController>();
    }
    private void Update()
    {
        OnMoveAnim();
    }

    public void LookDir(Vector3 inputDir)
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


    void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);  
    }

    private IEnumerator RotateToTarget()
    {
        // Mientras no estemos cerca de la dirección objetivo
        while (Vector3.Dot(transform.forward, targetDirection) < 0.996f)
        {
            // roto hacia la dirección objetivo
            transform.forward = Vector3.Slerp(
                transform.forward,
                targetDirection,
                Time.deltaTime * rotationSpeed);

            yield return null;
        }

        rotationCoroutine = null;
    }
}