using DefaultNamespace;
using UnityEngine;

namespace EnemyCore.States
{
    public class ChaseWithEndPoint : ChaseMonoState
    {
        public override void UpdateState()
        {
            base.UpdateState();
            if (enemyGameObjects.Count > 0)
            {
                if (currentMonoStateMachine.HasTarget(out PlayerCharacter target))
                {
                    StatsController statsContainer = target.GetComponent<StatsController>();
                    lastSeeTargetPosition = target.transform.position;

                    float speed = navMeshAgent.speed + Time.deltaTime;
                    if (speed <= maxSpeed)
                    {
                        navMeshAgent.speed = speed;
                    }
            
                    if (currentMonoStateMachine.IsSeeTarget())
                    {
                        currentMonoStateMachine.LookAtTarget();
                        if (statsContainer == null || statsContainer.isDead)
                        {
                            ExitState(currentMonoStateMachine.findTargetMonoState);
                            return;
                        }
                        float distance = Vector3.Distance(currentMonoStateMachine.transform.position, target.transform.position);
                        float fireRange = currentMonoStateMachine.GetFireRange();
                        float fireRangeOffest = currentMonoStateMachine.GetFireRangeOffest();
                        if (distance <= fireRange - fireRangeOffest)
                        {
                            ExitState(currentMonoStateMachine.attackMonoState);
                            return;
                        }
                    }
                    navMeshAgent.destination = lastSeeTargetPosition;
                }
                return;
            }

            navMeshAgent.destination = SergOBWUtils.GetNearestNavMeshPosition(endPointTransform.position);
        }
    }
}