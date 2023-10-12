using System;
using DefaultNamespace;
using UnityEngine;

public abstract class StatsHitReaction : MonoBehaviour, IDamageble
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

    public virtual void Damage()
    {
        OnHit?.Invoke();
        statsController.GetComponent<EnemySoundManager>().PlayHitSound();
    }
}

public interface IDamageble
{
    void Damage();
}
