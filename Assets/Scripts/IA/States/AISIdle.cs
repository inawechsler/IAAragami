using UnityEngine;

public class AISIdle<T> : AISBase<T>
{
    AIController AIController;
    public AISIdle(AIController controller)
    {
        AIController = controller;
    }
    public override void Execute()
    {
        base.Execute();

        look.LookDir(Vector3.zero);

    }
}
