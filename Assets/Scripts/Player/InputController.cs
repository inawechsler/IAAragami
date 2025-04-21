using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-3)]
public class InputController : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls inputActions;
    public Vector2 moveInput { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerControls();

        inputActions.PlayerLocomotionMap.Move.performed += OnMove;
        inputActions.PlayerLocomotionMap.Move.canceled += OnMove;
    }
    public void OnEnable()
    {
        if (inputActions == null)
            inputActions = new PlayerControls();
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
        print("Move");
        moveInput = context.ReadValue<Vector2>();
    }

}
