
using EnemyCore.States;

public class IdleState : EnemyState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (enemyGameObjects.Count > 0)
        {
            ExitState(currentMonoStateMachine.findTargetState);
        }
        else if (currentMonoStateMachine.GetEndPoint() != null)
        {
            ExitState(currentMonoStateMachine.findTargetState);
        }
        else if(enemyGameObjects.Count <= 0)
        {
            ExitState(currentMonoStateMachine.wounderState);
        }
    }
}