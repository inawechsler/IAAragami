using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-3)]
public class InputController : MonoBehaviour, PlayerInputControls.IPlayerLocomotionMapActions
{
    public PlayerInputControls inputActions;
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerInputControls();

        inputActions.PlayerLocomotionMap.Move.performed += OnMove;
        inputActions.PlayerLocomotionMap.Move.canceled += OnMove;

        inputActions.PlayerLocomotionMap.Look.performed += OnLook;
        inputActions.PlayerLocomotionMap.Look.canceled += OnLook;
    }
    public void OnEnable()
    {
        if (inputActions == null)
            inputActions = new PlayerInputControls();
        inputActions.Enable();

        inputActions.PlayerLocomotionMap.Enable();

        inputActions.PlayerLocomotionMap.SetCallbacks(this);
    }


    public void OnFire(InputAction.CallbackContext context)
    {
        print("Fire");
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

}
