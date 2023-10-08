using System;
using EnemyCore;
using UnityEngine;

public enum EnemyBehaviour
{
    Default = 0,
    StandAndShoot = 1
}

[Serializable]
public class AiStats
{
    public float radius = 15;
    [Range(0, 360)] public float angle = 110;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
}
[CreateAssetMenu(menuName = "Enemy")]
public class EnemySo : ScriptableObject
{
    public EnemySoundSo enemySoundSo;
    
    public StatsSo statsSo;

    public VisualsSo[] visuals;

    public WeaponStatsSo weaponStatsSo;

    public AiStats aiStats;

    public bool isEnemy;

    public Transform enemyPrefab;

    public bool isBoss;

    public EnemyBehaviour[] enemyBehaviour = new[] { EnemyBehaviour.Default };
}
