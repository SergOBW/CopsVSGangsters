using UnityEngine;

namespace Quests.HoldOnLevel
{
    [CreateAssetMenu()]
    public class HoldOnLevelQuestSo : QuestSo
    {
        public float timer;
        public override Quest CreateQuest()
        {
            return new HoldOnLevelQuest(this);
        }
    }
}