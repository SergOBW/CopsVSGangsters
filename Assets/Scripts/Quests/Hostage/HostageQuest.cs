using UnityEngine;

namespace Quests.Hostage
{
    public class HostageQuest : Quest
    {
        public int HostagesAmount => _hostagesAmount;
        public int CurrentHostageAmount => _currentHostageAmount;
        
        private int _hostagesAmount;
        private int _currentHostageAmount;

        public GameObject hostagePrefab;
        public HostageQuest(QuestSo questSo) : base(questSo)
        {
            HostageQuestSo hostageQuestSo = questSo as HostageQuestSo;

            if (hostageQuestSo == null)
            {
                Debug.LogError("Some error with creating kill enemy quest!");
                return;
            }

            _hostagesAmount = hostageQuestSo.hostagesAmount;
            hostagePrefab = hostageQuestSo.hostagePrefab;
            _currentHostageAmount = 0;
        }
        
        public override bool IsQuestItemValid(QuestItem questItem)
        {
            if (questItem.GetType() == typeof(HostageItem))
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

            if (_currentHostageAmount >= _hostagesAmount)
            {
                Debug.LogError("There is max of hostages");
                return;
            }

            if (_currentHostageAmount + 1 >= _hostagesAmount)
            {
                _currentHostageAmount += 1;
                FireOnQuestUpdatedEvent(this);
                FireOnQuestCompletedEvent(this);
            }
            else
            {
                _currentHostageAmount += 1;
                FireOnQuestUpdatedEvent(this);
            }
        }
    }
}