
using UnityEngine;

namespace Quests.LootMoney
{
    [CreateAssetMenu()]
    public class LootMoneyQuestSo : QuestSo
    {
        public int moneyToLoot;
        public override Quest CreateQuest()
        {
            return new LootMoneyQuest(this);
        }
    }
}