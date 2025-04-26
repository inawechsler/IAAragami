using NUnit.Framework;
using System.Collections.Generic;
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

        var move = GetComponent<IMove>();
        var look = GetComponent<ILook>(); 
        var crouch = GetComponent<ICrouch>();

        var stateList = new List<PSBase<PSEnum>>();
        var idle = new PSIDle<PSEnum>(PSEnum.Walk, PSEnum.Crouch, inputController);
        var walk = new PSWalk<PSEnum>(this, PSEnum.Idle);
        var crouching = new PSCrouch<PSEnum>(this, PSEnum.Idle);

        idle.AddTransition(PSEnum.Walk, walk);
        idle.AddTransition(PSEnum.Crouch, crouching);

        walk.AddTransition(PSEnum.Idle, idle);
        crouching.AddTransition(PSEnum.Idle, idle);

        stateList.Add(idle);
        stateList.Add(walk);
        stateList.Add(crouching);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm, crouch
                );
        }

        fsm.SetInit(idle);
    }

    private void Update()
    {
        fsm.OnExecute();
    }
}
