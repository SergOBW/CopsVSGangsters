using Abstract;
using EnemyCore.States;

public class DeadState : EnemyState
{
    public override void EnterState(IStateMachine monoStateMachine)
    {
        base.EnterState(monoStateMachine);
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }
}
