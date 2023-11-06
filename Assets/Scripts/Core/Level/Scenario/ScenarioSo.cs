using Level;
using UnityEngine;

public class ScenarioSo : ScriptableObject
{
    public string Name = "Default Scenario";

    public virtual Scenario CreateGameScenario()
    {
        return new Scenario(this);
    }
    
}