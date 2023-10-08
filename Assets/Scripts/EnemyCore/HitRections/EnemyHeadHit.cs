using EnemyCore;

public class EnemyHeadHit : StatsHitReaction
{
    public override void HitReaction(IDamaging damaging)
    {
        base.HitReaction(damaging);
        EnemyHandleMechanic.Instance.TakeDamage(statsController.name,true,damaging);
    }
}
