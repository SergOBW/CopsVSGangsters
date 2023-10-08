using Abstract;
using DefaultNamespace;
using UnityEngine;

namespace EnemyCore.States
{
    public class FindTargetMonoState : EnemyMonoState
    {
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            if (currentMonoStateMachine.HasTarget(out PlayerCharacter character))
            {
                currentMonoStateMachine.ClearTarget();
            }

            currentMonoStateMachine.ClearSpeed();
            navMeshAgent.destination = currentMonoStateMachine.transform.position;
        }

        public override void UpdateState()
        {
            if (enemyGameObjects.Count > 0)
            {
                if (FindNearestValidEnemy() == null)
                {
                    currentMonoStateMachine.ClearTarget();
                    ExitState(currentMonoStateMachine.idleMonoState);
                    return;
                }
                Transform nearestValidEnemy = FindNearestValidEnemy().transform;
                currentMonoStateMachine.SetTarget(nearestValidEnemy.GetComponent<PlayerCharacter>());
                if (currentMonoStateMachine.HasTarget(out PlayerCharacter character))
                {
                    ExitState(currentMonoStateMachine.chaseMonoState);
                }
            }
            else
            {
                ExitState(currentMonoStateMachine.idleMonoState);
            }
        }


        protected GameObject FindNearestValidEnemy()
        {
            if (enemyGameObjects == null || enemyGameObjects.Count == 0)
            {
                Debug.Log("There is no more enemy");
                return null;
            }

            GameObject nearestEnemy = null;
            float nearestDistanceSqr = float.MaxValue;
            Vector3 currentPosition = currentMonoStateMachine.transform.position;

            foreach (GameObject enemyGameObject in enemyGameObjects)
            {
                if (enemyGameObject == null)
                {
                    continue;
                }

                // Проверяем, есть ли у врага компонент StatsController и он живой
                StatsController statsController = enemyGameObject.GetComponent<StatsController>();
                if (statsController != null && !statsController.IsDead())
                {
                    Vector3 directionToTarget = enemyGameObject.transform.position - currentPosition;
                    float sqrDistanceToTarget = directionToTarget.sqrMagnitude;

                    if (sqrDistanceToTarget < nearestDistanceSqr)
                    {
                        nearestDistanceSqr = sqrDistanceToTarget;
                        nearestEnemy = enemyGameObject;
                    }
                }
            }

            return nearestEnemy;
        }
    }
}