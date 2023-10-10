using System;
using System.Collections.Generic;
using Abstract;
using ForWeapon;
using ForWeapon.New;
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

        isStarted = playerWeaponStatsSo.isStarted;
        
        DamageLevel = 0;
        ReloadSpeedLevel = 0;
        AccuracyLevel = 0;
        BulletCountLevel = 0;

        IsOpen = false;

        WeaponArms = playerWeaponStatsSo.weaponArms;
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
        SaveWeapon saveWeapon = SaveGameMechanic.Instance.GetWeaponSave(WeaponName);

        DamageLevel = saveWeapon.DamageLevel;
        ReloadSpeedLevel = saveWeapon.ReloadSpeedLevel;
        AccuracyLevel = saveWeapon.AccuracyLevel;
        BulletCountLevel = saveWeapon.BulletCountLevel;
        
        IsOpen = saveWeapon.IsOpen;
    }
}

public class WeaponManagerMechanic : IMechanic
{
    public static WeaponManagerMechanic Instance;

    private PlayerWeaponStatsSo[] _playerWeaponStatsSo;
    private Dictionary<string, PlayerWeaponStats> _playerWeaponStatsDictionary = new Dictionary<string, PlayerWeaponStats>();
    private PlayerWeaponStats _currentPlayerWeaponStats;
    
    public void Initialize()
    {
        Instance = this;
        _playerWeaponStatsSo = Resources.LoadAll<PlayerWeaponStatsSo>("ScriptableObjects/PlayerWeapons");
        SaveGameMechanic.Instance.OnDataRefreshed += Setup;
        Setup();
    }
    
    private void Setup()
    {
        _playerWeaponStatsDictionary = new Dictionary<string, PlayerWeaponStats>();
        for (int i = 0; i < _playerWeaponStatsSo.Length; i++)
        {
            PlayerWeaponStats newPlayerWeapon = new PlayerWeaponStats(_playerWeaponStatsSo[i]);
            newPlayerWeapon.LoadSaves();
            newPlayerWeapon.CalculateStats();
            _playerWeaponStatsDictionary.Add(newPlayerWeapon.WeaponName, newPlayerWeapon);
            if (newPlayerWeapon.isStarted)
            {
                SetWeapon(newPlayerWeapon.WeaponName);
            }
        }
    }
    public void SetWeapon(string weaponName)
    {
        if (_playerWeaponStatsDictionary.ContainsKey(weaponName))
        {
            _currentPlayerWeaponStats = _playerWeaponStatsDictionary[weaponName];
        }
    }

    public PlayerWeaponStats GetCurrentWeapon()
    {
        return _currentPlayerWeaponStats;
    }
}
