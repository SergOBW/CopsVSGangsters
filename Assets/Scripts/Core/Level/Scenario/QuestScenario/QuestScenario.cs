using System;
using System.Collections.Generic;
using Quests;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySoIntDictionary
{
    public List<EnemySo> enemySos = new List<EnemySo>();
    public List<int> enemyAmount = new List<int>();
}

namespace Level
{
    public class QuestScenario : Scenario
    {
        public QuestSo[] questSos;
        public VisualsSo[] visualsSo;
        public EnemySoIntDictionary enemySoIntDictionary = new EnemySoIntDictionary();

        public QuestScenario(ScenarioSo scenarioSo) : base(scenarioSo)
        {
            QuestScenarioSo questScenarioSo = scenarioSo as QuestScenarioSo;
            if (questScenarioSo == null)
            {
                Debug.LogError("There is error with creating game scenario");
                return;
            }
            questSos = questScenarioSo.questSos;
            enemySoIntDictionary = questScenarioSo.enemySoIntDictionary;
            visualsSo = questScenarioSo.visualsSo;
        }

        public EnemySo GetRandomEnemy()
        {
            return enemySoIntDictionary.enemySos[Random.Range(0, enemySoIntDictionary.enemySos.Count)];
        }

        public VisualsSo GetRandomVisuals()
        {
            return visualsSo.Length > 0 ? visualsSo[Random.Range(0, visualsSo.Length)] : null;
        }
    }
}