using System.Collections;
using UnityEngine;

public class AIView : MonoBehaviour
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


    void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);
    }

}