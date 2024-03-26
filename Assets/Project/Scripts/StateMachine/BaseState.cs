using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{

    public EState StateKey { get; private set; }

    public BaseState(EState stateKey)
    {
        StateKey = stateKey;
    }

    public abstract void Initialize(StateManager<EState> stateManager);

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract EState GetNextState();

    public abstract void OnTriggerEnter(Collider other);

    public abstract void OnTriggerStay(Collider other);

    public abstract void OnTriggerExit(Collider other);

    public abstract void HandleEvent(object eventData);

    public abstract bool ShouldTransition();
}