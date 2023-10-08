using Abstract;
using DefaultNamespace;
using EnemyCore.States;
using UnityEngine;

public class ChaseMonoState : EnemyMonoState
{
    protected Vector3 lastSeeTargetPosition;
    protected float maxSpeed = 4;


    public override void EnterState(MonoStateMachine monoStateMachine)
    {
        base.EnterState(monoStateMachine);
        if (currentMonoStateMachine.PreviousMonoState != currentMonoStateMachine.pauseMonoState)
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
        else
        {
            ExitState(currentMonoStateMachine.findTargetMonoState);
        }
    }
}