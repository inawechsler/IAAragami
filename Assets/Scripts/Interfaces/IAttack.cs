using System;
using UnityEngine;

public interface IAttack 
{
    void Attack();
    Action onAttack { get; set; }
    Action onHitPlayer { get; set; }
    bool LastAttackHit();
}
