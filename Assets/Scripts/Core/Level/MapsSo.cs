using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class MapsSo : ScriptableObject
{
    public string sceneName;
    public ScenarioSo defaultScenario;

    public ScenarioSo[] randomScenarioSos;
    public Sprite iconSprite;
}
