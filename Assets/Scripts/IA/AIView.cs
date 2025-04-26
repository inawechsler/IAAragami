using System.Collections;
using UnityEngine;

public class AIView : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;


    private void Awake()
    {
        GetComponent<IAttack>().onAttack += OnAttackAnim;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        OnMoveAnim();
    }

    public void OnAttackAnim()
    {
        animator.SetTrigger("Attack");
    }


    void OnMoveAnim()
    {
        animator.SetFloat("Vel", rb.linearVelocity.magnitude);
    }

}