using EnemyCore;

public class EnemyHeadHit : StatsHitReaction
{
    public override void Damage()
    {
        base.Damage();
        EnemyHandleMechanic.Instance.TakeDamage(statsController.name,true);
    }
}
