using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PSWalk<T> : PSBase<T>
{
    private InputController inputController;
    private PlayerController controller;

    private T inputToIdle;
    public PSWalk(PlayerController playerController, T InputWalk) : base()
    {
        inputController = playerController.inputController;
        inputToIdle = InputWalk;
        controller = playerController;
    }


    public override void Execute()
    {
        base.Execute();
        if (controller != null)
        {
            move.Move(move.CalculateMovementDirection());
            if (move.CalculateMovementDirection() != Vector3.zero)
            {
                look.LookDir(move.CalculateMovementDirection());

            } else
            {
                StateMachine.Transition(inputToIdle);
            }

        }
    }

}
