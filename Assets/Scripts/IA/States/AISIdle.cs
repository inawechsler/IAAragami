using UnityEngine;

public class AISIdle<T> : PSBase<T>
{
    T chaseInput;
    public AISIdle(T input)
    {
        chaseInput = input;
    }
    public override void Execute()
    {
        base.Execute();

        if (Input.GetKeyDown(KeyCode.K))
        {
            fsm.Transition(chaseInput);
        }
    }
}
