using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    
    public class PlayerStatsContainer : StatsContainer
    {
        public float heathRestoreSpeed;
        public float heathRestoreCount;

        public PlayerStatsContainer(PlayerStatsSo playerStatsSo) : base(playerStatsSo)
        {
            heathRestoreSpeed = playerStatsSo.heathRestoreSpeed;
            heathRestoreCount = playerStatsSo.heathRestoreCount;
        }
    }
    public class PlayerStatsController : StatsController
    {
        private float lastTakeDamage;
        private float timer;
        private bool isImmune;

        private PlayerStatsContainer statsContainerData;
        [SerializeField] private StatsSo defaultStatsSo;

        public event Action<float> OnHealthChanged;
        public event Action<float> OnArmourChanged;
        public event Action OnPlayerDie;

        private bool[] wasEnabled;

        private bool haveArmourBonus;
        [SerializeField] private bool isSelfInitialize;

        private void Awake()
        {
            if (isSelfInitialize)
            {
                Initialize();
            }
        }

        public override void Initialize(StatsSo statsSo = null)
        {
            if (statsSo == null)
            {
                statsSo = defaultStatsSo;
            }
            // Was in Awake
            lastTakeDamage = 0f;
            timer = 0f;
            
            // 
            statsContainerData = new PlayerStatsContainer(statsSo as PlayerStatsSo);
            SetDefaults();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(50f);
            }
            
            
            if (currentHealth < statsContainerData.startHealth)
            {
                if (timer - lastTakeDamage > 3f)
                {
                    float needToFull = statsContainerData.startHealth - currentHealth;
                    if (needToFull <= statsContainerData.heathRestoreCount)
                    {
                        currentHealth = statsContainerData.startHealth;
                    }
                    else
                    {
                        float perFrame = statsContainerData.heathRestoreCount * Time.deltaTime * statsContainerData.heathRestoreSpeed;
                        currentHealth += perFrame;
                    }
                    OnHealthChanged.Invoke(currentHealth);
                }
            }
            
            if (armour < statsContainerData.Startarmour && haveArmourBonus)
            {
                if (timer - lastTakeDamage > 3f)
                {
                    float needToFull = statsContainerData.Startarmour - armour;
                    if (needToFull <= statsContainerData.heathRestoreCount)
                    {
                        armour = statsContainerData.Startarmour;
                    }
                    else
                    {
                        float perFrame = statsContainerData.heathRestoreCount * Time.deltaTime * statsContainerData.heathRestoreSpeed;
                        armour += perFrame;
                    }
                    OnArmourChanged.Invoke(armour);
                }
            }
        }
        

        public override void SetDefaults()
        {
            currentHealth = statsContainerData.startHealth;
            isDead = statsContainerData.isDead;
            armour = statsContainerData.Startarmour;
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

            lastTakeDamage = timer;
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
            currentHealth = statsContainerData.startHealth;
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
            float percent = (currentHealth / statsContainerData.startHealth) * 100;
            return percent >= 50;
        }
    }
}
