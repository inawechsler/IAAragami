using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerController : MonoBehaviour
{
    private InputController inputController;
    private PlayerModel playerModel;
    private PlayerView playerView;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        playerModel = GetComponent<PlayerModel>();
        playerView = GetComponent<PlayerView>();
    }

    private void Update()
    {
        if (playerModel != null)
        {
            playerModel.SetMovementInput(CalculateMovementDirection());
            if (inputController.moveInput.magnitude > 0.1f)
            {
                playerView.LookDir(CalculateMovementDirection());
            }

        }
    }

    private Vector3 CalculateMovementDirection()
    {
        // crear el vector de movimiento usando el input
        // moveInput.y positivo (W) = adelante
        // moveInput.x positivo (D) = derecha
        Vector3 movement = transform.forward * -inputController.moveInput.y + transform.right * -inputController.moveInput.x;

        return movement.normalized;
    }

}
