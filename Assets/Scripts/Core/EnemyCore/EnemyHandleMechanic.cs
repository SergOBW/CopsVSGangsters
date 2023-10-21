using System;
using System.Collections.Generic;
using Abstract;
using EnemyCore.States;
using Level;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace EnemyCore
{
    public class EnemyHandleMechanic : GameModeMechanic
    {
        [SerializeField] private EnemySo[] enemySos;
        
        public static EnemyHandleMechanic Instance;
        private int _enemyCount;
        private int _enemyLeastCount;
        private int _headShotHits;
        public int StartedEnemyCount { get; private set; }
        private Dictionary<string,EnemyStatsController> _enemyStatsControllers;
        
        public event Action<bool> OnEnemyHit;
        private SpawnMonoMechanic _spawnMonoMechanic;

        #region Initialize
        
        public override void Initialize(Scenario scenario)
        {
            base.Initialize(scenario);
            Instance = this;
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

            _spawnMonoMechanic = GetComponent<SpawnMonoMechanic>();
            if ( FindObjectOfType<StartSpawner>() != null)
            {
                _spawnMonoMechanic.SpawnAtSpawnPoints(new GameObject().AddComponent<EnemyDefaultSetup>(),FindObjectOfType<StartSpawner>().GetSpawnPoints());
            }
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
            foreach (var waveSettingsSo in _waveDefenseScenario.waveSetting)
            {
                enemyCount += waveSettingsSo.enemyCount;
            }

            return enemyCount;
        }

        #endregion
        
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

        public void TakeDamage(string colliderName,bool isHead)
        {
            float damage = WeaponManagerMechanic.Instance.GetCurrentWeapon().Damage;
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

                if (SoundMonoMechanic.Instance != null)
                {
                    SoundMonoMechanic.Instance.PlayHit();
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
                if (LevelsMonoMechanic.Instance != null)
                {
                    LevelsMonoMechanic.Instance.WinLevel();
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
        
        public bool IsHeadBonus()
        {
            float percent = ((float)_headShotHits + 1 / (float)StartedEnemyCount) * 100f;
            return percent >= 50;
        }

        public EnemySo GetEnemySo()
        {
            return enemySos[Random.Range(0, enemySos.Length)];
        }

        public void OnPlayerTriggered(SpawnPoint[] spawnPoint)
        {
            _spawnMonoMechanic.SpawnAtSpawnPoints(new GameObject().AddComponent<EnemyDefaultSetup>(),spawnPoint);
        }

        public void SpawnEnemyWave()
        {
            WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
            if (waveSpawner != null)
            {
                _spawnMonoMechanic.SpawnAtSpawnPoints(new GameObject().AddComponent<WithEndpointSetup>(),waveSpawner.GetSpawnPoints());
            }
            else
            {
                Debug.LogError("There is no waveSpawner on scene!");
            }
        }
    }

    public class WithEndpointSetup : EnemyDefaultSetup
    {
        public override void Initialize()
        {
            isWithEndPoint = true;
            base.Initialize();
        }
    }
}