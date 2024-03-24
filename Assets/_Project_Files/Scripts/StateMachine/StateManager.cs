using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;
    protected bool IsTransitioningState = false;
    protected Stack<EState> StateStack = new Stack<EState>();

    public void AddState(EState stateKey, BaseState<EState> state)
    {
        States[stateKey] = state;
    }

    public void Start()
    {
        InitializeStates();
        if (CurrentState != null)
        {
            TransitionToState(CurrentState.StateKey);
        }
        else
        {
            Debug.LogError("CurrentState is not initialized.");
        }
    }

    public void Update()
    {
        if (!IsTransitioningState && CurrentState != null)
        {
            EState nextStateKey = CurrentState.GetNextState();
            if (!nextStateKey.Equals(CurrentState.StateKey) || CurrentState.ShouldTransition())
            {
                TransitionToState(nextStateKey);
            }
            else
            {
                CurrentState.UpdateState();
            }
        }
    }

    public void TransitionToState(EState stateKey)
    {
        if (IsTransitioningState || CurrentState == null) return;

        if (States.TryGetValue(stateKey, out BaseState<EState> newState))
        {
            IsTransitioningState = true;
            CurrentState.ExitState();
            StateStack.Push(CurrentState.StateKey);
            CurrentState = newState;
            CurrentState.EnterState();
            IsTransitioningState = false;
        }
        else
        {
            Debug.LogError("State not found: " + stateKey);
        }
    }

    public void ReturnToPreviousState()
    {
        if (IsTransitioningState || StateStack.Count == 0 || CurrentState == null) return;

        EState previousStateKey = StateStack.Pop();
        TransitionToState(previousStateKey);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerEnter(other);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerStay(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (CurrentState != null)
        {
            CurrentState.OnTriggerExit(other);
        }
    }

    public void HandleEvent(object eventData)
    {
        if (CurrentState != null)
        {
            CurrentState.HandleEvent(eventData);
        }
    }

    protected abstract void InitializeStates();
}