using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T>
{
    protected FSM<T> fsm;
    Dictionary<T, IState<T>> transitions = new(); 
    public FSM<T> StateMachine
    {
        get
        {
            return fsm;
        }
        set
        {
            fsm = value;
        }
    }

    public virtual void Enter()
    {
        
    }
    public virtual void Execute()
    {
        Debug.Log("Executing " + GetType().Name);
    }
    public virtual void Exit()
    {
        Debug.Log("Exiting " + GetType().Name);
    }

    public IState<T> GetTransition(T input)
    {
        if (transitions.ContainsKey(input))
        {
            return transitions[input];
        } else
        {
            Debug.LogWarning("No se encontró  válida para el input: " + input);
            return null;
        }
    }
    public void AddTransition(T input, IState<T> state)
    {
        transitions[input] = state;
    }
    public void RemoveTransitionByInput(T input)
    {
        if (transitions.ContainsKey(input))
        {
            transitions.Remove(input);
        }
    }

    public virtual void Initialize(params object[] args)
    {

    }
    public void RemoveTransitionByState(IState<T> state)
    {
        foreach (var transition in transitions)
        {
            if (transition.Value == state)
            {
                transitions.Remove(transition.Key);
                break;
            }
        }
    }
}


