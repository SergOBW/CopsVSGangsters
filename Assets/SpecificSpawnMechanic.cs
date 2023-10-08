using System.Collections.Generic;
using Abstract;
using DefaultNamespace;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemySpawnMechanic))]
public class SpecificSpawnMechanic : GameModeMechanic
{
    private LevelPassScenario _levelPassScenario;
    private List<EnemySpawnPoint> _spawnPoints;
    private int _enemyToSpawnCount;
    public override void Initialize(Scenario scenario)
    {
        base.Initialize(scenario);
        _levelPassScenario = currentScenario as LevelPassScenario;
        if (_levelPassScenario == null)
        {
            Debug.LogError("Specific Spawn Mechanic works only with level pass scenario");
            enabled = false;
        }

        SetupSpawnPoints();
        SpawnAll();
    }

    private void SpawnAll()
    {
        _enemyToSpawnCount = 0;
        foreach (var enemyAmount in _levelPassScenario.enemySoIntDictionary.enemyAmount)
        {
            _enemyToSpawnCount += enemyAmount;
        }

        if (_spawnPoints.Count < _enemyToSpawnCount)
        {
            Debug.LogError("There no enough spawn points");
            _enemyToSpawnCount = _spawnPoints.Count;
        }
        for (int i = 0; i < _levelPassScenario.enemySoIntDictionary.enemySos.Count; i++)
        {
            for (int j = 0; j < _levelPassScenario.enemySoIntDictionary.enemyAmount[i]; j++)
            {
                EnemySo enemySo = _levelPassScenario.enemySoIntDictionary.enemySos[i];
                EnemySpawnPoint enemySpawnPoint = GetSpawnPoint(enemySo);
                if (enemySpawnPoint == null)
                {
                    Debug.LogError("Some troubles with spawning");
                    continue;
                }
                EnemySpawn(enemySo,enemySpawnPoint);
            }
        }
    }

    private EnemySpawnPoint GetSpawnPoint(EnemySo enemySo)
    {
        if (_spawnPoints.Count <= 0)
        {
            return null;
        }

        List<EnemySpawnPoint> emptySpawnPoints = new List<EnemySpawnPoint>();
        foreach (var enemySpawnPoint in _spawnPoints)
        {
            if (enemySpawnPoint.CanSpawnEnemyHere(enemySo))
            {
                emptySpawnPoints.Add(enemySpawnPoint);
            }
        }

        if (emptySpawnPoints.Count <= 0)
        {
            return null;
        }

        return emptySpawnPoints[Random.Range(0, emptySpawnPoints.Count)];
    }
    
    private void SetupSpawnPoints()
    {
        _spawnPoints = new List<EnemySpawnPoint>();
        EnemySpawnPoint[] spawnPointArr = FindObjectsOfType<EnemySpawnPoint>();
        foreach (var spawnPoint in spawnPointArr)
        {
            _spawnPoints.Add(spawnPoint);
        }
        
    }
    
    private void EnemySpawn(EnemySo enemySo, EnemySpawnPoint enemySpawnPoint)
    {
        // Move player to the spawn Point
        
        NavMesh.SamplePosition(enemySpawnPoint.gameObject.transform.position, out NavMeshHit navMeshHit, 5, NavMesh.AllAreas);
        var myRandomPositionInsideNavMesh = navMeshHit.position;

        if (myRandomPositionInsideNavMesh == Vector3.positiveInfinity || myRandomPositionInsideNavMesh == Vector3.negativeInfinity)
        {
            Debug.LogError("The enemy spawn point is infinity ! Check is navmesh surface is on map");
            return;
        }

        Transform transfrom = Instantiate(enemySo.enemyPrefab, myRandomPositionInsideNavMesh, quaternion.identity);

        if (transfrom== null)
        {
            Debug.LogError("There is problem with spawning enemy");
            return;
        }

        EnemySetup enemySetup = transfrom.GetComponent<EnemySetup>();

        if (enemySetup != null)
        {
            enemySpawnPoint.BindSpawnPoint();
            enemySetup.Initialize(enemySo);
        }
    }
}
