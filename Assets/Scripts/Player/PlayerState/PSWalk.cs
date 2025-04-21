using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PSWalk<T> : State<T>
{
    private PlayerController playerController;
    private PlayerView playerView;
    private PlayerModel playerModel;
    private InputController inputController;

    private T inputToIdle;
    public PSWalk(PlayerController playerController, T InputWalk, FSM<T> fsm) : base()
    {
        this.playerController = playerController;
        playerView = playerController.playerView;
        inputController = playerController.inputController;
        inputToIdle = InputWalk;
        playerModel = playerController.playerModel;
        StateMachine = fsm;
    }


    public override void Execute()
    {
        base.Execute();
        if (playerController != null)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 moveInput = new Vector3(horizontal, 0, vertical);

            playerModel.SetMovementInput(CalculateMovementDirection());
            if (CalculateMovementDirection() != Vector3.zero)
            {
                playerView.LookDir(CalculateMovementDirection());

            } else
            {
                StateMachine.Transition(inputToIdle);
            }

        }
    }


    private Vector3 CalculateMovementDirection()
    {
        // crear el vector de movimiento usando el input
        // moveInput.y positivo (W) = adelante
        // moveInput.x positivo (D) = derecha
        Vector3 movement = playerController.transform.forward * -inputController.moveInput.y + 
            playerController.transform.right * -inputController.moveInput.x;

        return movement.normalized;
    }

}
