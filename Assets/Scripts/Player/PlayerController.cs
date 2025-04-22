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

    private IMove move;

    private ILook look;
    public PlayerView playerView { get; private set; }

    private FSM<PSEnum> fsm;

    private void Awake()
    {
        inputController = GetComponent<InputController>();
        playerModel = GetComponent<PlayerModel>();
        playerView = GetComponent<PlayerView>();
        move = GetComponent<IMove>();
        look = GetComponent<ILook>();


        InitFSM();
    }

    void InitFSM()
    {

        fsm = new FSM<PSEnum>();

        var stateList = new List<PSBase<PSEnum>>();
        var idle = new PSIDle<PSEnum>(PSEnum.Walk, inputController);
        var walk = new PSWalk<PSEnum>(this, PSEnum.Idle);

        idle.AddTransition(PSEnum.Walk, walk);
        walk.AddTransition(PSEnum.Idle, idle);

        stateList.Add(idle);
        stateList.Add(walk);

        for (int i = 0; i < stateList.Count; i++)
        {
            stateList[i].Initialize(look, move, fsm);
        }

        fsm.SetInit(idle);
    }

    private void Update()
    {
        fsm.OnExecute();
    }
}
