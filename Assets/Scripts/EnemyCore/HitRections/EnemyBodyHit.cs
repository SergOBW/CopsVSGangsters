using EnemyCore;

public class EnemyBodyHit : StatsHitReaction
{
    public override void HitReaction(IDamaging damaging)
    {
        base.HitReaction(damaging);
        EnemyHandleMechanic.Instance.TakeDamage(statsController.name,false,damaging);
    }
}
