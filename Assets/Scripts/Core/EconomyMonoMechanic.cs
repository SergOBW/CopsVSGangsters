using System;
using Abstract;
using Abstract.Inventory;
using Level;
using Level.LootMoneyScenario;
using Quests.LootMoney;
using Save;
using UnityEngine;

public class EconomyMonoMechanic : GlobalMonoMechanic
{
    private float _currentMoney;
    private float _tempMoney;

    public static EconomyMonoMechanic Instance;
    public event Action<float> OnMoneyAmountChanged;
    public event Action<float> OnTempMoneyAmountChanged;

    private LootMoneyScenario _lootMoneyScenario;
    private float _currentMoneyToWin;
    
    public override void Initialize()
    {
        Instance = this;
        SaveGameMechanic.Instance.OnDataRefreshed += Refresh;
        Refresh(SaveGameMechanic.Instance.GetGameSaves());
        LevelsMonoMechanic.Instance.OnLevelLoaded += SetupLevel;
        LevelsMonoMechanic.Instance.OnLevelUnLoaded += OnLevelUnLoaded;
    }

    private void SetupLevel()
    {
        switch (LevelsMonoMechanic.Instance.GetCurrentScenario())
        {
            case LootMoneyScenario lootMoneyScenario:
                SetupLootMoneyLevel(lootMoneyScenario);
                break;
        }
    }

    private void SetupLootMoneyLevel(LootMoneyScenario lootMoneyScenario)
    {
        _lootMoneyScenario = lootMoneyScenario;
        _currentMoneyToWin = CalculateMoneyToWinCount();
        CalculatePrices();
    }
    
    private void CalculatePrices()
    {
        int numLarge = 0;
        int numMedium = 0;
        int numSmall = 0;
        int numBonus = 0;
        
        LootMoneyItem[] lootMoneyItems = FindObjectsOfType<LootMoneyItem>();
        
        foreach (var lootMoneyItem in lootMoneyItems)
        {
            if (lootMoneyItem.lootMoneyType == LootMoneyType.Big)
            {
                numLarge++;
            }

            if (lootMoneyItem.lootMoneyType == LootMoneyType.Default)
            {
                numMedium++;
            }
            if (lootMoneyItem.lootMoneyType == LootMoneyType.Small)
            {
                numSmall++;
            }

            if (lootMoneyItem.lootMoneyType == LootMoneyType.Bonus)
            {
                numBonus++;
            }
        }
        
        float mediumCost = _lootMoneyScenario.startingMoneyOnLevel / (numLarge * 1.5f + numMedium + numSmall* 0.5f); // расчет стоимости среднего товара
        float smallCost = 0.5f * mediumCost; // стоимость маленького товара
        float largeCost = 1.5f * mediumCost; // стоимость большого товара
        float bonusCost = _lootMoneyScenario.startingBonusMoney / numBonus;
        
        // Выводим стоимость каждого типа товаров.
        foreach (var lootMoneyItem in lootMoneyItems)
        {
            if (lootMoneyItem.lootMoneyType == LootMoneyType.Big)
            {
                lootMoneyItem.ChangeMoneyAmount(largeCost);
            }

            if (lootMoneyItem.lootMoneyType == LootMoneyType.Default)
            {
                lootMoneyItem.ChangeMoneyAmount(mediumCost);
            }
            if (lootMoneyItem.lootMoneyType == LootMoneyType.Small)
            {
                lootMoneyItem.ChangeMoneyAmount(smallCost);
            }

            if (lootMoneyItem.lootMoneyType == LootMoneyType.Bonus)
            {
                lootMoneyItem.ChangeMoneyAmount(bonusCost);
            }
        }
        
        Debug.Log($"Medium cost {mediumCost}");
        Debug.Log($"Large cost {largeCost}");
        Debug.Log($"Small cost {smallCost}");
    }

    private void OnLevelUnLoaded()
    {
        _tempMoney = 0;
        OnTempMoneyAmountChanged?.Invoke(_tempMoney);
    }
    
    private void Refresh(GameSaves gameSaves)
    {
        _currentMoney = gameSaves.money;
        OnMoneyAmountChanged?.Invoke(_currentMoney);
    }
    
    private float CalculateMoneyToWinCount()
    {
        float moneyToWin = _lootMoneyScenario.startingMoneyOnLevel;
        if (Inventory.Instance.HasItem("Big drill"))
        {
            moneyToWin += _lootMoneyScenario.startingBonusMoney;
        }

        if (Inventory.Instance.HasItem("Great Bag"))
        {
            moneyToWin *= 1.5f;
        }
        return moneyToWin;
    }

    #region Public

    public bool TryToSpend(float value)
    {
        if (_currentMoney - value >= 0)
        {
            _currentMoney -= value;
            OnMoneyAmountChanged?.Invoke(_currentMoney);
            SoundMonoMechanic.Instance.PlayBuy();
            SaveGameMechanic.Instance.SaveMoney();
            return true;
        }
        return false;
    }

    public void AddMoney(int value)
    {
        _currentMoney += value;
        OnMoneyAmountChanged?.Invoke(_currentMoney);
        SaveGameMechanic.Instance.SaveMoney();
    }

    public void AddTempMoney(float value)
    {
        if (Inventory.Instance.HasItem("Great Bag"))
        {
            value *= 1.5f;
        }
        _tempMoney += (int)Math.Ceiling (value);
        if (_tempMoney >= _currentMoneyToWin && LevelStateMachine.Instance.IsPlayState())
        {
            LevelsMonoMechanic.Instance.WinLevel();
        }
        OnTempMoneyAmountChanged?.Invoke(_tempMoney);
    }

    public void CalculateMoney()=>AddMoney((int)_tempMoney);
    public float GetCurrentMoney() => _currentMoney;
    public float GetCurrentTempMoney() => _tempMoney;
    public void DoDoubleBonus()
    {
        _tempMoney *= 2;
        LevelsMonoMechanic.Instance.DoDoubleBonus();
    }
    public float GetCurrentMoneyToWin()=> _currentMoneyToWin;
    public float GetMaxMoney(int levelIndex)
    {
        switch (LevelsMonoMechanic.Instance.GetLevelScenario(levelIndex))
        {
            case LootMoneyScenario lootMoneyScenario:
                float tempMoney = lootMoneyScenario.startingMoneyOnLevel + lootMoneyScenario.startingBonusMoney;
                if (Inventory.Instance.HasItem("Great Bag"))
                {
                    tempMoney *= 1.5f;
                }
                return tempMoney;
        }

        return 0;
    }

    #endregion


    public void DoRewardedAddBonus()
    {
        AddMoney(5000);
        SaveGameMechanic.Instance.Save();
    }

    public int GetMoneyBonus()
    {
        return 5000;
    }
}
