using EnemyCore;

namespace Quests.KillEnemy
{
    public class KillEnemyItem : QuestItem
    {
        public void Initialize(EnemyStatsController enemyStatsController)
        {
            enemyStatsController.OnEnemyDie += OnOnEnemyDie;
        }

        private void OnOnEnemyDie(EnemyStatsController obj)
        {
            QuestsMechanic.Instance.TryToProgressQuest(this);
        }
    }
}