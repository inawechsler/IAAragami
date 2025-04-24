using UnityEngine;

public class PSCrouch<T> : PSBase<T>
{
    private PlayerView playerView;
    private PlayerController controller;
    private T inputToIdle;


    public PSCrouch(PlayerController playerController, T InputIdle) : base()
    {
        playerView = playerController.playerView;
        inputToIdle = InputIdle;
        controller = playerController;
    }

    public override void Enter()
    {
        base.Enter();

        playerView.OnCrouchAnim(true);
        crouch.ToggleCrouch();
    }

    public override void Execute()
    {
        //base.Execute();
        move.Move(move.CalculateMovementDirection());
        look.LookDir(move.CalculateMovementDirection());

        if (!controller.inputController.isCrouched)
        {
            StateMachine.Transition(inputToIdle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        crouch.ToggleCrouch();
        playerView.OnCrouchAnim(false);
    }
}
