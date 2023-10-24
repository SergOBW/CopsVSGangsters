using UnityEngine;

namespace EnemyCore.States
{
    public class FindTargetWithEndPoint : FindTargetState
    {
        public override void UpdateState()
        {
            navMeshAgent.isStopped = true;
            if (enemyGameObjects.Count > 0)
            {
                currentMonoStateMachine.ClearTarget();
                currentMonoStateMachine.SetTarget(FindNearestValidEnemy().transform);
                if (currentMonoStateMachine.HasTarget(out Transform transform))
                {
                    ExitState(currentMonoStateMachine.chaseState);
                }
                return;
            }
            ExitState(currentMonoStateMachine.chaseState);
        }
    }
}