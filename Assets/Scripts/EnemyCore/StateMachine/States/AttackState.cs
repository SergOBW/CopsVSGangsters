using Abstract;
using DefaultNamespace;
using EnemyCore.States;
using UnityEngine;

public class AttackState : EnemyState
{
    private float attackSpeed;
    private float timer;
    public override void EnterState(IStateMachine monoStateMachine)
    {
        base.EnterState(monoStateMachine);
        attackSpeed = currentMonoStateMachine.GetAttackSpeed();
        timer = attackSpeed;
        navMeshAgent.destination = currentMonoStateMachine.transform.position;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (currentMonoStateMachine.HasTarget(out PlayerCharacter target))
        {
            StatsController statsContainer = target.GetComponent<StatsController>();
            
            if (statsContainer == null || statsContainer.isDead)
            {
                ExitState(currentMonoStateMachine.findTargetState);
                return;
            }
            
            float distance = Vector3.Distance(currentMonoStateMachine.transform.position, target.transform.position);
            float fireRange = currentMonoStateMachine.GetFireRange();
                
            currentMonoStateMachine.LookAtTarget();
            
            if (currentMonoStateMachine.IsSeeTarget())
            {
                if (distance <= fireRange)
                {
                    timer += Time.deltaTime;
                    TryToAttack(statsContainer);
                }
                else ExitState(currentMonoStateMachine.chaseState);
            }
            else
            {
                ExitState(currentMonoStateMachine.findTargetState);
            }
            
        }
        else
        {
            ExitState(currentMonoStateMachine.findTargetState);
        }
    }

    private void TryToAttack(StatsController statsController)
    {
        if (timer >= attackSpeed)
        {
            currentMonoStateMachine.Attack(statsController);
            timer = 0f;
        }
    }
}