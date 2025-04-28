using UnityEngine;

public class PSIDle<T> : PSBase<T>
{

    private InputController inputController;
    T inputToWalk;
    T inputToCrouch;
    public PSIDle(T inputToWalk, T inputToCrouch, InputController inputController) : base()
    {
        this.inputController = inputController;
        this.inputToCrouch = inputToCrouch;
        this.inputToWalk = inputToWalk;
    }

    public override void Execute()
    {
        //base.Execute();

        if (inputController.moveInput.magnitude != 0)
        {

            StateMachine.Transition(inputToWalk);

        }
        else if (inputController.isCrouched)
        {

            StateMachine.Transition(inputToCrouch);

        }

    }
}
