using Quests;

namespace Level.LootMoneyScenario
{
    public class LootMoneyScenario : QuestScenario
    {
        public QuestSo[] questSos;

        public EnemySoIntDictionary enemySoIntDictionary;
        
        public float startingMoneyOnLevel = 10000;
        public float startingBonusMoney = 2000;
        
        public LootMoneyScenario(ScenarioSo scenarioSo) : base(scenarioSo)
        {
            if (scenarioSo is not LootMoneyScenarioSo)
            {
                return;
            }
            LootMoneyScenarioSo lootMoneyScenarioSo = scenarioSo as LootMoneyScenarioSo;
            questSos = lootMoneyScenarioSo.questSos;
            enemySoIntDictionary = lootMoneyScenarioSo.enemySoIntDictionary;
            startingBonusMoney = lootMoneyScenarioSo.startingBonusMoney;
            startingMoneyOnLevel = lootMoneyScenarioSo.startingMoneyOnLevel;
        }
    }
}