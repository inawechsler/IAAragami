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
        controller.playerModel.onCrouch?.Invoke();

    }

    public override void Execute()
    {
        //base.Execute();

        if(!controller.inputController.isCrouched)
        {
            StateMachine.Transition(inputToIdle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        controller.playerModel.onCrouch?.Invoke(); //Invocamos otra vez a la salida, controlador se encarga de detectar si se agacha o se levanta
        playerView.OnCrouchAnim(false);
    }
}
