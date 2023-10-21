using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    public SpawnPoint[] GetSpawnPoints()
    {
        return _enemySpawnPoints;
    }
}
