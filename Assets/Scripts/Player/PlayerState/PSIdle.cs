using UnityEngine;

public class PSIDle<T> : State<T>
{

    private InputController inputController;
    T inputToWalk;
    public PSIDle(InputController inputController, T inputToWalk, FSM<T> fsm) : base()
    {
        this.inputController = inputController;
        this.inputToWalk = inputToWalk;
        StateMachine = fsm;

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
