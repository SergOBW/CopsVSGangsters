using System;
using System.Collections.Generic;
using Quests;
using UnityEngine;

[Serializable]
public class EnemySoIntDictionary
{
    public List<EnemySo> enemySos = new List<EnemySo>();
    public List<int> enemyAmount = new List<int>();
}

namespace Level
{
    public class LevelPassScenario : Scenario
    {
        public QuestSo[] questSos;
        public EnemySoIntDictionary enemySoIntDictionary = new EnemySoIntDictionary();

        public LevelPassScenario(ScenarioSo scenarioSo) : base(scenarioSo)
        {
            LevelPassScenarioSo levelPassScenarioSo = scenarioSo as LevelPassScenarioSo;
            if (levelPassScenarioSo == null)
            {
                Debug.LogError("There is error with creating game scenario");
                return;
            }
            questSos = levelPassScenarioSo.questSos;
            enemySoIntDictionary = levelPassScenarioSo.enemySoIntDictionary;
        }
    }
}