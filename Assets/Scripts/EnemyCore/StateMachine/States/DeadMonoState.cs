using Abstract;
using EnemyCore.States;

public class DeadMonoState : EnemyMonoState
{
    public override void EnterState(MonoStateMachine monoStateMachine)
    {
        base.EnterState(monoStateMachine);
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }
}
