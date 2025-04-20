using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerController : MonoBehaviour
{
    public InputController inputController { get; private set; }
    public PlayerModel playerModel{ get; private set; }
    public PlayerView playerView { get; private set; }

    private FSM<PSEnum> fsm;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        playerModel = GetComponent<PlayerModel>();
        playerView = GetComponent<PlayerView>();

        InitFSM();
    }

    void InitFSM()
    {
        fsm = new FSM<PSEnum>();

        var idle = new PSIDle<PSEnum>(inputController, PSEnum.Walk, fsm);
        var walk = new PSWalk<PSEnum>(this, PSEnum.Idle, fsm);

        idle.AddTransition(PSEnum.Walk, walk);
        walk.AddTransition(PSEnum.Idle, idle);


        fsm.SetInit(idle);
    }

    private void Update()
    {
        fsm.OnExecute();
    }
}
