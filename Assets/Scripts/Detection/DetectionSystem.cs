using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask enemyLayer;
    
    private List<Transform> detectedEnemies = new List<Transform>();

    private Transform exception;

    public delegate void EnemyDetectedEventHandler(Transform enemy);
    public event EnemyDetectedEventHandler OnEnemyDetected;

    public delegate void EnemyLostEventHandler(Transform enemy);
    public event EnemyLostEventHandler OnEnemyLost;
    
    public delegate void EnemyListEventHandler(List<Transform> enemy);
    public event EnemyListEventHandler OnEnemyListChanged;

    public void Initialize(float detectionRadius, LayerMask layerMask)
    {
        this.detectionRadius = detectionRadius;
        enemyLayer = layerMask;
    }
    
    public void Initialize(float detectionRadius, LayerMask layerMask, Transform exception)
    {
        this.detectionRadius = detectionRadius;
        enemyLayer = layerMask;
        this.exception = exception;
    }

    private void FixedUpdate()
    {
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        HashSet<Transform> currentEnemies = new HashSet<Transform>(colliders.Length);
        foreach (Collider collider in colliders)
        {
            Transform enemy = collider.transform;
            if (enemy != exception && enemy != transform)
            {
                currentEnemies.Add(enemy);
            }
        }

        HashSet<Transform> enemiesToRemove = new HashSet<Transform>(detectedEnemies);
        enemiesToRemove.ExceptWith(currentEnemies);

        foreach (Transform enemy in currentEnemies)
        {
            if (!detectedEnemies.Contains(enemy))
            {
                detectedEnemies.Add(enemy);
                OnEnemyDetected?.Invoke(enemy);

                StatsController statsController = enemy.GetComponent<StatsController>();
                if (statsController != null)
                {
                    statsController.OnStatsDead += RemoveDeadEnemy;
                }
            }
        }

        foreach (Transform enemy in enemiesToRemove)
        {
            detectedEnemies.Remove(enemy);
            OnEnemyLost?.Invoke(enemy);

            StatsController statsController = enemy.GetComponent<StatsController>();
            if (statsController != null)
            {
                statsController.OnStatsDead -= RemoveDeadEnemy;
            }
        }

        OnEnemyListChanged?.Invoke(detectedEnemies);
    }

    private void RemoveDeadEnemy(StatsController statsController)
    {
        Transform deadEnemy = statsController.transform;
        detectedEnemies.Remove(deadEnemy);
        statsController.OnStatsDead -= RemoveDeadEnemy;
        OnEnemyLost?.Invoke(deadEnemy);
        OnEnemyListChanged?.Invoke(detectedEnemies);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public List<Transform> GetEnemyList()
    {
        return detectedEnemies;
    }
}