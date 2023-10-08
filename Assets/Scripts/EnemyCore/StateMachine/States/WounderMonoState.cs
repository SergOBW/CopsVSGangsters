using Abstract;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace EnemyCore.States
{
    public class WounderMonoState : EnemyMonoState
    {
        public Vector3 center;           // Центр квадрата/прямоугольника.
        public Vector2 size;             // Размеры квадрата/прямоугольника.
        public float wanderTimer = 5f;   // Время ожидания перед выбором следующей точки.

        private Transform target;       // Текущая целевая точка.
        private NavMeshAgent agent;
        private float timer;

        private float _agroLevel;
        private float _maxAgroLevel = 3;

        #region EnemyStateMachine

        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            _agroLevel = 0;
            wanderTimer = Random.Range(1, wanderTimer);
            timer = wanderTimer;
            if (currentMonoStateMachine == null)
            {
                Debug.Log("Some shit");
                return;
            }
            center = currentMonoStateMachine.GetCenterOfWounder();
            size = currentMonoStateMachine.GetSizeOfWounder();
            currentMonoStateMachine.ClearSpeed();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (enemyGameObjects.Count > 0)
            {
                if (characterInRange == null)
                {
                    characterInRange = enemyGameObjects[0].GetComponent<PlayerCharacter>();
                    characterInRange.OnFireEvent += OnTargetFire;
                }
                _agroLevel += Time.deltaTime;
                if (_agroLevel >= _maxAgroLevel)
                {
                    currentMonoStateMachine.SetTarget(enemyGameObjects[0].GetComponent<PlayerCharacter>());
                    ExitState(currentMonoStateMachine.chaseMonoState);
                    return;
                }
            }
            
            // Если таймер превышает заданное время wanderTimer, выбираем новую целевую точку.
            timer += Time.deltaTime;
            navMeshAgent.isStopped = false;
            if (timer >= wanderTimer)
            {
                Vector3 newPos = GetRandomPointInBounds();
                navMeshAgent.SetDestination(newPos);
                timer = 0;
            }
        }

        protected override void OnTargetFire()
        {
            base.OnTargetFire();
            _agroLevel += _maxAgroLevel;
        }

        #endregion

        #region Utilities

        // Метод для получения случайной точки в пределах квадрата/прямоугольника.
        Vector3 GetRandomPointInBounds()
        {
            Vector3 randomPoint = center + new Vector3(Random.Range(-size.x / 2f, size.x / 2f), 0f, Random.Range(-size.y / 2f, size.y / 2f));

            NavMeshHit navHit;
            NavMesh.SamplePosition(randomPoint, out navHit, 1f, NavMesh.AllAreas);

            return navHit.position;
        }

        #endregion
    }
}