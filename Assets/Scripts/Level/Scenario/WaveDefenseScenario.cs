using UnityEngine;

namespace Level
{
    public class WaveDefenseScenario : Scenario
    {
        public WaveSettingsSo[] waveSettingsSos;
        public WaveDefenseScenario(ScenarioSo scenarioSo) : base(scenarioSo)
        {
            WaveDefenseScenarioSo waveDefenseScenarioSo = scenarioSo as WaveDefenseScenarioSo;
            if (waveDefenseScenarioSo == null)
            {
                Debug.LogError("There error with creating wave scenario!");
                return;
            }

            waveSettingsSos = waveDefenseScenarioSo.waveSettingsSos;
        }
    }
}