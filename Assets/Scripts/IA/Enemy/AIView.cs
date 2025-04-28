using System.Collections;
using UnityEngine;

public class AIView : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] protected Animator animator;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        OnMoveAnim();
    }

    private void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);
    }

}