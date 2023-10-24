using System;
using Abstract;
using Save;
using UnityEngine;

public class EconomyMonoMechanic : GlobalMonoMechanic
{
    private float _currentMoney;
    private int _tempMoney;

    public static EconomyMonoMechanic Instance;
    public event Action<float> OnMoneyAmountChanged;
    public event Action<float> OnTempMoneyAmountChanged;
    
    public override void Initialize()
    {
        Instance = this;
        SaveGameMechanic.Instance.OnDataRefreshed += Refresh;
        Refresh(SaveGameMechanic.Instance.GetGameSaves());
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

    public void AddTempMoney(int value)
    {
        _tempMoney += value;
        OnTempMoneyAmountChanged?.Invoke(_tempMoney);
    }

    public void CalculateMoney()
    {
        AddMoney(_tempMoney);
    }
    
    public float GetCurrentMoney()
    {
        return _currentMoney;
    }

    public int GetCurrentTempMoney()
    {
        return _tempMoney;
    }

    public void DoDoubleBonus()
    {
        _tempMoney *= 2;
    }
}
