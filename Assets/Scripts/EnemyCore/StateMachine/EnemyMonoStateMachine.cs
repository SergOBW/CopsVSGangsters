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
        private StateMachineDebugger enemyDebugUi;
        private EnemyStatsController enemyStatsController;
        private EnemyAnimationManager animationManager;

        // States
        public AttackMonoState attackMonoState = new AttackMonoState();
        public ChaseMonoState chaseMonoState = new ChaseMonoState();
        public DeadMonoState deadMonoState = new DeadMonoState();
        public WounderMonoState wounderMonoState = new WounderMonoState();
        public IdleMonoState idleMonoState = new IdleMonoState();
        public PauseMonoState pauseMonoState = new PauseMonoState();
        public FindTargetMonoState findTargetMonoState = new FindTargetMonoState();
        public FindTargetMonoWithEndPoint findTargetMonoWithEndPoint = new FindTargetMonoWithEndPoint();
        public ChaseWithEndPoint chaseWithEndPoint = new ChaseWithEndPoint();

        private PlayerCharacter targetCharacter;
        private Transform endPoint;
        
        public override void Initialize()
        {
            GetComponents();
            
            enemyStatsController.OnEnemyDie += OnOnEnemyDie;
            if (LevelMonoStateMachine.Instance != null)
            {
                LevelMonoStateMachine.Instance.OnLevelPaused += OnLevelPaused;
            }
            if (enemyAi.GetEndPoint() != null)
            {
                endPoint = enemyAi.GetEndPoint();
                findTargetMonoState = findTargetMonoWithEndPoint;
                chaseMonoState = chaseWithEndPoint;
            }
            // Start in the Idle state
            ChangeState(idleMonoState);
            if (enemyDebugUi != null)
            {
                enemyDebugUi.Initialize(this);
            }
        }

        public void DeInitialize()
        {
            enemyStatsController.OnEnemyDie -= OnOnEnemyDie;
            if (LevelMonoStateMachine.Instance != null)
            {
                LevelMonoStateMachine.Instance.OnLevelPaused -= OnLevelPaused;
            }

            SetDefaultStates();
        }

        private void OnLevelPaused()
        {
            if (!LevelMonoStateMachine.Instance.IsPlayState() && CurrentMonoState != pauseMonoState && CurrentMonoState != deadMonoState)
            {
                ChangeState(pauseMonoState);
            }
        }

        public override void ChangeState(IMonoState monoState)
        {
            if (enemyStatsController.isDead)
            {
                if (CurrentMonoState != null)
                {
                    PreviousMonoState = CurrentMonoState;
                }

                CurrentMonoState = deadMonoState;
                CurrentMonoState.EnterState(this);
                OnStateChanged(PreviousMonoState, CurrentMonoState);
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
                //animationManager.SetSpeed(navMeshAgent.speed);
            }
        }

        private void OnOnEnemyDie(EnemyStatsController obj)
        {
            ChangeState(deadMonoState);
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

        public void SetTarget(PlayerCharacter character)
        {
            targetCharacter = character;
        }

        public void ClearTarget()
        {
            targetCharacter = null;
        }
        
        public bool HasTarget(out PlayerCharacter character)
        {
            character = targetCharacter;

            return character != null;
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
            if (statsController.isDead)
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
            enemyDebugUi = GetComponentInChildren<StateMachineDebugger>();
        }

        public void LookAtTarget()
        {
            if (targetCharacter == null)
            {
                Debug.LogError("There is no target Character");
                return;
            }
            //enemyAi.LookAtTarget(targetTransfrom);
            Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
            float time = 2 * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
        }

        public bool IsSeeTarget()
        {
            if (targetCharacter != null)
            {
                return enemyAi.IsSeeTarget(targetCharacter.gameObject);
            }
            return false;
        }

        public AiSensor GetAiSensor()
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