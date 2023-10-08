using UnityEngine;

namespace Quests.KillEnemy
{
    [CreateAssetMenu()]
    public class KillEnemyQuestSo : QuestSo
    {
        public int killAmount;
        
        public override Quest CreateQuest()
        {
            return new KillEnemyQuest(this);
        }
    }
}