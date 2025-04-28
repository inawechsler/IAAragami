using System;
using System.Collections;
using UnityEngine;



[DefaultExecutionOrder(-1)]
public class PlayerModel : MonoBehaviour, IMove, ILook, ICrouch
{
    [Header("Movement")]
    private float moveSpeed = 3f;
    private float originalMoveSpeed => moveSpeed;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 targetDirection;
    private Coroutine rotationCoroutine;

    [Header("Crouch")]
    [SerializeField] private float crouchHeightReduction; // Cuánto reducir la altura
    [SerializeField] private float crouchTransitionSpeed; // Velocidad de transición
    [SerializeField] private float crouchSpeed; // Velocidad de transición

    // Variables privadas para el crouch
    private float originalHeight;
    private float targetHeight;
    private Vector3 originalCenter;
    private Vector3 crouchCenter;
    private CapsuleCollider characterCollider;

    [Header("Components")]
    Rigidbody rb;
    InputController inputController;
    [SerializeField] private Camera mainCamera;

    public static event Action onEnemyHitPlayer; 
    public Transform Position { get; set; }
    public Action onCrouch { get; set; }

    void Awake()
    {
        onEnemyHitPlayer += ManagePlayerLoss;
        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController>();
        Position = transform;
        onCrouch += ToggleCrouch; //Cuando se invoque crouch se invocará ToggleCrouch
        characterCollider = GetComponent<CapsuleCollider>();
        SetCapsuleParam();
    }

    public void ManagePlayerLoss()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public static void RegisterEnemyHit()
    {
        onEnemyHitPlayer?.Invoke();
    }
    public void SetCapsuleParam()//Seteo los valores para el crouch
    {
        if (characterCollider != null)
        {
            originalHeight = characterCollider.height;  
            originalCenter = characterCollider.center;

            // Calcular el centro cuando está agachado
            crouchCenter = new Vector3(
                originalCenter.x,
                .36f,
                originalCenter.z
            );
        }
    }

    public void Move(Vector3 input)
    {
        if (input == Vector3.zero)
        {
            Vector3 stopVelocity = Vector3.zero;
            stopVelocity.y = rb.linearVelocity.y; // Conservamos solo la velocidad vertical
            rb.linearVelocity = stopVelocity;
            return;
        }
        Vector3 movementVelocity = transform.forward * moveSpeed;

        movementVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = movementVelocity;
    }

    public Vector3 CalculateMovementDirection()
    {

        Vector2 input = inputController.moveInput;

        if (input == Vector2.zero) return Vector3.zero; // No hay movimiento

        Vector3 direction = Vector3.zero;

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        //plano horizontal (XZ)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        direction = cameraForward * -input.y +
                   cameraRight * -input.x;
        return direction.normalized;
    }


    public void LookDir(Vector3 inputDir)
    {
        if (inputDir == Vector3.zero) return;

        bool directionChanged = (targetDirection == Vector3.zero) ||
                               (Vector3.Dot(transform.forward, inputDir) < 0.966f); // si el input es cero o el angulo entre forward y el input es mayor a 15°

        if (directionChanged)
        {
            // Nueva dirección objetivo
            targetDirection = inputDir;

            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);

            // nueva rotación
            rotationCoroutine = StartCoroutine(RotateToTarget());
        }
    }

    private IEnumerator RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection); //Obtengo quaternion de la dirección objetivo

        while (Quaternion.Angle(transform.rotation, targetRotation) > 2f) //Mientras el angulo sea mayor a 2  
        {
            transform.rotation = Quaternion.Slerp(//Spherical Lerp entre targetRotation y la rotación actual, Spherical porque emula una interpolación en base a una esfera, (rotacion en el eje Z)
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed);

            yield return null;
        }

        // rotación final exacta
        transform.rotation = targetRotation;

        rotationCoroutine = null;


    }

    public void ToggleCrouch()
    {
        moveSpeed = inputController.isCrouched ? crouchSpeed : originalMoveSpeed; //La velocidad de MoveSpeed se asigna en
                                                                                  //base al valor de isCrouched, si es true la velocidad es crouchSpeed, si no es originalMoveSpeed

        // Alternamos entre agachado y de pie
        targetHeight = inputController.isCrouched ? //Lo mismo que en moveSpeed pero con la altura
            originalHeight - crouchHeightReduction : originalHeight;

        Vector3 targetCenter = inputController.isCrouched ? 
            crouchCenter : originalCenter;

        StartCoroutine(TransitionCrouch(targetHeight, targetCenter));
    }

    private IEnumerator TransitionCrouch(float targetHeight, Vector3 targetCenter)
    {

        float startHeight = characterCollider.height;
        Vector3 startCenter = characterCollider.center;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * crouchTransitionSpeed; //Timer
            float t = Mathf.Clamp01(elapsedTime);

            // interpolar entre la altura actual y la objetivo
            characterCollider.height = Mathf.Lerp(startHeight, targetHeight, t); //Lerp en altura en base a su valor inicial y el target, en t segundos
            characterCollider.center = Vector3.Lerp(startCenter, targetCenter, t);

            yield return null;
        }

        characterCollider.height = targetHeight;//Aseguro que sea esa efectivamente la altura
        characterCollider.center = targetCenter;

    }

}
