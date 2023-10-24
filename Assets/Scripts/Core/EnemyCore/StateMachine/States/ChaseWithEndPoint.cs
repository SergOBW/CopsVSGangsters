using Abstract;
using DefaultNamespace;
using UnityEngine;

namespace EnemyCore.States
{
    public class ChaseWithEndPoint : ChaseState
    {
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            navMeshAgent.isStopped = false;
        }

        public override void UpdateState()
        {
            if (enemyGameObjects.Count > 0)
            {
                if (currentMonoStateMachine.HasTarget(out Transform transform))
                {
                    StatsController statsContainer = transform.GetComponent<StatsController>();
                    lastSeeTargetPosition = transform.position;

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
                            ExitState(currentMonoStateMachine.findTargetState);
                            return;
                        }
                        float distance = Vector3.Distance(currentMonoStateMachine.transform.position, transform.position);
                        float fireRange = currentMonoStateMachine.GetFireRange();
                        float fireRangeOffest = currentMonoStateMachine.GetFireRangeOffest();
                        if (distance <= fireRange - fireRangeOffest)
                        {
                            ExitState(currentMonoStateMachine.attackState);
                            return;
                        }
                    }
                    navMeshAgent.destination = SergOBWUtils.GetNearestNavMeshPosition(lastSeeTargetPosition,5);
                }
                else
                {
                    ExitState(currentMonoStateMachine.findTargetWithEndPoint);
                }
                return;
            }
            
            navMeshAgent.destination = SergOBWUtils.GetNearestNavMeshPosition(endPointTransform.position,5);
        }
    }
}