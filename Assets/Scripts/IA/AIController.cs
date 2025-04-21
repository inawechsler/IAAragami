using UnityEngine;

public class AIController : MonoBehaviour
{

    private FSM<PSEnum> fsm;

    private void Awake()
    {

        InitFSM();
    }

    void InitFSM()
    {
        //fsm = new FSM<PSEnum>();

        //var idle = new PSIDle<PSEnum>(inputController, PSEnum.Walk, fsm);
        //var walk = new PSWalk<PSEnum>(this, PSEnum.Idle, fsm);

        //idle.AddTransition(PSEnum.Walk, walk);
        //walk.AddTransition(PSEnum.Idle, idle);


        //fsm.SetInit(idle);
    }

    private void Update()
    {
        fsm.OnExecute();
    }
}