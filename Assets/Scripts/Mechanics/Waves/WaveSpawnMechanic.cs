using System;
using System.Collections.Generic;
using Abstract;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Wave
{
    public int enemySpawnSpeed;
    public int waveSpawnSpeed;
    public int enemyCount;
    public EnemySo[] enemySos;
    
    public int currentEnemyCount;
    public Wave(WaveSettingsSo waveSettingsSo)
    {
        enemySpawnSpeed = waveSettingsSo.enemySpawnSpeed;
        waveSpawnSpeed = waveSettingsSo.waveSpawnSpeed;
        enemyCount = waveSettingsSo.enemyCount;
        enemySos = waveSettingsSo.enemySos;

        currentEnemyCount = enemyCount;
    }
}

[RequireComponent(typeof(EnemySpawnMechanic))]
public class WaveSpawnMechanic : GameModeMechanic
{
    private EnemySpawnMechanic enemySpawnMechanic;

    private List<Wave> _waves = new List<Wave>();

    private float timer;

    private float lastWaveSpawnTime;
    private float lastEnemySpawnTime;
    private int currentWaveCount;
    public event Action<int,int,int> OnWaveStatusChanged;

    private Wave _currentWave;
    private Wave _previousWave;

    public override void Initialize(Scenario scenario)
    {
        base.Initialize(scenario);
        WaveDefenseScenario waveDefenseScenario = scenario as WaveDefenseScenario;
        if (waveDefenseScenario != null)
        {
            Initialize(waveDefenseScenario.waveSettingsSos);
        }
    }

    public void Initialize(WaveSettingsSo[] waveSettingsSos)
    {
        enemySpawnMechanic = GetComponent<EnemySpawnMechanic>();

        _waves = new List<Wave>();
        foreach (var waveSettingsSo in waveSettingsSos)
        {
            _waves.Add(new Wave(waveSettingsSo));
        }

        if (_waves.Count <= 0)
        {
            Debug.LogError("There is no waves added! Go and add them!");
            return;
        }
        
        currentWaveCount = _waves.Count;
        _currentWave = _waves[0];
        InvokeRepeating("ThrowEvent",0.5f,0.5f);
    }
    private void Update()
    {
        if (!LevelMonoStateMachine.Instance.IsPlayState() || _waves.Count <= 0)
        {
            return;
        }
        timer += Time.deltaTime;
        if (_currentWave == null)
        {
            return;
        }
        
        if (timer - lastWaveSpawnTime > _currentWave.waveSpawnSpeed && currentWaveCount > 0)
        {
            if (timer - lastEnemySpawnTime > _currentWave.enemySpawnSpeed && _currentWave.currentEnemyCount > 0)
            {
                enemySpawnMechanic.SpawnEnemy(_currentWave.enemySos[Random.Range(0,_currentWave.enemySos.Length)]);
                _currentWave.currentEnemyCount--;
                lastEnemySpawnTime = timer;
            }

            if (_currentWave.currentEnemyCount <= 0)
            {
                currentWaveCount--;
                lastWaveSpawnTime = timer;
                _previousWave = _currentWave;
                _waves.Remove(_currentWave);
                if (_waves.Count > 0)
                {
                    _currentWave = _waves[0];
                }
            }

        }
    }

    private void ThrowEvent()
    {
        OnWaveStatusChanged?.Invoke(currentWaveCount,_currentWave.currentEnemyCount,_currentWave.enemyCount);
    }

    public override void DeInitialize()
    {

        currentWaveCount = 0;
        _currentWave = null;
        CancelInvoke("ThrowEvent");
        OnWaveStatusChanged?.Invoke(currentWaveCount,_currentWave.currentEnemyCount,_currentWave.enemyCount);
    }
}
