using System;
using System.Collections;
using UnityEngine;

public class MeleeModel : AIModel, IAttack
{
    [Header("Attack")]
    [SerializeField] private Collider attackCollider;
    private bool _lastAttackHit = false;
    public Action onAttack { get; set; }
    protected Coroutine lastAttackHitSCor;
    public Action onHitPlayer;

    protected override void Awake()
    {
        base.Awake();
    }

    public void EnableAttackCollider()
    {
        _lastAttackHit = false; // Reiniciar el estado del hit
        attackCollider.isTrigger = true;
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider player)
    {
        // Che, verificamos si el collider está activo y si golpeamos al jugador
        if (attackCollider.enabled && player.CompareTag("Player"))
        {
            if (lastAttackHitSCor != null)
                StopCoroutine(lastAttackHitSet());

            onHitPlayer?.Invoke();
            lastAttackHitSCor = StartCoroutine(lastAttackHitSet());
        }
    }

    private IEnumerator lastAttackHitSet()
    {
        _lastAttackHit = true;
        yield return new WaitForSeconds(.5f);
        _lastAttackHit = false;

        lastAttackHitSCor = null;
    }

    public bool LastAttackHit()
    {
        return _lastAttackHit;
    }

    public void Attack()
    {
        onAttack?.Invoke();
    }
}