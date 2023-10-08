﻿using System;
using System.Collections.Generic;
using Abstract;
using EnemyCore.States;
using Level;
using UnityEngine;

namespace EnemyCore
{
    public class EnemyHandleMechanic : GameModeMechanic
    {
        public static EnemyHandleMechanic Instance;
        private int _enemyCount;
        private int _enemyLeastCount;
        private int _headShotHits;
        public int StartedEnemyCount { get; private set; }
        private Dictionary<string,EnemyStatsController> _enemyStatsControllers;
        
        public event Action<bool> OnEnemyHit;

        #region Initialize
        
        private void Awake()
        {
            Instance = this;
        }
        public override void Initialize(Scenario scenario)
        {
            base.Initialize(scenario);
            _enemyStatsControllers = new Dictionary<string, EnemyStatsController>();
            int enemyCount = 0;
            switch (scenario)
            {
                case LevelPassScenario _levelPassScenario:
                    enemyCount = SetLevelPassScenario(_levelPassScenario);
                    break;
                case WaveDefenseScenario _waveDefenseScenario :
                    enemyCount = SetWaveDefenseScenario(_waveDefenseScenario);
                    break;
            }
            
            _enemyCount = enemyCount;
            _enemyLeastCount = _enemyCount;
            StartedEnemyCount = enemyCount;
            _headShotHits = 0;
        }

        private int SetLevelPassScenario(LevelPassScenario _levelPassScenario)
        {
            int enemyCount = 0;
            foreach (var enemyAmount in _levelPassScenario.enemySoIntDictionary.enemyAmount)
            {
                enemyCount += enemyAmount;
            }

            return enemyCount;
        }
        
        private int SetWaveDefenseScenario(WaveDefenseScenario _waveDefenseScenario)
        {
            int enemyCount = 0;
            foreach (var waveSettingsSo in _waveDefenseScenario.waveSettingsSos)
            {
                enemyCount += waveSettingsSo.enemyCount;
            }

            return enemyCount;
        }

        #endregion
        
        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.K))
            {
                List<EnemyStatsController> enemyStatsControllers = new List<EnemyStatsController>();
                foreach (var enemyStatsController in _enemyStatsControllers)
                {
                    enemyStatsControllers.Add(enemyStatsController.Value);
                }

                foreach (var enemyStatsController in enemyStatsControllers)
                {
                    enemyStatsController.TakeDamage(500);
                }
            }
            */
        }

        public void Register(EnemyStatsController enemyStatsController)
        {
            if (!_enemyStatsControllers.ContainsKey(enemyStatsController.name))
            {
                _enemyStatsControllers.Add(enemyStatsController.name, enemyStatsController);
            }
            else
            {
                Debug.LogError("There is enemy with name " + enemyStatsController+ " already exist!");
            }
            
        }

        public void TakeDamage(string colliderName,bool isHead, IDamaging damaging)
        {
            float damage = damaging.GetDamage();
            if (_enemyStatsControllers.TryGetValue(colliderName, out var controller))
            {
                if (isHead)
                {
                    controller.TakeDamage(damage * 1.5f);
                    if (controller.isDead)
                    {
                        _headShotHits++;
                    }
                    OnEnemyHit?.Invoke(true);
                }
                else
                {
                    OnEnemyHit?.Invoke(false);
                    controller.TakeDamage(damage);
                }

                if (SoundMechanic.Instance != null)
                {
                    SoundMechanic.Instance.PlayHit();
                }
            }
            
        }

        public void UnRegister(EnemyStatsController enemyStatsController)
        {
            if (_enemyStatsControllers.ContainsKey(enemyStatsController.name))
            {
                _enemyStatsControllers.Remove(enemyStatsController.name);
                _enemyCount--;
            }
            
            if (_enemyCount <= 0)
            {
                if (currentScenario.GetType() == typeof(LevelPassScenario))
                {
                    return;
                }
                if (LevelsMechanic.Instance != null)
                {
                    LevelsMechanic.Instance.WinLevel();
                }
            }
        }
        
        public override void DeInitialize()
        {
            base.DeInitialize();
            foreach (var kEnemyStatsController in _enemyStatsControllers)
            {
                kEnemyStatsController.Value.gameObject.GetComponent<EnemyMonoStateMachine>().DeInitialize();
            }
            _enemyStatsControllers = new Dictionary<string, EnemyStatsController>();
            _enemyCount = 0;
        }

        public int GetEnemyCount()
        {
            return _enemyCount;
        }

        public float GetCurrentEnemyHealth(int startHealth)
        {
            float health = startHealth * (float)Math.Pow(1.05,LevelsMechanic.Instance.GetLastSavedLevel());
            return health;
        }

        public bool IsHeadBonus()
        {
            float percent = ((float)_headShotHits + 1 / (float)StartedEnemyCount) * 100f;
            return percent >= 50;
        }
    }
}