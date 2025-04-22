using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
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
    public void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);
    }

    public void OnCrouchAnim(bool value)
    {
        animator.SetBool("isCrouch", value);
    }
}
