using Abstract;

namespace EnemyCore.States
{
    public class PauseMonoState : EnemyMonoState
    {
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
            LevelMonoStateMachine.Instance.OnStateChangedEvent += UnPause;
        }

        private void UnPause(IMonoState arg1, IMonoState arg2)
        {
            if (LevelMonoStateMachine.Instance.IsPlayState())
            {
                ExitState(currentMonoStateMachine.PreviousMonoState);
            }
        }

        public override void ExitState(IMonoState monoState)
        {
            LevelMonoStateMachine.Instance.OnStateChangedEvent -= UnPause;
            base.ExitState(monoState);
        }
    }
}