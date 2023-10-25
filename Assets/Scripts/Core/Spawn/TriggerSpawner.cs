using DefaultNamespace;
using EnemyCore;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    [SerializeField] private TriggerSpawner[] _triggerSpawners;
    private bool _isUsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerDetector playerDetector))
        {
            if (!_isUsed)
            {
                Debug.Log(other.name);
                EnemyHandleMechanic.Instance.OnPlayerTriggered(_enemySpawnPoints);
                if (_triggerSpawners.Length > 0)
                {
                    foreach (var trigger in _triggerSpawners)
                    {
                        trigger._isUsed = true;
                        if (trigger.TryGetComponent(out Collider collider))
                        {
                            collider.enabled = false;
                        }
                    }
                }
                _isUsed = true;
                if (TryGetComponent(out Collider myCollider))
                {
                    myCollider.enabled = false;
                }
            }
        }
    }
}
