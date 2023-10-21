using System;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class StatsController : MonoBehaviour
    {
        public bool isDead;

        public float currentHealth;

        public float armour;

        public virtual void Initialize(Stats stats)
        {
            isDead = false;
        }
        
        public virtual void SetDefaults()
        {
            
        }

        public virtual void Heal(float amount)
        {
            
        }
        
        public virtual void TakeDamage(float damage)
        {
            
        }
        
        public virtual void Die()
        {
            
        }

        public virtual bool IsDead()
        {
            return isDead;
        }

        public void OnStatsDeadEvent()
        {
            OnStatsDead?.Invoke(this);
        }
        public event Action<StatsController> OnStatsDead;

    }
}