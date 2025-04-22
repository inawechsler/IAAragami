using System.Collections;
using UnityEngine;

public class AIView : MonoBehaviour, ILook
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    // Cache para evitar c�lculos repetidos
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;


    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;


    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        OnMoveAnim();
    }

    public void LookDir(Vector3 inputDir)
    {
        inputDir.Normalize();

        // verifico si la direcci�n cambi�
        bool directionChanged = (targetDirection == Vector3.zero) ||
                                (Vector3.Dot(targetDirection, inputDir) < 0.966f); // 15 grados masomenos

        if (directionChanged)
        {
            //nueva direcci�n objetivo
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
        // Mientras no estemos cerca de la direcci�n objetivo
        while (Vector3.Dot(transform.forward, targetDirection) < 0.996f)
        {
            // roto hacia la direcci�n objetivo
            transform.forward = Vector3.Slerp(
                transform.forward,
                targetDirection,
                Time.deltaTime * rotationSpeed);

            yield return null;
        }

        rotationCoroutine = null;
    }
}