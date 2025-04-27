using UnityEngine;
using UnityEngine.Windows;

public class MAISAttack<T> : AISBase<T>
{
    Transform _transform;
    public MAISAttack(Transform transform)
    {
        _transform = transform;
    }
    public override void Enter()
    {
        base.Enter();
        attack.Attack();
        look.LookDir(LookAtTarget());
    }

    Vector3 LookAtTarget()
    {
        Vector3 targetDir = _transform.position - controller.transform.position;
        targetDir.y = 0;
        return targetDir.normalized;
    }

    public override void Execute()
    {
        base.Execute();
    }
}
