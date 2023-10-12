using Abstract;

namespace EnemyCore.States
{
    public class PauseState : EnemyState
    {
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
            LevelStateMachine.Instance.OnStateChangedEvent += UnPause;
        }

        private void UnPause(IState arg1, IState arg2)
        {
            if (LevelStateMachine.Instance.IsPlayState())
            {
                ExitState(currentMonoStateMachine.PreviousState);
            }
        }

        public override void ExitState(IState monoState)
        {
            LevelStateMachine.Instance.OnStateChangedEvent -= UnPause;
            base.ExitState(monoState);
        }
    }
}