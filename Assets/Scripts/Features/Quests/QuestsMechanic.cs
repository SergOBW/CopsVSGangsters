using System;
using System.Collections.Generic;
using Abstract;
using Level;
using Quests;
using Quests.Hostage;
using Quests.Item;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestsMechanic : GameModeMechanic
{
    
    private List<Quest> _quests = new List<Quest>();

    public static QuestsMechanic Instance;
    private void Awake()
    {
        Instance = this;
    }

    public override void Initialize(Scenario scenario)
    {
        base.Initialize(scenario);
        LevelPassScenario levelPassScenario = scenario as LevelPassScenario;
        if (levelPassScenario != null)
        {
            SetupNewQuests(levelPassScenario.questSos);
        }
    }

    public void SetupNewQuests(QuestSo[] questSos)
    {

        if (_quests.Count>0 )
        {
            Deinitialize();
        }

        if (questSos.Length <= 0)
        {
            return;
        }
        
        foreach (var questSo in questSos)
        {
            Quest newQuest = questSo.CreateQuest();
            _quests.Add(newQuest);
            newQuest.Initialize(this);
            newQuest.OnQuestUpdated += OnQuestUpdated;
            newQuest.OnQuestCompleted += OnQuestCompleted;
        }
        
        CalculateHostages();
        CalculateLootItems();
    }

    private void Update()
    {
        if (_quests.Count <= 0) return;
        for (int i = 0; i < _quests.Count; i++)
        {
            _quests[i].Update();
        }
    }

    private void CalculateHostages()
    {
        HostageSpawnPoint[] hostageSpawnPoints = FindObjectsOfType<HostageSpawnPoint>();
        List<HostageQuest> hostageQuests = new List<HostageQuest>();

        int hostageAmount = 0;

        // Calculate hostages
        foreach (var quest in _quests)
        {
            if (quest.GetType() == typeof(HostageQuest))
            {
                HostageQuest hostageQuest = quest as HostageQuest;
                if (hostageQuest != null)
                {
                    hostageAmount += hostageQuest.HostagesAmount;
                    hostageQuests.Add(hostageQuest);
                }
            }
        }

        if (hostageSpawnPoints.Length < hostageAmount)
        {
            Debug.LogError("There is no more hostageSpawnPoints to handle all hostages from quest!!");
            return;
        }
        // Creating list of spawnPoints to better random
        List<HostageSpawnPoint> hostageSpawnPointsList = new List<HostageSpawnPoint>();
        foreach (var hostageSpawnPoint in hostageSpawnPoints)
        {
            hostageSpawnPointsList.Add(hostageSpawnPoint);
        }

        foreach (var hostageQuest in hostageQuests)
        {
            for (int i = 0; i < hostageQuest.HostagesAmount; i++)
            {
                HostageSpawnPoint hostageSpawnPoint =
                    hostageSpawnPointsList[Random.Range(0, hostageSpawnPointsList.Count)];
                Instantiate(hostageQuest.hostagePrefab, hostageSpawnPoint.transform);
                hostageSpawnPointsList.Remove(hostageSpawnPoint);
            }
        }
    }
    private void CalculateLootItems()
    { 
        ItemSpawnPoint[] itemSpawnPoints = FindObjectsOfType<ItemSpawnPoint>();
        List<LootItemQuest> lootItemQuests = new List<LootItemQuest>();

        int itemsAmount = 0;

        // Calculate hostages
        foreach (var quest in _quests)
        {
            if (quest.GetType() == typeof(LootItemQuest))
            {
                LootItemQuest hostageQuest = quest as LootItemQuest;
                if (hostageQuest != null)
                {
                    itemsAmount += hostageQuest.LootAmount;
                    lootItemQuests.Add(hostageQuest);
                }
            }
        }

        if (itemSpawnPoints.Length < itemsAmount)
        {
            Debug.LogError("There is no more loot spawnpoints to handle all loot from quest!!");
            return;
        }
        // Creating list of spawnPoints to better random
        List<ItemSpawnPoint> lootSpawnPoints = new List<ItemSpawnPoint>();
        foreach (var hostageSpawnPoint in itemSpawnPoints)
        {
            lootSpawnPoints.Add(hostageSpawnPoint);
        }

        foreach (var lootItemQuest in lootItemQuests)
        {
            for (int i = 0; i < lootItemQuest.LootAmount; i++)
            {
                ItemSpawnPoint hostageSpawnPoint =
                    lootSpawnPoints[Random.Range(0, lootSpawnPoints.Count)];
                Instantiate(lootItemQuest.lootItemPrefab, hostageSpawnPoint.transform);
                lootSpawnPoints.Remove(hostageSpawnPoint);
            }
        }
    }

    private void OnQuestCompleted(Quest quest)
    {
        if (_quests.Contains(quest))
        {
            _quests.Remove(quest);
        }

        if (_quests.Count <= 0 && LevelStateMachine.Instance.IsPlayState())
        {
            LevelsMonoMechanic.Instance.WinLevel();
        }
    }

    private void Deinitialize()
    {
        foreach (var quest in _quests)
        {
            quest.OnQuestUpdated -= OnQuestUpdated;
        }

        _quests = new List<Quest>();
    }

    private void OnQuestUpdated(Quest quest)
    {
        
    }

    public void TryToProgressQuest(QuestItem questItem)
    {
        foreach (var quest in _quests)
        {
            if (quest.IsQuestItemValid(questItem))
            {
                quest.TryToProgressQuest(questItem);
                break;
            }
        }
    }

    public List<Quest> GetQuests()
    {
        return _quests;
    }
    
}
