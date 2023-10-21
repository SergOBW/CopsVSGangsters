using DefaultNamespace;
using UnityEngine;

public class StartSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    public SpawnPoint[] GetSpawnPoints()
    {
        return _enemySpawnPoints;
    }
}
