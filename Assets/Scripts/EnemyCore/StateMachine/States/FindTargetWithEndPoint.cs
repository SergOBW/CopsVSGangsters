namespace EnemyCore.States
{
    public class FindTargetWithEndPoint : FindTargetState
    {
        public override void UpdateState()
        {
            base.UpdateState();
            navMeshAgent.isStopped = true;
            if (enemyGameObjects.Count > 0)
            {
                currentMonoStateMachine.ClearTarget();
                currentMonoStateMachine.SetTarget(FindNearestValidEnemy().GetComponent<PlayerCharacter>());
                if (currentMonoStateMachine.HasTarget(out PlayerCharacter character))
                {
                    ExitState(currentMonoStateMachine.chaseState);
                }
                return;
            }
            ExitState(currentMonoStateMachine.chaseState);
        }
    }
}