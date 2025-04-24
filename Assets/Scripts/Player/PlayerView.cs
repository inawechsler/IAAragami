using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    private InputController input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<InputController>();
    }

    private void Update()
    {
        OnMoveAnim();
    }
    public void OnMoveAnim()
    {
        animator.SetFloat("Vel", input.moveInput.magnitude);
    }
    public void OnCrouchAnim(bool isCrouched)
    {
        animator.SetBool("isCrouch", isCrouched);
    }
}
