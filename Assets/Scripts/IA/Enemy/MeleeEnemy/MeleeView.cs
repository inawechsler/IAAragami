using System.Collections;
using UnityEngine;

public class MeleeView : AIView
{
    protected override void Awake()
    {
        base.Awake();
        GetComponent<IAttack>().onAttack += OnAttackAnim;
    }

    public void OnAttackAnim()
    {
        animator.SetTrigger("Attack");
    }

}