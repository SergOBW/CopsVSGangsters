using System;

namespace Abstract
{
    public class StateMachine : IStateMachine
    {
        public IState CurrentState { get; set; }
        public IState PreviousState { get; set; }
        public event Action<IState, IState> OnStateChangedEvent;
        public virtual void Initialize()
        {
            
        }
        
        public virtual void ChangeState(IState monoState)
        {
            if (CurrentState != null)
            {
                PreviousState = CurrentState;
            }

            CurrentState = monoState;
            CurrentState.EnterState(this);
            OnStateChangedEvent?.Invoke(PreviousState,CurrentState);
        }

        public virtual void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.UpdateState();
            }
        }
    }
}