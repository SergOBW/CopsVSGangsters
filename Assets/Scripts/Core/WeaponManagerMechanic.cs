using System;
using System.Collections.Generic;
using Abstract;
using ForWeapon;
using ForWeapon.New;
using Save;
using UnityEngine;

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
    
    public bool isStarted;

    
    // GamePlay Mechanics
    public int DamageLevel;
    public int ReloadSpeedLevel;
    public int AccuracyLevel;
    public int BulletCountLevel;

    public bool IsOpen;
    
    // Current 

    public float Damage { get; private set; }
    public float ReloadSpeed { get; private set; }
    public int BulletCount { get; private set; }
    public float Accuracy { get; private set; }
    
    public float DamageUpgradePrice { get; set; }
    public float ReloadSpeedPrice { get; set; }
    public int BulletCountPrice { get; set; }
    public float AccuracyPrice { get; set; }
    public GameObject WeaponGameObject { get; set; }
    public Sprite WeaponIcon { get; set; }

    public GameObject WeaponArms;

    public bool IsEquiped;
    public WeaponType weaponType;

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

        isStarted = playerWeaponStatsSo.isStarted;
        
        DamageLevel = 0;
        ReloadSpeedLevel = 0;
        AccuracyLevel = 0;
        BulletCountLevel = 0;

        IsOpen = false;

        WeaponArms = playerWeaponStatsSo.weaponArms;
        WeaponIcon = playerWeaponStatsSo.weaponIcon;

        WeaponGameObject = playerWeaponStatsSo.weaponGameObject;
        weaponType = playerWeaponStatsSo.weaponType;
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

    public void LoadSaves()
    {
        GameSaves gameSaves = SaveGameMechanic.Instance.GetGameSaves();
        SaveWeapon saveWeapon = null;
        foreach (var save in gameSaves.weapons)
        {
            if (save.weaponName == WeaponName)
            {
                saveWeapon = save;
                break;
            }
        }

        if (saveWeapon == null)
        {
            Debug.LogError($"There is no weapon with name {WeaponName} in saves!");
            DamageLevel = 0;
            ReloadSpeedLevel = 0;
            AccuracyLevel = 0;
            BulletCountLevel = 0;
        
            IsOpen = false;
            IsEquiped = false;
            return;
        }

        DamageLevel = saveWeapon.DamageLevel;
        ReloadSpeedLevel = saveWeapon.ReloadSpeedLevel;
        AccuracyLevel = saveWeapon.AccuracyLevel;
        BulletCountLevel = saveWeapon.BulletCountLevel;
        
        IsOpen = saveWeapon.IsOpen;
        IsEquiped = saveWeapon.isEquiped;
        //Debug.Log($"Loading saves {WeaponName} is open = {IsOpen} , is equiped = {IsEquiped}");
    }
}

public class WeaponManagerMechanic : IMechanic
{
    public static WeaponManagerMechanic Instance;

    private PlayerWeaponStatsSo[] _playerWeaponStatsSo;
    private Dictionary<string, PlayerWeaponStats> _playerWeaponStatsDictionary = new Dictionary<string, PlayerWeaponStats>();
    
    private List<PlayerWeaponStats> _playerLoadout;
    private PlayerWeaponStats _currentWeapon;

    public event Action OnPlayerLoadOutChanges;
    
    public void Initialize()
    {
        Instance = this;
        _playerWeaponStatsSo = Resources.LoadAll<PlayerWeaponStatsSo>("ScriptableObjects/PlayerWeapons");
        _playerLoadout = new List<PlayerWeaponStats>();
        SaveGameMechanic.Instance.OnDataRefreshed += RefreshWeaponInfo;
        RefreshWeaponInfo(SaveGameMechanic.Instance.GetGameSaves());
    }
    
    private void RefreshWeaponInfo(GameSaves gameSaves)
    {
        _playerWeaponStatsDictionary = new Dictionary<string, PlayerWeaponStats>();
        for (int i = 0; i < _playerWeaponStatsSo.Length; i++)
        {
            PlayerWeaponStats newPlayerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[i]);
            newPlayerWeapon.LoadSaves();
            newPlayerWeapon.CalculateStats();
            _playerWeaponStatsDictionary.Add(newPlayerWeapon.WeaponName, newPlayerWeapon);
        }

        RefreshLoadOut();
    }

    private void RefreshLoadOut()
    {
        _playerLoadout = new List<PlayerWeaponStats>();
        foreach (var playerWeaponStats in _playerWeaponStatsDictionary.Values)
        {
            if (playerWeaponStats.IsEquiped)
            {
                _playerLoadout.Add(playerWeaponStats);
            }
        }
        OnPlayerLoadOutChanges?.Invoke();
    }
    public void SetWeapon(string weaponName)
    {
        PlayerWeaponStats newPlayerWeaponStats;
        if (!_playerWeaponStatsDictionary.TryGetValue(weaponName, out var value))
        {
            Debug.LogError("There is no weapon in dictionary");
            return;
        }

        newPlayerWeaponStats = value;
        if (_playerLoadout.Contains(newPlayerWeaponStats))
        {
            Debug.LogError("Weapon is in loadout");
            return;
        }
        bool isInCurrent = false;
        foreach (var playerWeaponStats in _playerLoadout)
        {
            if (playerWeaponStats.weaponType == newPlayerWeaponStats.weaponType)
            {
                isInCurrent = true;
            }
        }

        if (isInCurrent)
        {
            for (int i = 0; i < _playerLoadout.Count; i++)
            {
                if (_playerLoadout[i].weaponType == newPlayerWeaponStats.weaponType)
                {
                    _playerLoadout[i].IsEquiped = false;
                    SaveGameMechanic.Instance.SaveWeapon(_playerLoadout[i]);
                    _playerLoadout[i] = newPlayerWeaponStats;
                    _playerLoadout[i].IsEquiped = true;
                }
            }
        }
        else
        {
            newPlayerWeaponStats.IsEquiped = true;
            _playerLoadout.Add(newPlayerWeaponStats);
        }
        
        SaveGameMechanic.Instance.SaveWeapon(newPlayerWeaponStats);
        SaveGameMechanic.Instance.Save();
        OnPlayerLoadOutChanges?.Invoke();;
    }

    public PlayerWeaponStats GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public List<PlayerWeaponStats> GetCurrentWeapons()
    {
        return _playerLoadout;
    }

    public List<PlayerWeaponStats> GetPlayerWeaponStats()
    {
        List<PlayerWeaponStats> list = new List<PlayerWeaponStats>();
        foreach (var playerWeaponStats in _playerWeaponStatsDictionary.Values)
        {
            list.Add(playerWeaponStats);
        }

        return list;
    }

    public void TryToEquip(PlayerWeaponStats currentWepaonStats)
    {
        if (!_playerWeaponStatsDictionary.ContainsKey(currentWepaonStats.WeaponName))
        {
            Debug.LogError($"The weapon - {currentWepaonStats.WeaponName}, is not in game");
            return;
        }

        if (!currentWepaonStats.IsOpen)
        {
            Debug.LogError($"The weapon - {currentWepaonStats.WeaponName}, is not unlocked");
            return;
        }
        SetWeapon(currentWepaonStats.WeaponName);
    }

    public void TryToBuy(PlayerWeaponStats currentWepaonStats)
    {
        if (!_playerWeaponStatsDictionary.TryGetValue(currentWepaonStats.WeaponName, out var value))
        {
            Debug.LogError("There is no weapon in dictionary");
            return;
        }

        currentWepaonStats = value;
        if (currentWepaonStats.IsOpen)
        {
            Debug.LogError($"The gun {currentWepaonStats.WeaponName} is already buyed");
            return;
        }
        
        if (EconomyMonoMechanic.Instance.TryToSpend(currentWepaonStats.WeaponBuyPrice) )
        {
            currentWepaonStats.IsOpen = true;
            SaveGameMechanic.Instance.SaveWeapon(currentWepaonStats);
            SaveGameMechanic.Instance.Save();
            SetWeapon(currentWepaonStats.WeaponName);
        }
    }

    public void PickWeapon(PlayerWeaponStats currentWeaponCurrentWeaponStats)
    {
        _currentWeapon = currentWeaponCurrentWeaponStats;
        
    }
}
