using NUnit.Framework;
using UnityEngine;
public class FAISIdle<T> : AISBase<T>
{
    MonoBehaviour monoBehaviour;
    private bool[] _originalStates;
    IFlocking[] flockingBehaviours;
    public FAISIdle(MonoBehaviour monoBehaviourRef)
    {
        monoBehaviour = monoBehaviourRef;
    }
    public override void Enter()
    {
        base.Enter();

        flockingBehaviours = monoBehaviour.GetComponents<IFlocking>();
        _originalStates = new bool[flockingBehaviours.Length];

        for (int i = 0; i < flockingBehaviours.Length; i++)
        {
            _originalStates[i] = flockingBehaviours[i].IsActive;
            flockingBehaviours[i].IsActive = false;
            Debug.Log(flockingBehaviours[i].GetType().Name);
        }
    }
    public override void Execute()
    {
        base.Execute();
        move.Move(Vector3.zero);

    }

    public override void Exit()
    {
        base.Exit();

        for (int i = 0; i < _originalStates.Length; i++)
        {
            flockingBehaviours[i].IsActive = _originalStates[i];
        }
    }


}
