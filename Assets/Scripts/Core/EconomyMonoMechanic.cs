using System;
using Abstract;
using Level;
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
        LootMoneyItem[] lootMoneyItems = FindObjectsOfType<LootMoneyItem>();
        float moneyOnLevel = 20000;

        CalculatePrices(moneyOnLevel,lootMoneyItems);

    }
    
    private void CalculatePrices(float totalCost,LootMoneyItem[] lootMoneyItems)
    {
        int numLarge = 0;
        int numMedium = 0;
        int numSmall = 0;
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
        }
        
        float mediumCost = totalCost / (1.5f * numLarge + numMedium + 0.5f * numSmall); // расчет стоимости среднего товара
        float smallCost = 0.5f * mediumCost; // стоимость маленького товара
        float largeCost = 1.5f * mediumCost; // стоимость большого товара
        
        

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
        }
        
        
        Debug.Log(lootMoneyItems.Length);
        Debug.Log(totalCost);
        
        Debug.Log(totalCost / lootMoneyItems.Length);
        
        Debug.Log(largeCost);
        Debug.Log(mediumCost);
        Debug.Log(smallCost);
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
        _tempMoney += value;
        OnTempMoneyAmountChanged?.Invoke(_tempMoney);
    }

    public void CalculateMoney()
    {
        AddMoney((int)_tempMoney);
    }
    
    public float GetCurrentMoney()
    {
        return _currentMoney;
    }

    public float GetCurrentTempMoney()
    {
        return _tempMoney;
    }

    public void DoDoubleBonus()
    {
        _tempMoney *= 2;
    }
}
