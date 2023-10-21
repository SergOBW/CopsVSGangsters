using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyCore

{
    public class EnemyStatsController : StatsController
    {
        private EnemyAnimationManager _animationManager;
        
        
        public event Action<float> OnHealthChanged;
        public event Action<EnemyStatsController> OnEnemyDie;
        public event Action<EnemyStatsController> OnEnemyReadyToSpawn;
        
        private bool[] wasEnabled;
        private Guid _guid;

        private Stats _enemyStats;
        private EnemyVisualsController _enemyVisualsController;
        

        public override void Initialize(Stats stats)
        {
            _enemyStats = stats;
            _guid = Guid.NewGuid();
            transform.name = _guid.ToString();
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
            currentHealth = _enemyStats.startHealth;
            isDead = _enemyStats.isDead;
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
