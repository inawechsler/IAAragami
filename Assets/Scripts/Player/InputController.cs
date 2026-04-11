using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-3)]
public class InputController : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls inputActions;

    public PlayerModel playerModel { get; private set; }
    public Vector2 moveInput { get; private set; }

    public bool isCrouched { get; private set; } = false;

    private void Awake()
    {
        playerModel = GetComponent<PlayerModel>();

        inputActions = new PlayerControls();
        inputActions.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.PlayerLocomotionMap.Enable();
    }

    private void OnDisable()
    {
        inputActions.PlayerLocomotionMap.Disable();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouched = !isCrouched; //Si estaba agachado se desagacha, si no, se agacha
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

}
