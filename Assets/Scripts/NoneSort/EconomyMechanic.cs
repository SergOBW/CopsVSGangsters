using System;
using Abstract;
using UnityEngine;
using Yandex.Plugins.Login;

public class EconomyMechanic : GlobalMechanic
{
    [SerializeField] private float startingMoney;
    private float _currentMoney;

    public static EconomyMechanic Instance;
    public event Action<float> OnMoneySpended;
    public event Action<float> OnMoneyAmountChanged;
    private void Awake()
    {
        Instance = this;
    }
    
    public override void Initialize()
    {
        SaveManagerMechanic.Instance.OnDataRefreshed += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        _currentMoney = SaveManagerMechanic.Instance.GetMoneySave();
        OnMoneyAmountChanged?.Invoke(_currentMoney);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMoney(1000000);
            SaveManagerMechanic.Instance.SaveMoney();
            SaveManagerMechanic.Instance.Save();
        }
        */
    }

    public bool TryToSpend(float value)
    {
        if (_currentMoney - value >= 0)
        {
            _currentMoney -= value;
            OnMoneySpended?.Invoke(value);
            OnMoneyAmountChanged?.Invoke(_currentMoney);
            SoundMechanic.Instance.PlayBuy();
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
    }
    

    public float GetCurrentMoney()
    {
        return _currentMoney;
    }
}
