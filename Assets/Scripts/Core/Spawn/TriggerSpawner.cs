using DefaultNamespace;
using EnemyCore;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    private bool _isUsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerDetector playerDetector))
        {
            if (!_isUsed)
            {
                Debug.Log(other.name);
                EnemyHandleMechanic.Instance.OnPlayerTriggered(_enemySpawnPoints);
                _isUsed = true;
            }
        }
    }
}
