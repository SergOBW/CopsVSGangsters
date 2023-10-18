using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using DefaultNamespace;
using EnemyCore;
using JetBrains.Annotations;
using Level;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yandex.Plugins.Login;
using Random = UnityEngine.Random;

[Serializable]
public class LevelSave
{
    public int completedStars;
    public int isOpen;
}

[RequireComponent(typeof(GameModeMechanicsManager))]
public class LevelsMonoMechanic : GlobalMonoMechanic
{
    public static LevelsMonoMechanic Instance;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private PlayerStatsSo defaultPlayerStats;

    [SerializeField] private int startedLevel;
    
    [SerializeField] private MapsSo[] mapsSo;

    [SerializeField] private int startedLevelWinMoney = 1000;
    [SerializeField] private int eachLevelWinMoney = 200;
    
    
    private GameLevelInfo _currentGameLevelInfo;

    public event Action OnLevelLoaded;
    public event Action OnLevelUnLoaded;

    public event Action OnLevelLoose;
    public event Action OnLevelWin;

    private int currentLevelIndex;
    private int lastCompletedLevelIndex;

    private List<LevelSave> levelSaves;

    private GameModeMechanicsManager _gameModeMechanicsManager;
    private LevelStateMachine _levelStateMachine;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var levelSave in levelSaves)
            {
                levelSave.isOpen = 1;
            }
            
            SaveGameMechanic.Instance.SaveLeveSaves(levelSaves);
        }

        if (_levelStateMachine != null)
        {
            _levelStateMachine.Update();
        }
        
    }

    public override void Initialize()
    {
        Instance = this;
        SaveGameMechanic.Instance.OnDataRefreshed+= OnDataRefreshed;
        _gameModeMechanicsManager = GetComponent<GameModeMechanicsManager>();
        _levelStateMachine = new LevelStateMachine();
        _levelStateMachine.Initialize();
        OnDataRefreshed();
    }

    private void OnDataRefreshed()
    {
        levelSaves = SaveGameMechanic.Instance.GetGameSaves().LevelSaves;

        if (levelSaves == null)
        {
            levelSaves = new List<LevelSave>();
            lastCompletedLevelIndex = 0;
            LevelSave levelSave = new LevelSave{ completedStars = 0, isOpen = 1 };
            levelSaves.Add(levelSave);

            Debug.LogError("There is no Level saves");
            return;
        }

        for (int i = 0; i < levelSaves.Count; i++)
        {
            if (levelSaves[i].isOpen == 0)
            {
                lastCompletedLevelIndex = i - 1;
                break;
            }
        }
    }

    #region LevelOperations

    public void SelectLevel(int levelId = 0)
    {
        if (levelId == 0)
        {
            levelId = startedLevel;
            Debug.Log("Need to load last level from save manager");
        }
        currentLevelIndex = levelId;
        SetCurrentGameLevel();
        string sceneName = _currentGameLevelInfo.sceneName;
        StartCoroutine(LoadScene(sceneName));
    }
    
    private IEnumerator LoadScene(string sceneName)
    {
        LevelStateMachine.Instance.Initialize();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // Setup player
        GameObject playerGameObject =  Instantiate(playerPrefab, FindObjectOfType<PlayerSpawnPoint>().transform.position,FindObjectOfType<PlayerSpawnPoint>().transform.rotation);
        PlayerStatsController playerStatsController = playerGameObject.GetComponent<PlayerStatsController>();
        playerGameObject.GetComponent<PlayerCharacter>().SetupArms();
        playerStatsController.Initialize(defaultPlayerStats);
        playerStatsController.OnPlayerDie += LooseLevel;
        _gameModeMechanicsManager.Initialize(_currentGameLevelInfo);
        OnLevelLoaded?.Invoke();
    }

    public void ExitLevel()
    {
        StartCoroutine(UnLoadScene());
    }
    
    private IEnumerator UnLoadScene()
    {
        _gameModeMechanicsManager.DeInitialize();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Hub");
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        LevelStateMachine.Instance.LevelUnloaded();
        OnLevelUnLoaded?.Invoke();
    }
    
    public void NextLevel()
    {
        _gameModeMechanicsManager.DeInitialize();
        if (currentLevelIndex + 1 < GetTotalLevelCount())
        {
            currentLevelIndex++;
        }
        else
        {
            Debug.Log("You win the game congrats!");
        }
        SetCurrentGameLevel();
        StartCoroutine(LoadScene(_currentGameLevelInfo.sceneName.ToString()));
    }

    public void RestartLevel()
    {
        _gameModeMechanicsManager.DeInitialize();
        StartCoroutine(LoadScene(_currentGameLevelInfo.sceneName.ToString()));
    }

    public void WinLevel()
    {
        int completedStars = 1;
        int money = startedLevelWinMoney + eachLevelWinMoney * currentLevelIndex;
        if (EnemyHandleMechanic.Instance.IsHeadBonus())
        {
            money += 200;
            completedStars++;
        }

        if (FindObjectOfType<PlayerStatsController>().IsHpBonus())
        {
            money += 200;
            completedStars++;
        }
        levelSaves[currentLevelIndex].completedStars = completedStars;
        EconomyMonoMechanic.Instance.AddMoney(money);
        // Unlock next level
        if (currentLevelIndex + 1 < GetTotalLevelCount())
        {
            levelSaves[currentLevelIndex + 1].isOpen = 1;
        }
        else
        {
            Debug.Log("You win the game congrats!");
        }
        SaveGameMechanic.Instance.SaveLeveSaves(levelSaves);
        OnLevelWin?.Invoke();
    }

    public void LooseLevel()
    {
        LevelStateMachine.Instance.ChangeState(LevelStateMachine.Instance.levelMonoEndState);
        OnLevelLoose?.Invoke();
    }

    #endregion
    
    #region Getters

    public int GetTotalLevelCount()
    {
        int maxLevels = 0;
        foreach (var mapSo in mapsSo)
        {
            maxLevels += mapSo.GetScenariosCount();
        }
        return maxLevels;
    }

    public LevelSave GetInfoAboutLevel(int levelNumber)
    {
        return levelSaves[levelNumber];
    }
    
    #endregion
    
    private void SetCurrentGameLevel()
    {
        int currentScenarioIndex = (int)MathF.Floor(currentLevelIndex / mapsSo.Length);
        int currentMapIndex = currentLevelIndex % mapsSo.Length;

        int maxScriptedLevels = 0;
        
        foreach (var mapsSo in mapsSo)
        {
            foreach (var scenario in mapsSo.scenarioSos)
            {
                maxScriptedLevels++;
            }
        }
        
        MapsSo _currentMap = mapsSo[currentMapIndex];
        Scenario levelPassScenario;
        if (currentLevelIndex < maxScriptedLevels)
        {
            levelPassScenario = _currentMap.scenarioSos[currentScenarioIndex].CreateGameScenario();
        }
        else
        {
            levelPassScenario = _currentMap.randomScenarioSos[Random.Range(0,_currentMap.randomScenarioSos.Length)].CreateGameScenario();
        }
        _currentGameLevelInfo = new GameLevelInfo(_currentMap,levelPassScenario);
    }

    public int GetLastSavedLevel()
    {
        OnDataRefreshed();
        return lastCompletedLevelIndex;
    }

    public int GetStarts()
    {
        return levelSaves[currentLevelIndex].completedStars;
    }

    [CanBeNull]
    public Scenario GetCurrentScenario()
    {
        if (_currentGameLevelInfo != null)
        {
            return _currentGameLevelInfo.currentGameScenario;
        }
        return null;
    }


    public int GetMapsCount()
    {
        return mapsSo.Length;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            LevelStateMachine.Instance.Pause();
        }
    }
}
