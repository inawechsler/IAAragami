using UnityEngine;
public class AISIdle<T> : AISBase<T>
{
    public AISIdle()
    {
    }

    public override void Execute()
    {
        base.Execute();
        move.Move(Vector3.zero);
    }

}
