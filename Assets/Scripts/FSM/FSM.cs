using Unity.VisualScripting;
using UnityEngine;

public class FSM<T> 
{
    IState<T> currentState;

    public FSM()
    {

    }

    public IState<T> getState()
    {
        return currentState;
    }

    public void OnExecute()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void SetInit(IState<T> state)
    {
        currentState = state;
        currentState.Enter();

        state.StateMachine = this;
    }

    public void Transition(T input)
    {
        var nextState = currentState.GetTransition(input);
        if (nextState == null)
        {
            //Debug.LogWarning("No se encontró transición válida para el input: " + input + " del estado " + currentState.GetType().Name);
            return;
        }


        currentState.Exit();
        currentState = nextState;
        currentState.Enter();

    }
}
