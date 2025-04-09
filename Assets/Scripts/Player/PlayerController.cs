using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerController : MonoBehaviour, PlayerInputControls.IPlayerLocomotionMapActions
{
    public PlayerInputControls inputActions;
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    private PlayerModel playerModel;

    private void Awake()
    {
        playerModel = GetComponent<PlayerModel>();
        
    }
    public void OnEnable()
    {
        inputActions = new PlayerInputControls();

        inputActions.Enable();

        inputActions.PlayerLocomotionMap.Enable();

        inputActions.PlayerLocomotionMap.SetCallbacks(this);
    }

    void OnDisable()
    {
        inputActions.PlayerLocomotionMap.Disable();
        inputActions.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    private void Update()
    {
        playerModel.SetMovementInput(moveInput);
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
