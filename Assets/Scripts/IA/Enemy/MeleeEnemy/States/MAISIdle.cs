using UnityEngine;
//MAISAttack = Melee AI State Attack 
public class MAISIdle<T> : AISBase<T>
{

    public MAISIdle()
    {
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }

}
