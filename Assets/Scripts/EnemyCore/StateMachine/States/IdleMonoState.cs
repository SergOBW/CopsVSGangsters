
using EnemyCore.States;

public class IdleMonoState : EnemyMonoState
{
    public override void UpdateState()
    {
        base.UpdateState();
        if (enemyGameObjects.Count > 0)
        {
            ExitState(currentMonoStateMachine.findTargetMonoState);
        }
        else if (currentMonoStateMachine.GetEndPoint() != null)
        {
            ExitState(currentMonoStateMachine.findTargetMonoState);
        }
        else if(enemyGameObjects.Count <= 0)
        {
            ExitState(currentMonoStateMachine.wounderMonoState);
        }
    }
}