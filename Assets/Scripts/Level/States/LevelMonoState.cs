using Abstract;

namespace Level.States
{
    public class LevelMonoState : IMonoState
    {
        protected LevelMonoStateMachine currentMonoStateMachine;
        public virtual void EnterState(MonoStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as  LevelMonoStateMachine;
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState(IMonoState monoState)
        {
            currentMonoStateMachine.ChangeState(monoState);
        }
    }
}