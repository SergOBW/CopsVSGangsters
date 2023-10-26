using DamageNumbersPro;
using DefaultNamespace;
using EnemyCore;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyVisualsController : MonoBehaviour
{
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    private CharacterJoint[] _characterJoints;
    [SerializeField] private GameObject[] enemyVisuals;
    private SkinnedMeshRenderer _currentEnemyRenderer;
    [SerializeField] private Material hitMaterial;
    private float timer;
    [SerializeField] private DamageNumber _damageNumberPrefab;

    [SerializeField] private Transform meleeSlot;
    [SerializeField] private Transform weaponSlot;
    protected int index;
    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        ChooseRandomVisual();
    }

    private void Update()
    {
        if (GlobalSettings.Instance.graphicsQuality == GraphicsQuality.Low)
        {
            return;
        }
        timer += Time.deltaTime * 3;
        if (timer <= 1f)
        {
            Color color = _currentEnemyRenderer.materials[0].color;
            color = new Color(color.r, color.g, color.b, timer);
            _currentEnemyRenderer.materials[0].color = color;
        }
        else
        {
            Color color = _currentEnemyRenderer.materials[0].color;
            color = new Color(color.r, color.g, color.b, 0);
            _currentEnemyRenderer.materials[0].color = color;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    protected virtual void ChooseRandomVisual()
    {
        foreach (var goVisual in enemyVisuals)
        {
            goVisual.SetActive(false);   
        }

        index = Random.Range(0, enemyVisuals.Length);
        GameObject enemyVisual = enemyVisuals[index];
        timer = 1;
        enemyVisual.SetActive(true);
        _currentEnemyRenderer = enemyVisual.GetComponent<SkinnedMeshRenderer>();
        _currentEnemyRenderer.materials = new[] { new Material(hitMaterial), _currentEnemyRenderer.materials[0] };
    }

    public void Hit(float damage)
    {
        timer = 0;
        _damageNumberPrefab.Spawn(transform.position + Vector3.up,damage);
    }
    
    public void InitializeHits(EnemyStatsController enemyStatsController)
    {
        StatsHitReaction[] hits = GetComponentsInChildren<StatsHitReaction>();
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                hit.Initialize(enemyStatsController);
            }
        }
    }

    public Transform GetMeleeSlot()
    {
        return meleeSlot;
    }
    
    public Transform GetWeaponSlot()
    {
        return weaponSlot;
    }
}
