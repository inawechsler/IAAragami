using UnityEngine;

public class AISAttack<T> : AISBase<T>
{
    float _elapsedTime;
    float _seconds = 2f;

    public AISAttack()
    {
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
