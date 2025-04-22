using UnityEngine;

public class PSIDle<T> : PSBase<T>
{

    private InputController inputController;
    T inputToWalk;
    public PSIDle(T inputToWalk, InputController inputController) : base()
    {
        this.inputController = inputController;
        this.inputToWalk = inputToWalk;
    }

    public override void Execute()
    {
        base.Execute();

        if (inputController != null)
        {
            if (inputController.moveInput.magnitude != 0)
            {
                if (StateMachine != null)
                {
                    StateMachine.Transition(inputToWalk);
                }
            }
        }
    }
}
