using UnityEngine;

namespace Quests.Item
{
    public class LootItemQuest : Quest
    {

        public int CurrentLootAmount => _currentLootAmount;
        public int LootAmount => _lootAmount;
        
        private int _currentLootAmount;
        private int _lootAmount;

        public GameObject lootItemPrefab;
        public LootItemQuest(QuestSo questSo) : base(questSo)
        {
            LootItemQuestSo lootItemQuestSo = questSo as LootItemQuestSo;
            if (lootItemQuestSo == null)
            {
                Debug.LogError("Error, some problem with creating item quest");
                return;
            }

            _lootAmount = lootItemQuestSo.lootAmount;
            lootItemPrefab = lootItemQuestSo.lootItemPrefab;
            _currentLootAmount = 0;
        }
        
        public override bool IsQuestItemValid(QuestItem questItem)
        {
            if (questItem.GetType() == typeof(LootItem))
            {
                return true;
            }

            return false;
        }

        public override void TryToProgressQuest(QuestItem questItem)
        {
            if (!IsQuestItemValid(questItem))
            {
                return;
            }
            
            if (_currentLootAmount >= _lootAmount)
            {
                Debug.LogError("There is max of kills");
                return;
            }

            if (_currentLootAmount + 1 >= _lootAmount)
            {
                _currentLootAmount += 1;
                FireOnQuestUpdatedEvent(this);
                FireOnQuestCompletedEvent(this);
            }
            else
            {
                _currentLootAmount += 1;
                FireOnQuestUpdatedEvent(this);
            }
        }
    }
}