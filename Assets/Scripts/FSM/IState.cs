using UnityEngine;

public interface IState<T>
{
    void Enter();
    void Execute();

    void Exit();

    FSM<T> StateMachine { get; set; }
    IState<T> GetTransition(T input);
    void AddTransition(T input, IState<T> state);

    void RemoveTransitionByInput(T input);

    void RemoveTransitionByState(IState<T> state);

}
