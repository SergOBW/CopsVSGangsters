using Abstract;
using Player;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] private GlobalMonoMechanic[] monoMechanics;
    [SerializeField] private EnemySpawnMechanic _enemySpawnMechanic;

    private void Start()
    {
        FindObjectOfType<PlayerStatsController>().Initialize();
        foreach (var monoMechanic in monoMechanics)
        {
            monoMechanic.Initialize();
        }
        _enemySpawnMechanic.SpawnAtAllSpawnPoints();
    }
}
