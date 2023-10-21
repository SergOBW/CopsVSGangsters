namespace Level
{ 
    public class GameLevelInfo
    {
        public readonly string sceneName;
        public Scenario currentGameScenario;

        public GameLevelInfo(MapsSo mapsSo, Scenario scenario)
        {
            sceneName = mapsSo.sceneName;
            currentGameScenario = scenario;
        }
    }
}