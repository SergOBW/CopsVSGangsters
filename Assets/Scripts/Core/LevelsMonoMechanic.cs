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
    
    [SerializeField] private MapsSo[] mapsSo;
    private GameLevelInfo _currentGameLevelInfo;
    
    private int currentLevelIndex;
    private int lastCompletedLevelIndex;
    
    private List<SaveLevel> levelSaves;
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
            levelSaves = new List<SaveLevel>();
            lastCompletedLevelIndex = 0;
            SaveLevel saveLevel = new SaveLevel{ lootedMoney = 0, isOpen = 1 };
            levelSaves.Add(saveLevel);

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

    public void Play()
    {
        SelectLevel(lastCompletedLevelIndex);
    }
    public void SelectLevel(int levelId = 0)
    {
        currentLevelIndex = levelId;
        _currentGameLevelInfo = GetCurrentGameLevelInfo();
        string sceneName = _currentGameLevelInfo.sceneName;
        StartCoroutine(LoadScene(sceneName));
    }
    
    private GameLevelInfo GetCurrentGameLevelInfo()
    {
        MapsSo _currentMap = mapsSo[currentLevelIndex];
        Scenario levelScenario = _currentMap.defaultScenario.CreateGameScenario();
        return new GameLevelInfo(_currentMap,levelScenario);
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
        levelSaves[currentLevelIndex].lootedMoney = EconomyMonoMechanic.Instance.GetCurrentTempMoney();
        if (currentLevelIndex + 1 < levelSaves.Count)
        {
            levelSaves[currentLevelIndex + 1].isOpen = 1;
        }
        SaveGameMechanic.Instance.SaveLeveSaves(levelSaves);
        OnLevelWin?.Invoke();
    }

    public void DoDoubleBonus()
    {
        levelSaves[currentLevelIndex].lootedMoney = EconomyMonoMechanic.Instance.GetCurrentTempMoney();
        SaveGameMechanic.Instance.SaveLeveSaves(levelSaves);
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
    
    public Sprite GetMapImage(int i)
    {
        return mapsSo[i].iconSprite;
    }

    public SaveLevel GetLevelSave(int i)
    {
        return levelSaves[i];
    }
    
    #endregion
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            LevelStateMachine.Instance.Pause();
        }
    }

    public Scenario GetLevelScenario(int i)
    {
        return mapsSo[i].defaultScenario.CreateGameScenario();
    }
}
