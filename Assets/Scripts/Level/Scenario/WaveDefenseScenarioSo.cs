using UnityEngine;

namespace Level
{
    [CreateAssetMenu()]
    public class WaveDefenseScenarioSo : ScenarioSo
    {
        public WaveSettingsSo[] waveSettingsSos;

        public override Scenario CreateGameScenario()
        {
            return new WaveDefenseScenario(this);
        }
    }
}