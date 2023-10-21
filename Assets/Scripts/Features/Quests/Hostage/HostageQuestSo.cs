using UnityEngine;

namespace Quests.Hostage
{
    [CreateAssetMenu()]
    public class HostageQuestSo : QuestSo
    {
        public int hostagesAmount;

        public GameObject hostagePrefab;
        
        public override Quest CreateQuest()
        {
            return new HostageQuest(this);
        }
    }
}