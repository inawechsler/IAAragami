using UnityEngine;
using UnityEngine.Windows;

public class AISAttack<T> : AISBase<T>
{
    float _elapsedTime;
    float _seconds = 2f;
    Transform _transform;
    AIController _controller;
    public AISAttack(Transform transform, AIController controller)
    {
        _transform = transform;
        _controller = controller;
    }
    public override void Enter()
    {
        base.Enter();
        attack.Attack();
        look.LookDir(LookAtTarget());
    }

    Vector3 LookAtTarget()
    {
        Vector3 targetDir = _transform.position - _controller.transform.position;
        targetDir.y = 0;
        return targetDir.normalized;
    }

    public override void Execute()
    {
        base.Execute();
    }
}
