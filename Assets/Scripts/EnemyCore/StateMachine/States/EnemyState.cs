using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyCore.States
{
    public abstract class EnemyState : IState
    {
        protected EnemyMonoStateMachine currentMonoStateMachine;
        protected AiSensor aiSensor;
        protected List<GameObject> enemyGameObjects = new List<GameObject>();
        protected NavMeshAgent navMeshAgent;
        protected PlayerCharacter characterInRange;
        protected Transform endPointTransform;
        
        public virtual void EnterState(IStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as EnemyMonoStateMachine;
            if (currentMonoStateMachine == null)
            {
                Debug.Log("Error at this point");
                return;
            }

            endPointTransform = currentMonoStateMachine.GetEndPoint();
            aiSensor = currentMonoStateMachine.GetAiSensor();
            enemyGameObjects = aiSensor.objects;
            navMeshAgent = currentMonoStateMachine.GetNavMeshAgent();
            SubscribeOnEvents();
            if (LevelStateMachine.Instance == null)
            {
                return;
            }
            if (!LevelStateMachine.Instance.IsPlayState() && this != currentMonoStateMachine.pauseState && currentMonoStateMachine.CurrentState != currentMonoStateMachine.deadState)
            {
                ExitState(currentMonoStateMachine.pauseState);
            }
        }

        public virtual void UpdateState()
        {
            if (enemyGameObjects.Count <= 0)
            {
                currentMonoStateMachine.ClearTarget();
            }
        } 

        public virtual void ExitState(IState monoState)
        {
            UnSubscribeOnEvents();
            if (currentMonoStateMachine != null)
            {
                currentMonoStateMachine.ChangeState(monoState);
            }
        }

        protected virtual void SubscribeOnEvents()
        {
            if (enemyGameObjects.Count > 0)
            {
                enemyGameObjects[0].gameObject.TryGetComponent(out PlayerCharacter targetCharacter);
                {
                    characterInRange = targetCharacter;
                    characterInRange.OnFireEvent += OnTargetFire;
                }
            }
        }

        protected virtual void OnTargetFire()
        {
            
        }

        protected virtual void UnSubscribeOnEvents()
        {
            if (characterInRange != null)
            {
                characterInRange.OnFireEvent -= OnTargetFire;
            }
        }

        protected virtual void OnEnemyDetected(Transform enemy)
        {
            
        }
        
        protected virtual void OnEnemyLost(Transform enemy)
        {
            if (enemy == currentMonoStateMachine.HasTarget(out PlayerCharacter character))
            {
                currentMonoStateMachine.ClearTarget();
            }
        }
    }
}