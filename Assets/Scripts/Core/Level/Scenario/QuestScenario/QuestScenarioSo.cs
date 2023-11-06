using Quests;
using UnityEngine;


namespace Level
{
    [CreateAssetMenu(menuName = "Level/Scenario/Quest")]
    public class QuestScenarioSo : ScenarioSo
    {
        public QuestSo[] questSos;

        public EnemySoIntDictionary enemySoIntDictionary;
        public VisualsSo[] visualsSo;

        public override Scenario CreateGameScenario()
        {
            return new QuestScenario(this);
        }
    }
}