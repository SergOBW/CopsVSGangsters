using DefaultNamespace;
using UnityEngine;

namespace Sctructures
{
    public class SctructuresStatsController : StatsController
    {
        [SerializeField]private StatsSo statsSo;

        private void Awake()
        {
            Initialize(statsSo);
        }

        public override void Initialize(StatsSo statsSo)
        {
            isDead = false;
            currentHealth = this.statsSo.startHealth;
            
        }

        public override void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0f)
            {
                OnStatsDeadEvent();
                isDead = true;
                Destroy(gameObject);
            }
        }
    }
}