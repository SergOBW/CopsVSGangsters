using System;
using DefaultNamespace;
using EnemyCore;
using EnemyCore.States;
using Quests.KillEnemy;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDefaultSetup : MonoBehaviour, ISpawnable
{
    private EnemyStatsController _enemyStatsController;
    private EnemyAi _enemyAi;
    private EnemyAnimationManager _enemyAnimationManager;
    private EnemyWeaponManager _enemyWeaponManager;
    private EnemySoundManager _enemySoundManager;
    private EnemyMonoStateMachine _enemyMonoStateMachine;
    private KillEnemyItem _killEnemyItem;
    private CapsuleCollider _capsuleCollider;
    
    private EnemySo currentEnemySo;

    private VisualsSo _enemyVisualSo;

    protected bool isWithEndPoint;
    
    public virtual void Initialize()
    {
        currentEnemySo = EnemyHandleMechanic.Instance.GetEnemySo();
        CreateVisuals();
        CreateScripts();
        SetDefaults();
    }

    private void CreateVisuals()
    {
        _enemyVisualSo = currentEnemySo.visuals[Random.Range(0,currentEnemySo.visuals.Length)];
        Transform visualsGo = Instantiate(_enemyVisualSo.VisualGameObject, transform);
        visualsGo.localPosition = _enemyVisualSo.Position;
        visualsGo.localRotation = Quaternion.Euler(_enemyVisualSo.Rotation);
        visualsGo.localScale = _enemyVisualSo.Scale;
    }
    

    private void SetDefaults()
    {
        GameObject detector = Instantiate(new GameObject(), transform);
        detector.tag = "EnemyDetector";
        detector.layer = LayerMask.NameToLayer("Interactable");
        detector.transform.localPosition = new Vector3(0, 0.1f, 0);
        
        _enemyStatsController.Initialize(currentEnemySo.stats);
        _enemySoundManager.Initialize(currentEnemySo.enemySoundSo);
        _enemyAi.Initialize(currentEnemySo.aiStats,currentEnemySo.stats.startSpeed,isWithEndPoint,currentEnemySo.stats.maxSpeed);
        _enemyAnimationManager.Initialize(_enemyVisualSo.animatorOverrideController);
        _enemyWeaponManager.Initialize();
        _enemyWeaponManager.EquipWeapon(currentEnemySo.weaponStatsSo);
        _enemyMonoStateMachine.Initialize();
        _killEnemyItem.Initialize(_enemyStatsController);
        /*
        _capsuleCollider.center = new Vector3(0, 0.7f, 0);
        _capsuleCollider.radius = 0.2f;
        _capsuleCollider.height = 1.5f;
        */
    }
    

    private void CreateScripts()
    {
        _enemyStatsController = gameObject.AddComponent<EnemyStatsController>();
        _enemyAi =  gameObject.AddComponent<EnemyAi>();
        _enemyMonoStateMachine = gameObject.AddComponent<EnemyMonoStateMachine>();
        _enemyAnimationManager = gameObject.AddComponent<EnemyAnimationManager>();
        _enemyWeaponManager = gameObject.AddComponent<EnemyWeaponManager>();
        _enemySoundManager = gameObject.AddComponent<EnemySoundManager>();
        _killEnemyItem = gameObject.AddComponent<KillEnemyItem>();
            //_capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
    }

    public Type GetSpawnPointType()
    {
        return typeof(EnemySpawnPoint);
    }

    public GameObject GetObjectToSpawn()
    {
        return gameObject;
    }

    public bool IsSpawnOnNavMesh()
    {
        return true;
    }
}
