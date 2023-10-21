using System;

namespace Abstract
{
    public interface IState
    {
        public Type Type
        {
            get => GetType();
        }
        public void EnterState(IStateMachine stateMachine);
        public void UpdateState();
        public void ExitState(IState IState);
    }
}