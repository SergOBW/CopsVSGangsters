using System;
using UnityEngine;

namespace Level
{
    public class WaveDefenseScenario : Scenario
    {
        public WaveSettings[] waveSetting;
        public WaveDefenseScenario(ScenarioSo scenarioSo) : base(scenarioSo)
        {
            WaveDefenseScenarioSo waveDefenseScenarioSo = scenarioSo as WaveDefenseScenarioSo;
            if (waveDefenseScenarioSo == null)
            {
                Debug.LogError("There error with creating wave scenario!");
                return;
            }

            waveSetting = waveDefenseScenarioSo.waveSettings;
        }
    }
}