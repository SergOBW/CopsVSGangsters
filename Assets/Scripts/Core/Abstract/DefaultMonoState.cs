namespace Abstract
{
    public class DefaultMonoState : IState
    {
        public IStateMachine CurrentMonoStateMachine { get; set; }

        public void EnterState(IStateMachine stateMachine)
        {
            CurrentMonoStateMachine = stateMachine;
        }

        public void UpdateState()
        {
            
        }

        public void ExitState(IState IState)
        {
            CurrentMonoStateMachine.ChangeState(IState);
        }
        
    }
}