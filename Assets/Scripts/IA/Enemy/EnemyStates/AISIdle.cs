using UnityEngine;
public class AISIdle<T> : AISBase<T>
{

    public AISIdle()
    {
    }

    public override void Enter()
    {
        base.Enter();
        //move.Move(Vector3.zero);
    }

}
