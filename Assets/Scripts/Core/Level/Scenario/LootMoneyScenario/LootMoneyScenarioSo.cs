using UnityEngine;

namespace Level.LootMoneyScenario
{
    [CreateAssetMenu(menuName = "Level/Scenario/Loot Money")]
    public class LootMoneyScenarioSo : QuestScenarioSo
    {
        public float startingMoneyOnLevel = 10000;
        public float startingBonusMoney = 2000;

        public override Scenario CreateGameScenario()
        {
            return new LootMoneyScenario(this);
        }
    }
}