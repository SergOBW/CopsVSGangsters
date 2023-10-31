using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Core;
using JetBrains.Annotations;
using Level;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GameModeMechanicsManager))]
public class LevelsMonoMechanic : GlobalMonoMechanic
{
    public static LevelsMonoMechanic Instance;
    public event Action OnLevelLoaded;
    public event Action OnLevelUnLoaded;

    public event Action OnLevelLoose;
    public event Action OnLevelWin;
    
    [SerializeField] private int startedLevel;
    [SerializeField] private MapsSo[] mapsSo;
    private GameLevelInfo _currentGameLevelInfo;
    
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
        
        OnDataRefreshed(SaveGameMechanic.Instance.GetGameSaves());
    }

    private void OnDataRefreshed(GameSaves gameSaves)
    {
        levelSaves = gameSaves.LevelSaves;

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
    
    private IEnumerator LoadScene(string sceneName)
    {
        LevelStateMachine.Instance.Initialize();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // Setup player
        PlayerMonoMechanic.Instance.SpawnNewCharacter();
        
        // Setup Game mode
        _gameModeMechanicsManager.Initialize(_currentGameLevelInfo);
        
        // Level loaded
        OnLevelLoaded?.Invoke();
    }

    public void ExitLevel()
    {
        StartCoroutine(UnLoadScene());
    }
    
    private IEnumerator UnLoadScene()
    {
        _gameModeMechanicsManager.DeInitialize();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        LevelStateMachine.Instance.LevelUnloaded();
        OnLevelUnLoaded?.Invoke();
    }
    
    public void RestartLevel()
    {
        Debug.Log("Level restart");
        _gameModeMechanicsManager.DeInitialize();
        StartCoroutine(LoadScene(_currentGameLevelInfo.sceneName));
    }

    public void WinLevel()
    {
        Debug.Log("Level win");
        OnLevelWin?.Invoke();
    }

    public void LooseLevel()
    {
        Debug.Log("Level loose");
        OnLevelLoose?.Invoke();
    }

    #endregion
    
    #region Getters
    
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
    
    #endregion
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            LevelStateMachine.Instance.Pause();
        }
    }

    public Sprite GetMapImage(int i)
    {
        return mapsSo[i].iconSprite;
    }
}
