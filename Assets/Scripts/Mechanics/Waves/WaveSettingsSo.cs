using UnityEngine;

[CreateAssetMenu()]
public class WaveSettingsSo : ScriptableObject
{
    public int enemyCount;
    public int enemySpawnSpeed;
    public int waveSpawnSpeed;

    public EnemySo[] enemySos;
}
