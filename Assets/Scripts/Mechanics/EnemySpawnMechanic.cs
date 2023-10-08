using System.Collections.Generic;
using Abstract;
using DefaultNamespace;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawnMechanic : GameModeMechanic
{
    private List<EnemySpawnPoint> spawnPoints;
    [SerializeField] private EnemySo defaultEnemySo;


    public override void Initialize(Scenario gameScenario)
    {
        base.Initialize(gameScenario);
        SetupSpawnPoints();
    }

    public void SpawnEnemy(EnemySo enemySo = null)
    {
        if (enemySo == null)
        {
            //Debug.LogError("You trying to spawn an null enemy ! Check scripts! Spawning default");
            enemySo = defaultEnemySo;
        }
        DefaultSpawnPointSpawn(enemySo);
    }

    public void DefaultSpawnPointSpawn(EnemySo enemySo = null)
    {
        if (enemySo == null)
        {
            enemySo = defaultEnemySo;
        }
        if (spawnPoints.Count <= 0)
        {
            return;
        }

        List<EnemySpawnPoint> emptySpawnPoints = new List<EnemySpawnPoint>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.CanSpawnEnemyHere(enemySo))
            {
                emptySpawnPoints.Add(spawnPoint);
            }
        }

        if (emptySpawnPoints.Count <= 0)
        {
            return;
        }

        EnemySpawnPoint randomSpawnPoint = emptySpawnPoints[Random.Range(0, emptySpawnPoints.Count)];
        
        EnemySpawn(enemySo,randomSpawnPoint);
    }

    public void SpawnAtAllSpawnPoints(EnemySo enemySo = null)
    {
        if (enemySo == null)
        {
            enemySo = defaultEnemySo;
        }
        if (spawnPoints.Count > 0)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                EnemySpawn(enemySo,spawnPoints[i]);
            }
        }
    }
    

    private void EnemySpawn(EnemySo enemySo, EnemySpawnPoint positionToSpawn)
    {
        // Move player to the spawn Point
        
        NavMesh.SamplePosition(positionToSpawn.transform.position, out NavMeshHit navMeshHit, Mathf.Infinity, NavMesh.AllAreas);
        var myRandomPositionInsideNavMesh = navMeshHit.position;

        Transform transfrom = Instantiate(enemySo.enemyPrefab, myRandomPositionInsideNavMesh, quaternion.identity);

        EnemySetup enemySetup = transfrom.GetComponent<EnemySetup>();
        
        enemySetup.Initialize(enemySo);
        
    }

    private void SetupSpawnPoints()
    {
        spawnPoints = new List<EnemySpawnPoint>();
        EnemySpawnPoint[] spawnPointArr = FindObjectsOfType<EnemySpawnPoint>();
        foreach (var spawnPoint in spawnPointArr)
        {
            spawnPoints.Add(spawnPoint);
        }
        
    }

    public override void DeInitialize()
    {
        spawnPoints = new List<EnemySpawnPoint>();
    }
}
