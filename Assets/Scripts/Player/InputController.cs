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
        inputActions.Enable();

        inputActions.PlayerLocomotionMap.Enable();

        inputActions.PlayerLocomotionMap.SetCallbacks(this);

    }
    public void OnEnable()
    {
        if (inputActions == null)
            inputActions = new PlayerControls();

        inputActions.PlayerLocomotionMap.Move.performed += OnMove;
        inputActions.PlayerLocomotionMap.Move.canceled += OnMove;


        inputActions.PlayerLocomotionMap.Crouch.performed += OnCrouch;
        inputActions.PlayerLocomotionMap.Crouch.canceled += OnCrouch;
    }


    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if(moveInput != Vector2.zero)
            //{
            //    return;
            //}
            isCrouched = !isCrouched;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

}
