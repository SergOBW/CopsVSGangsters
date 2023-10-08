using Quests;
using UnityEngine;


namespace Level
{
    [CreateAssetMenu()]
    public class LevelPassScenarioSo : ScenarioSo
    {
        public QuestSo[] questSos;

        public EnemySoIntDictionary enemySoIntDictionary;

        public override Scenario CreateGameScenario()
        {
            return new LevelPassScenario(this);
        }
    }
}