using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyCore

{
    internal class EnemyStatsContainer : StatsContainer
    {
        public Guid guid;

        public EnemyStatsContainer(StatsSo statsSo) : base(statsSo)
        {
            guid = Guid.NewGuid();
            isDead = false;
        }
    }
    
    
    public class EnemyStatsController : StatsController
    {
        private EnemyAnimationManager _animationManager;
        
        
        public event Action<float> OnHealthChanged;
        public event Action<EnemyStatsController> OnEnemyDie;
        public event Action<EnemyStatsController> OnEnemyReadyToSpawn;
        
        private bool[] wasEnabled;

        private EnemyStatsContainer enemyStatsContainer;
        private EnemyVisualsController _enemyVisualsController;
        

        public override void Initialize(StatsSo statsSo)
        {
            enemyStatsContainer = new EnemyStatsContainer(statsSo);
            transform.name = enemyStatsContainer.guid.ToString();
            SetDefaults();
            //EnemyManager.Instance.RegisterEnemyAi(this);
            _animationManager = GetComponent<EnemyAnimationManager>();
            _enemyVisualsController = GetComponentInChildren<EnemyVisualsController>();
            if (_enemyVisualsController != null)
            {
                _enemyVisualsController.InitializeHits(this);
            }
            if (EnemyHandleMechanic.Instance != null)
            {
                EnemyHandleMechanic.Instance.Register(this);
            }
        }
        
        
        
        public override void SetDefaults()
        {
            currentHealth = enemyStatsContainer.startHealth;
            isDead = enemyStatsContainer.isDead;
            var capsuleColliders = GetComponents<CapsuleCollider>();
            foreach (var collider in capsuleColliders)
            {
                collider.enabled = true;
            }

            OnHealthChanged?.Invoke(currentHealth);
            OnEnemyReadyToSpawn?.Invoke(this);
        }

        public override void TakeDamage(float damage)
        {
            if (isDead)
            {
                return;
            }
            currentHealth -= damage;
            if (_enemyVisualsController != null)
            {
                _enemyVisualsController.Hit(damage);
            }
            if (currentHealth <= 0)
            {
                Die();
                return;
            }
            OnHealthChanged?.Invoke(currentHealth);
            if (_animationManager!= null)
            {
                _animationManager.HitReaction();
            }
        }
        
        public override void Die()
        {
            isDead = true;
            if (_animationManager != null)
            {
                _animationManager.DieReaction();
            }
            if (EnemyHandleMechanic.Instance != null)
            {
                EnemyHandleMechanic.Instance.UnRegister(this);
            }

            if (TryGetComponent(out CapsuleCollider collider))
            {
                collider.enabled = false;
            }
            OnEnemyDie?.Invoke(this);
            GetComponent<EnemySoundManager>().PlayDieSound();
            GetComponent<NavMeshAgent>().enabled = false;
            OnStatsDeadEvent();
            StartCoroutine(DestroyGameObject());
        }
        
        IEnumerator DestroyGameObject()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        public override bool IsDead()
        {
            return isDead;
        }
    }
    
}
