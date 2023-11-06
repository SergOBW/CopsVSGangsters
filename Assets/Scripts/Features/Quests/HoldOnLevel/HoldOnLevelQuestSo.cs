using UnityEngine;

namespace Quests.HoldOnLevel
{
    [CreateAssetMenu(menuName = "Quests/Hold on level")]
    public class HoldOnLevelQuestSo : QuestSo
    {
        public float timer;
        public override Quest CreateQuest()
        {
            return new HoldOnLevelQuest(this);
        }
    }
}