using EnemyCore;
using EnemyCore.States;
using Quests.KillEnemy;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    private EnemyStatsController _enemyStatsController;
    private EnemyAi _enemyAi;
    private EnemyAnimationManager _enemyAnimationManager;
    private EnemyWeaponManager _enemyWeaponManager;
    private EnemySoundManager _enemySoundManager;
    private EnemyMonoStateMachine _enemyMonoStateMachine;
    private KillEnemyItem _killEnemyItem;
    
    [SerializeField] private EnemySo currentEnemySo;

    private VisualsSo _enemyVisualSo;
    public void Initialize(EnemySo enemySo)
    {
        currentEnemySo = enemySo;
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
        _enemyStatsController.Initialize(currentEnemySo.statsSo);
        _enemySoundManager.Initialize(currentEnemySo.enemySoundSo);
        _enemyAi.Initialize(currentEnemySo.aiStats,currentEnemySo.statsSo.speed,currentEnemySo.isEnemy,currentEnemySo.statsSo.maxSpeed);
        _enemyAnimationManager.Initialize(_enemyVisualSo.animatorOverrideController);
        _enemyWeaponManager.Initialize();
        _enemyWeaponManager.EquipWeapon(currentEnemySo.weaponStatsSo);
        _enemyMonoStateMachine.Initialize();
        _killEnemyItem.Initialize(_enemyStatsController);
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
    }
}
