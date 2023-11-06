
using UnityEngine;

namespace Quests.LootMoney
{
    [CreateAssetMenu(menuName = "Quests/Loot Money")]
    public class LootMoneyQuestSo : QuestSo
    {
        public int moneyToLoot;
        public override Quest CreateQuest()
        {
            return new LootMoneyQuest(this);
        }
    }
}