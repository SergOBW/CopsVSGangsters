namespace EnemyCore.States
{
    public class FindTargetMonoWithEndPoint : FindTargetMonoState
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
                    ExitState(currentMonoStateMachine.chaseMonoState);
                }
                return;
            }
            ExitState(currentMonoStateMachine.chaseMonoState);
        }
    }
}