using UnityEngine;

[CreateAssetMenu()]
public class MapsSo : ScriptableObject
{
    public string sceneName;
    public ScenarioSo[] scenarioSos;

    public ScenarioSo[] randomScenarioSos;

    public int GetScenariosCount()
    {
        return scenarioSos.Length + randomScenarioSos.Length;
    }
}
