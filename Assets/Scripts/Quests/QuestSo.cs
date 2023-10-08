using UnityEngine;

namespace Quests
{
    public class QuestSo : ScriptableObject
    {
        public string ruTitle;
        public string enTitle;
        public string trTitle;
        
        public string ruGoal;
        public string enGoal;
        public string trGoal;
        
        public virtual Quest CreateQuest()
        {
            return new Quest(this);
        }
    }
}