using Abstract;

namespace Level.States
{
    public class LevelMonoState : IState
    {
        protected LevelStateMachine currentMonoStateMachine;
        public virtual void EnterState(IStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as  LevelStateMachine;
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState(IState IState)
        {
            currentMonoStateMachine.ChangeState(IState);
        }
    }
}