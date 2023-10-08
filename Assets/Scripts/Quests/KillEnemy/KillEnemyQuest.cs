using UnityEngine;

namespace Quests.KillEnemy
{
    public class KillEnemyQuest : Quest
    {
        public int CurrentKillAmount => _currentKillAmount;
        public int KillAmount => _killAmount;
        
        private int _killAmount;
        private int _currentKillAmount;
        public KillEnemyQuest(QuestSo questSo) : base(questSo)
        {
            KillEnemyQuestSo killEnemyQuestSo = questSo as KillEnemyQuestSo;

            if (killEnemyQuestSo == null)
            {
                Debug.LogError("Some error with creating kill enemy quest!");
                return;
            }

            _killAmount = killEnemyQuestSo.killAmount;
            _currentKillAmount = 0;
        }


        public override bool IsQuestItemValid(QuestItem questItem)
        {
            if (questItem.GetType() == typeof(KillEnemyItem))
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

            if (_currentKillAmount >= _killAmount)
            {
                Debug.LogError("There is max of kills");
                return;
            }

            if (_currentKillAmount + 1 >= _killAmount)
            {
                _currentKillAmount += 1;
                FireOnQuestUpdatedEvent(this);
                FireOnQuestCompletedEvent(this);
            }
            else
            {
                _currentKillAmount += 1;
                FireOnQuestUpdatedEvent(this);
            }
        }
    }
}