using UnityEngine;

namespace Quests.Hostage
{
    [CreateAssetMenu(menuName = "Quests/Hostage")]
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