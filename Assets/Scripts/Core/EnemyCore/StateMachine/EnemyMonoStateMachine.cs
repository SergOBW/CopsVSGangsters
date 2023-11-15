using Abstract;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyCore.States
{
    public class EnemyMonoStateMachine : MonoStateMachine
    {
        // Componetns
        public NavMeshAgent navMeshAgent;
        public EnemyWeaponManager weaponManager;
        private EnemyAi enemyAi;
        private EnemyStatsController enemyStatsController;
        private EnemyAnimationManager animationManager;
        private EnemySoundManager _soundManager;

        // States
        public AttackState attackState = new AttackState();
        public ChaseState chaseState = new ChaseState();
        public DeadState deadState = new DeadState();
        public WounderState wounderState = new WounderState();
        public IdleState idleState = new IdleState();
        public PauseState pauseState = new PauseState();
        public FindTargetState findTargetState = new FindTargetState();
        public FindTargetWithEndPoint findTargetWithEndPoint = new FindTargetWithEndPoint();
        public ChaseWithEndPoint chaseWithEndPoint = new ChaseWithEndPoint();

        private Transform targetTransform;
        private Transform endPoint;
        
        public override void Initialize()
        {
            GetComponents();
            
            enemyStatsController.OnEnemyDie += OnOnEnemyDie;
            if (LevelStateMachine.Instance != null)
            {
                LevelStateMachine.Instance.OnLevelPaused += OnLevelPaused;
            }
            if (enemyAi.GetEndPoint() != null)
            {
                endPoint = enemyAi.GetEndPoint();
                findTargetState = findTargetWithEndPoint;
                chaseState = chaseWithEndPoint;
            }
            // Start in the Idle state
            ChangeState(idleState);
        }

        public void DeInitialize()
        {
            enemyStatsController.OnEnemyDie -= OnOnEnemyDie;
            if (LevelStateMachine.Instance != null)
            {
                LevelStateMachine.Instance.OnLevelPaused -= OnLevelPaused;
            }

            SetDefaultStates();
        }

        private void OnLevelPaused()
        {
            if (!LevelStateMachine.Instance.IsPlayState() && CurrentState != pauseState && CurrentState != deadState)
            {
                ChangeState(pauseState);
            }
        }

        public override void ChangeState(IState monoState)
        {
            if (enemyStatsController.isDead)
            {
                if (CurrentState != null)
                {
                    PreviousState = CurrentState;
                }

                CurrentState = deadState;
                CurrentState.EnterState(this);
                OnStateChanged(PreviousState, CurrentState);
                return;
            }
            base.ChangeState(monoState);
        }
        

        public override void Update()
        {
            base.Update();
            if (animationManager != null)
            {
                if (navMeshAgent.velocity.magnitude > enemyAi.GetDefaultSpeed() + 0.5)
                {
                    animationManager.SetSpeed(2);
                }
                else
                {
                    animationManager.SetSpeed(navMeshAgent.velocity.magnitude/navMeshAgent.speed);
                }
            }
        }

        private void OnOnEnemyDie(EnemyStatsController obj)
        {
            ChangeState(deadState);
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        public Vector3 GetCenterOfWounder()
        {
            return transform.position;
        }

        public Vector2 GetSizeOfWounder()
        {
            return new Vector2(10, 10);
        }

        public void SetTarget(Transform transform)
        {
            targetTransform = transform;
        }

        public void ClearTarget()
        {
            targetTransform = null;
        }
        
        public bool HasTarget(out Transform transform)
        {
            transform = targetTransform;
            
            return transform != null;
        }

        public float GetFireRange()
        {
            return weaponManager.GetShootingRange();
        }

        public float GetAttackSpeed()
        {
            return weaponManager.GetAttackSpeed();
        }
        
        public float GetFireRangeOffest()
        {
            return weaponManager.GetDistanceOffest();
        }

        public void Attack(StatsController statsController)
        {
            if (statsController.isDead || !LevelStateMachine.Instance.IsPlayState())
            {
                return;
            }
            GameObject sender = gameObject;
            if (sender != null)
            {
                bl_DamageInfo info = new bl_DamageInfo(weaponManager.GetDamage());
                info.Sender = sender;
                sender.SetIndicator(Color.red);
                statsController.transform.GetComponent<bl_DamageCallback>().OnDamage(info);
            }
            statsController.TakeDamage(weaponManager.GetDamage());
            _soundManager.PlayAttack();
            weaponManager.Shoot();
            animationManager.ShootReaction();
            
        }
        
        public Transform GetEndPoint()
        {
            return endPoint;
        }

        private void GetComponents()
        {
            enemyAi = GetComponent<EnemyAi>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            weaponManager = GetComponent<EnemyWeaponManager>();
            enemyStatsController = GetComponent<EnemyStatsController>();
            animationManager = GetComponent<EnemyAnimationManager>();
            _soundManager = GetComponent<EnemySoundManager>();
        }

        public void LookAtTarget()
        {
            if (targetTransform == null)
            {
                Debug.LogError("There is no target Character");
                return;
            }
            Quaternion lookRotation = Quaternion.LookRotation(targetTransform.position - transform.position);
            float time = 2 * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
        }

        public bool IsSeeTarget()
        {
            if (targetTransform != null)
            {
                return enemyAi.IsSeeTarget(targetTransform.gameObject);
            }
            return false;
        }

        public Sensor GetAiSensor()
        {
            return enemyAi.GetAiSensor();
        }

        public void ClearSpeed()
        {
            enemyAi.ClearSpeed();
        }

        public float GetMaxSpeed()
        {
            return enemyAi.GetMaxSpeed();
        }

        public float GetDefaultSpeed()
        {
            return enemyAi.GetDefaultSpeed();
        }
    }
}