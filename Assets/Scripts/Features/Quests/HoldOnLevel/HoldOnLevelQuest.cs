using UnityEngine;

namespace Quests.HoldOnLevel
{
    public class HoldOnLevelQuest : Quest
    {
        private readonly float _startingTimer;
        private float _currentTimer;
        private bool _isQuestCompleted;
        public HoldOnLevelQuest(QuestSo questSo) : base(questSo)
        {
            HoldOnLevelQuestSo holdOnLevelQuestSo = questSo as HoldOnLevelQuestSo;
            if (holdOnLevelQuestSo != null)
            {
                _startingTimer = holdOnLevelQuestSo.timer;
                _currentTimer = 0;
                _isQuestCompleted = false;
            }
        }


        public override void Update()
        {
            base.Update();
            if (_currentTimer < _startingTimer)
            {
                FireOnQuestUpdatedEvent(this);
                _currentTimer += Time.deltaTime;
            }

            if (_currentTimer >= _startingTimer && !_isQuestCompleted)
            {
                _isQuestCompleted = true;
                FireOnQuestCompletedEvent(this);
            }
        }

        public float GetCurrentTime()
        {
            return _currentTimer;
        }

        public float GetStartedTime()
        {
            return _startingTimer;
        }
    }
}