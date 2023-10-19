using System;
using Abstract;
using UnityEngine;
using Yandex.Plugins.Login;

public class EconomyMonoMechanic : GlobalMonoMechanic
{
    [SerializeField] private float startingMoney;
    private float _currentMoney;

    public static EconomyMonoMechanic Instance;
    public event Action<float> OnMoneySpended;
    public event Action<float> OnMoneyAmountChanged;
    
    public override void Initialize()
    {
        Instance = this;
        SaveGameMechanic.Instance.OnDataRefreshed += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        _currentMoney = SaveGameMechanic.Instance.GetMoneySave();
        OnMoneyAmountChanged?.Invoke(_currentMoney);
    }

    public bool TryToSpend(float value)
    {
        if (_currentMoney - value >= 0)
        {
            _currentMoney -= value;
            OnMoneySpended?.Invoke(value);
            OnMoneyAmountChanged?.Invoke(_currentMoney);
            SoundMonoMechanic.Instance.PlayBuy();
            SaveGameMechanic.Instance.SaveMoney();
            return true;
        }
        return false;
    }
    
    public bool HasEnoughMoney(float value)
    {
        if (_currentMoney - value >= 0)
        {
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
    

    public float GetCurrentMoney()
    {
        return _currentMoney;
    }
}
