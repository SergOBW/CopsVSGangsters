using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerStatsController : StatsController
    {
        private bool isImmune;

        private Stats _statsData;

        public event Action<float> OnHealthChanged;
        public event Action<float> OnArmourChanged;
        public event Action OnPlayerDie;

        private bool[] wasEnabled;

        private bool haveArmourBonus;
        
        public override void Initialize(Stats stats)
        {
            if (stats == null)
            {
                stats = new Stats();
            }

            _statsData = stats;
            SetDefaults();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(50f);
            }
        }
        

        public override void SetDefaults()
        {
            currentHealth = _statsData.startHealth;
            isDead = _statsData.isDead;
            armour = _statsData.startArmour;
            isImmune = false;
            OnHealthChanged?.Invoke(currentHealth);
            OnArmourChanged?.Invoke(armour);
        }
        public override void TakeDamage(float damage)
        {
            if (isDead || isImmune)
            {
                return;
            }
            
            if (!isDead)
            {
                if (armour > 0)
                {
                    if (armour - damage < 0)
                    {
                        var damageToHp = Math.Abs(armour - damage);
                        currentHealth -= damageToHp;
                    }
                    armour -= damage;
                }
                else
                {
                    currentHealth -= damage;  
                }
            }

            if (currentHealth <= 0)
            {
                Die();
            }
            OnHealthChanged?.Invoke(currentHealth);
            OnArmourChanged?.Invoke(armour);
        }

        public override void Die()
        {
            isDead = true;
            OnPlayerDie?.Invoke();
        }
        
        public override void Heal(float amount)
        {
            base.Heal(amount);
            currentHealth = _statsData.startHealth;
            OnHealthChanged?.Invoke(currentHealth);
        }

        public void ReviveBonus()
        {
            SetDefaults();
            StartCoroutine(ReviveImmuneTimer());
        }

        IEnumerator ReviveImmuneTimer()
        {
            isImmune = true;
            yield return new WaitForSeconds(3);
            SetDefaults();
        }

        public bool IsHpBonus()
        {
            float percent = (currentHealth / _statsData.startHealth) * 100;
            return percent >= 50;
        }

        public float GetStartedHealth()
        {
            return _statsData.startHealth;
        }
    }
}
