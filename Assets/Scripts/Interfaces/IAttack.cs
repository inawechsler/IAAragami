using System;
using UnityEngine;

public interface IAttack 
{
    void Attack();
    Action onAttack { get; set; }
    bool LastAttackHit();
}
