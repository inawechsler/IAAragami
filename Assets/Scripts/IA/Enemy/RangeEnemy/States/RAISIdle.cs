using UnityEngine;

//RAISAttack = Range AI State Attack 

public class RAISIdle<T> : AISBase<T>
{

    public RAISIdle()
    {
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }

}
