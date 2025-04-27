using UnityEngine;
using UnityEngine.Windows;

public class RAISAttack<T> : AISBase<T>
{
    Transform _transform;
    public RAISAttack(Transform transform)
    {
        _transform = transform;
    }
    public override void Enter()
    {
        base.Enter();
        attack.Attack();
    }

    public override void Execute()
    {
        base.Execute();
    }
}
