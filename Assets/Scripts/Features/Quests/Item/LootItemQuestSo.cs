using UnityEngine;

namespace Quests.Item
{
    [CreateAssetMenu()]
    public class LootItemQuestSo : QuestSo
    {
        public int lootAmount;
        public GameObject lootItemPrefab;
        public override Quest CreateQuest()
        {
            return new LootItemQuest(this);
        }
    }
}