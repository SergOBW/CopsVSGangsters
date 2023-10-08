using System;
using DefaultNamespace;
using UnityEngine;

public abstract class StatsHitReaction : MonoBehaviour
{
    protected StatsController statsController;
    public event Action OnHit;

    public void Initialize(StatsController statsController)
    {
        this.statsController = statsController;
        statsController.OnStatsDead += StatsOnOnStatsDead; 
    }

    private void StatsOnOnStatsDead(StatsController obj)
    {
        Destroy(this);
    }

    public virtual void HitReaction(IDamaging damaging)
    {
        OnHit?.Invoke();
        statsController.GetComponent<EnemySoundManager>().PlayHitSound();
    }
}

public interface IDamaging
{
    float GetDamage();
    void Damage();
}
