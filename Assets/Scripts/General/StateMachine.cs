using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState;
    private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();

    public IState CurrentState => currentState;

    public void RegisterState<T>(T state) where T : IState
    {
        states[typeof(T)] = state;
    }

    public void ChangeState<T>() where T : IState
    {
        if (states.TryGetValue(typeof(T), out var newState))
        {
            if (newState == currentState) return;
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
        else
        {
            Debug.LogError($"State {typeof(T).Name} not registered!");
        }
    }

    public void Update()
    {        
        currentState?.Tick();
    }
}
