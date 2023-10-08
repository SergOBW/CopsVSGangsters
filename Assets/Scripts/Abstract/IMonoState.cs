using System;
using Abstract;

public interface IMonoState
{
    public Type Type
    {
        get => GetType();
    }
    public void EnterState(MonoStateMachine monoStateMachine);
    public void UpdateState();
    public void ExitState(IMonoState monoState);
}
