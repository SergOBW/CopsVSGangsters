using System;

namespace Quests
{
    public class Quest
    {
        // Quest types
        // Collecting something
        // Kill count
        // Stay countdown
        
        public string ruTitle { get; private set; }
        public string enTitle{ get; private set; }
        public string trTitle{ get; private set; }
        
        public string ruGoal{ get; private set; }
        public string enGoal{ get; private set; }
        public string trGoal{ get; private set; }
        
        public event Action<Quest> OnQuestCompleted;
        public event Action<Quest> OnQuestUpdated;

        public Quest(QuestSo questSo)
        {
            ruTitle = questSo.ruTitle;
            enTitle = questSo.enTitle;
            trTitle = questSo.trTitle;

            ruGoal = questSo.ruGoal;
            enGoal = questSo.enGoal;
            trGoal = questSo.trGoal;
        }

        public virtual void Initialize(QuestsMechanic questsMechanic)
        {
            
        }

        public virtual void TryToProgressQuest(QuestItem questItem)
        {
            
        }

        public virtual bool IsQuestItemValid(QuestItem questItem)
        {
            return false;
        }

        protected void FireOnQuestCompletedEvent(Quest quest)
        {
            OnQuestCompleted?.Invoke(quest);
        }
        
        protected void FireOnQuestUpdatedEvent(Quest quest)
        {
            OnQuestUpdated?.Invoke(quest);
        }
    }
}