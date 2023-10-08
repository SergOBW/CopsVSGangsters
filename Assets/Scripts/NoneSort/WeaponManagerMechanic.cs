using System;
using System.Collections.Generic;
using Abstract;
using ForWeapon;
using ForWeapon.New;
using UnityEngine;
using Yandex.Plugins.Login;

public class PlayerWeaponStats
{
    // Default
    public readonly string WeaponName;

    public int WeaponBuyPrice;
    
    private readonly float _startedDamageCost;
    private readonly float _startedReloadSpeedCost;
    private readonly float _startedAccuracyCost;
    private readonly int _startedBulletCountCost;

    private readonly float _startedDamage;
    private readonly float _startedReloadSpeed;
    private readonly float _startedAccuracy;
    private readonly int _startedBulletCount;

    private readonly float _damageLevelMultiplayer;
    private readonly float _reloadSpeedLevelMultiplayer;
    private readonly float _accuracyLevelMultiplayer;
    private readonly int _bulletCountLevelMultiplayer;
    
    private readonly float _damagePriceLevelMultiplayer;
    private readonly float _reloadSpeedPriceLevelMultiplayer;
    private readonly float _accuracyPriceLevelMultiplayer;
    private readonly int _bulletCountPriceLevelMultiplayer;

    
    // GamePlay Mechanics
    public int DamageLevel;
    public int ReloadSpeedLevel;
    public int AccuracyLevel;
    public int BulletCountLevel;

    public int IsOpen;
    
    // Current 

    public float Damage { get; private set; }
    public float ReloadSpeed { get; private set; }
    public int BulletCount { get; private set; }
    public float Accuracy { get; private set; }
    
    public float DamageUpgradePrice { get; set; }
    public float ReloadSpeedPrice { get; set; }
    public int BulletCountPrice { get; set; }
    public float AccuracyPrice { get; set; }

    public GameObject WeaponArms;

    public PlayerWeaponStats(PlayerWeaponStatsSo playerWeaponStatsSo)
    {
        // Initialize
        WeaponName = playerWeaponStatsSo.weaponName;
        WeaponBuyPrice = playerWeaponStatsSo.weaponBuyPrice;
        // Prices
        _startedDamageCost = playerWeaponStatsSo.startedDamageCost;
        _startedReloadSpeedCost = playerWeaponStatsSo.startedReloadSpeedCost;
        _startedAccuracyCost = playerWeaponStatsSo.startedAccuracyCost;
        _startedBulletCountCost = playerWeaponStatsSo.startedBulletCountCost;
        // Stats
        _startedDamage = playerWeaponStatsSo.startedDamage;
        _startedReloadSpeed = playerWeaponStatsSo.startedReloadSpeed;
        _startedAccuracy = playerWeaponStatsSo.startedAccuracy;
        _startedBulletCount = playerWeaponStatsSo.startedBulletCount;
        // Stats multiplayer
        _damageLevelMultiplayer = playerWeaponStatsSo.damageLevelMultiplayer;
        _reloadSpeedLevelMultiplayer = playerWeaponStatsSo.reloadSpeedLevelMultiplayer;
        _accuracyLevelMultiplayer = playerWeaponStatsSo.accuracyLevelMultiplayer;
        _bulletCountLevelMultiplayer = playerWeaponStatsSo.bulletCountLevelMultiplayer;
        // Price multiplayer
        _damagePriceLevelMultiplayer = playerWeaponStatsSo.damagePriceLevelMultiplayer;
        _reloadSpeedPriceLevelMultiplayer = playerWeaponStatsSo.reloadSpeedPriceLevelMultiplayer;
        _bulletCountPriceLevelMultiplayer = playerWeaponStatsSo.bulletCountPriceLevelMultiplayer;
        _accuracyPriceLevelMultiplayer = playerWeaponStatsSo.accuracyPriceLevelMultiplayer;
        // Load saves
        SaveWeapon saveWeapon = SaveManagerMechanic.Instance.GetWeaponSave(WeaponName);

        DamageLevel = saveWeapon.DamageLevel;
        ReloadSpeedLevel = saveWeapon.ReloadSpeedLevel;
        AccuracyLevel = saveWeapon.AccuracyLevel;
        BulletCountLevel = saveWeapon.BulletCountLevel;

        IsOpen = saveWeapon.IsOpen;

        //WeaponArms = playerWeaponStatsSo.weaponArms;
    }

    public void CalculateStats()
    {
        Damage = _startedDamage + _damageLevelMultiplayer * DamageLevel;
        ReloadSpeed = _startedReloadSpeed + _reloadSpeedLevelMultiplayer * ReloadSpeedLevel;
        BulletCount = _startedBulletCount + _bulletCountLevelMultiplayer * BulletCountLevel;
        Accuracy = Math.Clamp(_startedAccuracy - _accuracyLevelMultiplayer * AccuracyLevel,0,20);

        DamageUpgradePrice = _startedDamageCost + _damagePriceLevelMultiplayer * DamageLevel;
        ReloadSpeedPrice = _startedReloadSpeedCost + _reloadSpeedPriceLevelMultiplayer * ReloadSpeedLevel;
        BulletCountPrice = _startedBulletCountCost + _bulletCountPriceLevelMultiplayer * BulletCountLevel;
        AccuracyPrice = _startedAccuracyCost + _accuracyPriceLevelMultiplayer * AccuracyLevel;
    }
}

public class WeaponManagerMechanic : GlobalMechanic
{
    public static WeaponManagerMechanic Instance;

    [SerializeField] private PlayerWeaponStatsSo[] _playerWeaponStatsSo;
    private List<PlayerWeaponStats> _playerWeaponStats = new List<PlayerWeaponStats>();
    private PlayerCharacter _playerCharacter;
    
    [SerializeField]private string _currentWeaponName = "Makarov";
    [SerializeField]private string startingWeaponName = "MAKAROV";

    public event Action OnNewWeaponValues;

    private int _grenadeCount;
    [SerializeField] private int startingGrenadeCount = 3;
    [SerializeField] private int grenadeDamage = 100;

    private void Awake()
    {
        Instance = this;
    }
    
    public override void Initialize()
    {
        SaveManagerMechanic.Instance.OnDataRefreshed += Setup;
        Setup();
    }
    
    private void Setup()
    {
        _playerWeaponStats = new List<PlayerWeaponStats>();
        foreach (var playerWeaponStatsSo in _playerWeaponStatsSo)
        {
            PlayerWeaponStats newPlayerWeapon = new PlayerWeaponStats(playerWeaponStatsSo);
            SaveWeapon saveWeapon = SaveManagerMechanic.Instance.GetWeaponSave(newPlayerWeapon.WeaponName);
            newPlayerWeapon.DamageLevel = saveWeapon.DamageLevel;
            newPlayerWeapon.ReloadSpeedLevel = saveWeapon.ReloadSpeedLevel;
            newPlayerWeapon.BulletCountLevel = saveWeapon.BulletCountLevel;
            newPlayerWeapon.AccuracyLevel = saveWeapon.AccuracyLevel;
            newPlayerWeapon.CalculateStats();
            _playerWeaponStats.Add(newPlayerWeapon);
        }

        _grenadeCount = startingGrenadeCount;
        OnNewWeaponValues?.Invoke();
    }

    public void InitializeNewPlayer(PlayerCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
    }

    #region Getters

    public PlayerWeaponStats GetWeaponStats(string weaponName)
    {
        foreach (var playerWeaponStats in _playerWeaponStats)
        {
            if (playerWeaponStats.WeaponName == weaponName)
            {
                return playerWeaponStats;
            }
        }
        return null;
    }

    public List<PlayerWeaponStats> GetWeaponsStats()
    {
        return _playerWeaponStats;
    }
    
    public PlayerWeaponStatsSo[] GetPlayerWeaponStatsSo()
    {
        return _playerWeaponStatsSo;
    }
    
    public float GetCurrentWeaponDamage()
    {
        float damage = 30;
        string weaponName = _playerCharacter.GetWeaponName();
        PlayerWeaponStats playerWeaponStats = GetWeaponStats(weaponName);
        if (playerWeaponStats == null)
        {
            return damage;
        }

        playerWeaponStats.CalculateStats();
        damage = playerWeaponStats.Damage;
        return damage;
    }
    
    public WeaponBehaviour[] GetWeaponsToPlay(WeaponBehaviour[] weaponBehaviours)
    {
        List<WeaponBehaviour> avaibleWeaponBehaviours = new List<WeaponBehaviour>();
        WeaponBehaviour[] weaponBehavioursFromInventory = weaponBehaviours;
        if (weaponBehavioursFromInventory.Length <= 0)
        {
            Debug.Log("There is no weapons in inventory");
            return null;
        }

        for (int i = 0; i < weaponBehavioursFromInventory.Length; i++)
        {
            string weaponName = weaponBehavioursFromInventory[i].GetWeaponName();
            PlayerWeaponStats playerWeaponStats = null;
            foreach (var playerWeaponStat in _playerWeaponStats)
            {
                if (playerWeaponStat.WeaponName == weaponName)
                {
                    playerWeaponStats = playerWeaponStat;
                    break;
                }
            }

            if (playerWeaponStats == null)
            {
                Debug.Log("There is no weaponStat with that name");
                continue;
            }

            if (playerWeaponStats.IsOpen == 1)
            {
                avaibleWeaponBehaviours.Add(weaponBehavioursFromInventory[i]);
            }
        }

        return avaibleWeaponBehaviours.ToArray();

    }
    
    public string GetStartedWeaponName()
    {
        return startingWeaponName;
    }
    
    public float GetGrenadeDamage()
    {
        return grenadeDamage;
    }

    #endregion
    
    
    public void SetWeapon(string weaponName)
    {
        _currentWeaponName = weaponName;
    }
    
    public void TryToUpgradeStat(WeaponStatType type)
    {
        Debug.Log($"TryToUpgrade{type}");
    
        PlayerWeaponStats playerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[0]);
    
        foreach (var playerWeaponStats in _playerWeaponStats)
        {
            if (playerWeaponStats.WeaponName == _currentWeaponName)
            {
                playerWeapon = playerWeaponStats;
                break;
            }
        }
    
        switch (type)
        {
            case WeaponStatType.Damage:
                if (playerWeapon.DamageLevel >= 5) return;
                if (EconomyMechanic.Instance.TryToSpend(playerWeapon.DamageUpgradePrice))
                    playerWeapon.DamageLevel++;
                break;
            case WeaponStatType.ReloadSpeed:
                if (playerWeapon.ReloadSpeedLevel >= 5) return;
                if (EconomyMechanic.Instance.TryToSpend(playerWeapon.ReloadSpeedPrice))
                    playerWeapon.ReloadSpeedLevel++;
                break;
            case WeaponStatType.BulletCount:
                if (playerWeapon.BulletCountLevel >= 5) return;
                if (EconomyMechanic.Instance.TryToSpend(playerWeapon.BulletCountPrice))
                    playerWeapon.BulletCountLevel++;
                break;
            case WeaponStatType.Accuracy:
                if (playerWeapon.AccuracyLevel >= 5) return;
                if (EconomyMechanic.Instance.TryToSpend(playerWeapon.AccuracyPrice))
                    playerWeapon.AccuracyLevel++;
                break;
            default:
                return;
        }
    
        playerWeapon.CalculateStats();
        SaveManagerMechanic.Instance.SaveWeapon(playerWeapon);
    }

    public bool TryToBuyWeapon(string weaponName)
    {
        Debug.Log("Try to buy " + weaponName );
        PlayerWeaponStats playerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[0]);
    
        foreach (var playerWeaponStats in _playerWeaponStats)
        {
            if (playerWeaponStats.WeaponName == weaponName)
            {
                playerWeapon = playerWeaponStats;
                break;
            }
        }

        if (playerWeapon.IsOpen > 0)
        {
            Debug.Log("You already have this weapon");
            return false;
        }

        if (EconomyMechanic.Instance.TryToSpend(playerWeapon.WeaponBuyPrice))
        {
            playerWeapon.IsOpen = 1;
        }
        
        SaveManagerMechanic.Instance.SaveWeapon(playerWeapon);
        return true;
    }

    public bool CanBuyUpgradeOnCurrentWeapon()
    {
        bool isAllOk = false;
        PlayerWeaponStats playerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[0]);
    
        foreach (var playerWeaponStats in _playerWeaponStats)
        {
            if (playerWeaponStats.WeaponName == _currentWeaponName)
            {
                playerWeapon = playerWeaponStats;
                break;
            }
        }
        if (EconomyMechanic.Instance.HasEnoughMoney(playerWeapon.DamageUpgradePrice))
        {
            if (playerWeapon.DamageLevel < 5) isAllOk = true;
        }

        if (EconomyMechanic.Instance.HasEnoughMoney(playerWeapon.ReloadSpeedPrice))
        {
            if (playerWeapon.ReloadSpeedLevel < 5) isAllOk = true;
        }
        if (EconomyMechanic.Instance.HasEnoughMoney(playerWeapon.BulletCountPrice))
        {
            if (playerWeapon.BulletCountLevel < 5) isAllOk = true;
        }
        if (EconomyMechanic.Instance.HasEnoughMoney(playerWeapon.AccuracyPrice))
        {
            if (playerWeapon.AccuracyLevel < 5) isAllOk = true;
        }

        return isAllOk;
    }

    public bool IsMaxLevel(WeaponStatType type, string weaponName)
    {
        PlayerWeaponStats playerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[0]);
    
        foreach (var playerWeaponStats in _playerWeaponStats)
        {
            if (playerWeaponStats.WeaponName == weaponName)
            {
                playerWeapon = playerWeaponStats;
                break;
            }
        }
    
        switch (type)
        {
            case WeaponStatType.Damage:
                if (playerWeapon.DamageLevel >= 5) return true;
                break;
            case WeaponStatType.ReloadSpeed:
                if (playerWeapon.ReloadSpeedLevel >= 5) return true;
                break;
            case WeaponStatType.BulletCount:
                if (playerWeapon.BulletCountLevel >= 5) return true;
                break;
            case WeaponStatType.Accuracy:
                if (playerWeapon.AccuracyLevel >= 5) return true;
                break;
            default:
                return false;
        }
        
        return false;
    }
    

    public void TrowGrenade()
    {
        _grenadeCount--;
    }

    public bool CanThrowGrenade()
    {
        return _grenadeCount > 0;
    }
}

public class WeaponBehaviour
{
    public string GetWeaponName()
    {
        throw new NotImplementedException();
    }
}
