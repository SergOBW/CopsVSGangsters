﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    [Serializable]
    public class WaveSettings
    {
        public int enemyCount;
    }
    [CreateAssetMenu()]
    public class WaveDefenseScenarioSo : ScenarioSo
    {
        [FormerlySerializedAs("waveSettingsSos")] public WaveSettings[] waveSettings;

        public override Scenario CreateGameScenario()
        {
            return new WaveDefenseScenario(this);
        }
    }
}