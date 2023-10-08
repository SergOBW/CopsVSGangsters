using System;

namespace Abstract
{
    public interface IStateMachine
    {
        public IState currentState { get; set;  }
        public IState previousState { get; set;}
    
        public event Action<IState,IState> OnStateChanged;
        
        public void Initialize();

        public void ChangeState(IState monoState);
    }
}