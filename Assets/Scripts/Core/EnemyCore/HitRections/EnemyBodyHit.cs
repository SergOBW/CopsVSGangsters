using EnemyCore;

public class EnemyBodyHit : StatsHitReaction
{
    public override void Damage()
    {
        base.Damage();
        EnemyHandleMechanic.Instance.TakeDamage(statsController.name,false);
    }
}
