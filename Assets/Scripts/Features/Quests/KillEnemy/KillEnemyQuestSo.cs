using UnityEngine;

namespace Quests.KillEnemy
{
    [CreateAssetMenu(menuName = "Quests/Kill Enemy")]
    public class KillEnemyQuestSo : QuestSo
    {
        public int killAmount;
        
        public override Quest CreateQuest()
        {
            return new KillEnemyQuest(this);
        }
    }
}