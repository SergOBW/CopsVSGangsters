using System;
using UnityEngine;

namespace Abstract
{
    public abstract class MonoStateMachine : MonoBehaviour, IStateMachine
    {
        public IState CurrentState { get; set; }
        public IState PreviousState { get; set; }
        public event Action<IState,IState> OnStateChangedEvent;


        public abstract void Initialize();

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

        protected void OnStateChanged(IState previousState, IState currentMonoState)
        {
            OnStateChangedEvent?.Invoke(previousState,currentMonoState);
        }

        public virtual void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.UpdateState();
            }
        }

        protected void SetDefaultStates()
        {
            CurrentState = new DefaultMonoState();
            PreviousState = new DefaultMonoState();
        }

    }
}
