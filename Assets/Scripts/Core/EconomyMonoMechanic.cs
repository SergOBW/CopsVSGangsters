using System;
using Abstract;
using Save;

public class EconomyMonoMechanic : GlobalMonoMechanic
{
    private float _currentMoney;

    public static EconomyMonoMechanic Instance;
    public event Action<float> OnMoneySpended;
    public event Action<float> OnMoneyAmountChanged;
    
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
