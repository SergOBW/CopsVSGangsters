using Abstract;
using DefaultNamespace;
using EnemyCore.States;
using UnityEngine;

public class ChaseState : EnemyState
{
    protected Vector3 lastSeeTargetPosition;
    protected float maxSpeed = 4;


    public override void EnterState(IStateMachine monoStateMachine)
    {
        base.EnterState(monoStateMachine);
        if (currentMonoStateMachine.PreviousState != currentMonoStateMachine.pauseState)
        {
            navMeshAgent.speed = currentMonoStateMachine.GetDefaultSpeed();
        }
        maxSpeed = currentMonoStateMachine.GetMaxSpeed();
        navMeshAgent.isStopped = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
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
                    ExitState(currentMonoStateMachine.findTargetState);
                    return;
                }
                float distance = Vector3.Distance(currentMonoStateMachine.transform.position, target.transform.position);
                float fireRange = currentMonoStateMachine.GetFireRange();
                float fireRangeOffest = currentMonoStateMachine.GetFireRangeOffest();
                if (distance <= fireRange - fireRangeOffest)
                {
                    ExitState(currentMonoStateMachine.attackState);
                    return;
                }
            }
            navMeshAgent.destination = lastSeeTargetPosition;
        }
        else
        {
            ExitState(currentMonoStateMachine.findTargetState);
        }
    }
}