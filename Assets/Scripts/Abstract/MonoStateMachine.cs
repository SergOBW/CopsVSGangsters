using System;
using UnityEngine;

namespace Abstract
{
    public abstract class MonoStateMachine : MonoBehaviour
    {
        public IMonoState CurrentMonoState { get; protected set; }
        public IMonoState PreviousMonoState { get; protected set; }
    
        public event Action<IMonoState,IMonoState> OnStateChangedEvent;


        public abstract void Initialize();

        public virtual void ChangeState(IMonoState monoState)
        {
            if (CurrentMonoState != null)
            {
                PreviousMonoState = CurrentMonoState;
            }

            CurrentMonoState = monoState;
            CurrentMonoState.EnterState(this);
            OnStateChangedEvent?.Invoke(PreviousMonoState,CurrentMonoState);
        }

        protected void OnStateChanged(IMonoState previousState, IMonoState currentMonoState)
        {
            OnStateChangedEvent?.Invoke(previousState,currentMonoState);
        }

        public virtual void Update()
        {
            if (CurrentMonoState != null)
            {
                CurrentMonoState.UpdateState();
            }
        }

        protected void SetDefaultStates()
        {
            CurrentMonoState = new DefaultMonoState();
            PreviousMonoState = new DefaultMonoState();
        }

    }
}
