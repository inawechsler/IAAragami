using System;
using System.Collections;
using UnityEngine;

public class MeleeModel : AIModel
{
    [Header("Attack")]
    [SerializeField] private Collider attackCollider;
    protected Coroutine lastAttackHitSCor;

    protected override void Awake()
    {
        base.Awake();
        lostSightDuration = 3f;
        attackCollider.enabled = false;
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
        //Si el trigger está activo y el objeto tiene el tag del player
        if (attackCollider.enabled && player.CompareTag("Player"))
        {
            if (lastAttackHitSCor != null)
                StopCoroutine(lastAttackHitSet());

            PlayerModel.RegisterEnemyHit();
            onHitPlayer?.Invoke(); //Invoco al evento de hit acertado
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
}