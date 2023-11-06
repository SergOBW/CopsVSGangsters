using Abstract;
using Level;
using UnityEngine;

public class GameModeMechanicsManager : MonoBehaviour
{
    public static GameModeMechanicsManager Instance;
    
    [SerializeField] private GameModeMechanic[] waveDefenceMechanics;
    [SerializeField] private GameModeMechanic[] levelByLevelMechanics;

    private GameModeMechanic[] _currentGameModeMechanics;

    private void Awake()
    {
        Instance = this;
    }


    public void Initialize(GameLevelInfo gameLevelInfo)
    {
        switch (gameLevelInfo.currentGameScenario)
        {
            case WaveDefenseScenario waveDefenseScenario:
                InitializeWaveDefenceGame(waveDefenseScenario);
                break;
            case QuestScenario levelPassScenario:
                InitializeLevelByLevelGame(levelPassScenario);
                break;
            default: InitializeExploration(gameLevelInfo.currentGameScenario);
                break;
        }
    }
    private void InitializeExploration(Scenario scenario)
    {
        Debug.Log("The Exploration initialized scenario name = " + scenario.Name);
    }

    private void InitializeWaveDefenceGame(WaveDefenseScenario waveDefenseScenario)
    {
        //Debug.Log("The WaveDefenseScenario initialized scenario name = " + waveDefenseScenario.Name);
        foreach (var gameModeMechanic in waveDefenceMechanics)
        {
            gameModeMechanic.Initialize(waveDefenseScenario);
        }
    }
    
    private void InitializeLevelByLevelGame(QuestScenario questScenario)
    {
        //Debug.Log("The LevelPassScenario initialized scenario name = " + levelPassScenario.Name);
        foreach (var gameModeMechanic in levelByLevelMechanics)
        {
            gameModeMechanic.Initialize(questScenario);
        }
    }
    

    public void DeInitialize()
    {
        if (_currentGameModeMechanics != null)
        {
            foreach (var gameModeMechanic in _currentGameModeMechanics)
            {
                gameModeMechanic.DeInitialize();
            }
        }
    }
}
