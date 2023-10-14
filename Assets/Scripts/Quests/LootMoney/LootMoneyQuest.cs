using Quests.Item;
using UnityEngine;

namespace Quests.LootMoney
{
    public class LootMoneyQuest : Quest
    {
        public float CurrentMoneyAmount
        {
            get => _currentMoneyCount;
        }
        
        public float MoneyToLoot
        {
            get => _moneyToLoot;
        }
        
        private int _moneyToLoot;
        private int _currentMoneyCount;
        public LootMoneyQuest(QuestSo questSo) : base(questSo)
        {
            LootMoneyQuestSo lootItemQuestSo = questSo as LootMoneyQuestSo;
            if (lootItemQuestSo == null)
            {
                Debug.LogError("Error, some problem with creating item quest");
                return;
            }

            _moneyToLoot = lootItemQuestSo.moneyToLoot;
            _currentMoneyCount = 0;
        }

        public override bool IsQuestItemValid(QuestItem questItem)
        {
            if (questItem.GetType() == typeof(LootMoneyItem))
            {
                return true;
            }

            return false;
        }

        public override void TryToProgressQuest(QuestItem questItem)
        {
            base.TryToProgressQuest(questItem);
            LootMoneyItem lootMoneyItem = questItem as LootMoneyItem;
            if (_currentMoneyCount >= _moneyToLoot)
            {
                Debug.LogError("There is max of money");
                return;
            }

            if (_currentMoneyCount + lootMoneyItem.GetMoneyAmount() >= _moneyToLoot)
            {
                _currentMoneyCount = _moneyToLoot;
                FireOnQuestUpdatedEvent(this);
                FireOnQuestCompletedEvent(this);
            }
            else
            {
                _currentMoneyCount += lootMoneyItem.GetMoneyAmount();
                FireOnQuestUpdatedEvent(this);
            }
        }
    }
}