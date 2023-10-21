using System;

namespace Abstract
{
    public interface IStateMachine
    {
        public IState CurrentState { get; set; }
        public IState PreviousState { get; set; }
    
        public event Action<IState,IState> OnStateChangedEvent;
        
        public void Initialize();

        public void ChangeState(IState monoState);
    }
}