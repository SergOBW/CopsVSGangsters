using System;
using System.Collections.Generic;
using Abstract;
using ForWeapon.New;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
    public class PlayerSaves
    {
        // Levels

        public List<LevelSave> LevelSaves;

        // Player stats
        public string PlayerStatsData;
        public float sensitivity;
        // Economy
        public float money;
        

        public List<SaveWeapon> weapons;
        public float sound;

        public PlayerSaves()
        {
            // Maps

            LevelSaves = new List<LevelSave>();

            // PlayerStats
            PlayerStatsData = "default";
            
            
            // Weapons List
            weapons = new List<SaveWeapon>();
            
            // Economy
            money = 0;
            
            // Sens
            sensitivity = 1f;
        }
        
        public bool IsMyDataBetter(PlayerSaves playerSaves)
        {
            int lastSavedLevel = -1;
            for (int i = 0; i < LevelSaves.Count; i++)
            {
                if (LevelSaves[i].isOpen == 0)
                {
                    lastSavedLevel = i - 1;
                    break;
                }
            }
            
            Debug.Log("My last completed level = " + lastSavedLevel );

            int onotherSavedLevel = -1;
            
            for (int i = 0; i < playerSaves.LevelSaves.Count; i++)
            {
                if (playerSaves.LevelSaves[i].isOpen == 0)
                {
                    onotherSavedLevel = i - 1;
                    break;
                }
            }
            Debug.Log("Another last completed level = " + onotherSavedLevel );
            bool isDataBetter = lastSavedLevel > onotherSavedLevel;
            
            Debug.Log("My  money = " + money );
            Debug.Log("Another money = " + playerSaves.money );
            if (lastSavedLevel == onotherSavedLevel)
            {
                isDataBetter = money > playerSaves.money;
            }
            Debug.Log("IS MY BETTER = " + isDataBetter);

            return isDataBetter;
        }
    }

namespace Yandex.Plugins.Login
{
    public class SaveManagerMechanic : GlobalMechanic
    {
        public static SaveManagerMechanic Instance;
        
        private PlayerSaves _playerSaves;
        private PlayerSaves _netPlayerSaves;
        // Constants
        
        private string startedWeaponName = "MAKAROV";
        private const string ATTACHMENTS_KEY = "Attachments";
        
        private const string DAMAGE_LEVEL_KEY = "DamageLevel";
        private const string RELOAD_SPEED_LEVEL_KEY = "ReloadSpeedLevel";
        private const string ACCURACY_LEVEL_KEY = "AccuracyLevel";
        private const string BULLET_COUNT_LEVEL_KEY = "BulletCountLevel";
        
        // Events
        public event Action OnDataRefreshed;

        private bool d1;
        private bool d2;

        #region UNITY_METHODS
        
        private void Awake()
        {
            Instance = this;
        }
        #endregion
        
        public override void Initialize()
        {
            startedWeaponName = WeaponManagerMechanic.Instance.GetStartedWeaponName();
            LoadLastSave();
        }

        #region LoadData

        public void LoadLastSave()
        {
            _playerSaves = new PlayerSaves();
            
            // Loading levels
            
            LoadLevels();

            // Loading weapons

            LoadWeapons();

            // Player Stats
            _playerSaves.PlayerStatsData = PlayerPrefs.GetString("PlayerStatsData","default");
            _playerSaves.sensitivity = PlayerPrefs.GetFloat("sensitivity",1f);
            _playerSaves.sound = PlayerPrefs.GetFloat("sound",1);
            
            // Economy

            _playerSaves.money = PlayerPrefs.GetFloat("money", 0);
            
            if (AddManager.Instance != null)
            {
                AddManager.Instance.LoadFromExternStorage();
            }
            
            OnDataRefreshed?.Invoke();

        }

        private void LoadLevels()
        {
            List<LevelSave> levelSaves = _playerSaves.LevelSaves;
            for (int i = 0; i < LevelsMechanic.Instance.GetTotalLevelCount(); i++)
            {
                levelSaves.Add(new LevelSave());
            }
            
            if (levelSaves.Count > 0)
            {
                for (int i = 0; i < levelSaves.Count; i++)
                {
                    if (i == 0)
                    {
                        levelSaves[i].isOpen = 1;
                        levelSaves[i].completedStars = PlayerPrefs.GetInt(i + "completedStars",0);
                    }
                    else
                    {
                        levelSaves[i].isOpen = PlayerPrefs.GetInt(i + "isOpen",0);
                        levelSaves[i].completedStars = PlayerPrefs.GetInt(i + "completedStars",0);
                    }
                }
            }
        }

        private void LoadWeapons()
        {
            foreach (var weapon in WeaponManagerMechanic.Instance.GetPlayerWeaponStatsSo())
            {
                SaveWeapon newSaveWeapon = new SaveWeapon(weapon.weaponName, "default", 0);
                _playerSaves.weapons.Add(newSaveWeapon);
            }
            foreach (var saveWeapon in _playerSaves.weapons)
            {
                if (saveWeapon.name == startedWeaponName)
                {
                    saveWeapon.IsOpen = 1;
                    saveWeapon.attachments = PlayerPrefs.GetString(saveWeapon.name + ATTACHMENTS_KEY,"default");
                }
                else
                {
                    saveWeapon.IsOpen = PlayerPrefs.GetInt(saveWeapon.name, 0);
                    saveWeapon.attachments = PlayerPrefs.GetString(saveWeapon.name + ATTACHMENTS_KEY,"default");
                }

                saveWeapon.DamageLevel = PlayerPrefs.GetInt(saveWeapon.name + DAMAGE_LEVEL_KEY, 0);
                saveWeapon.AccuracyLevel = PlayerPrefs.GetInt(saveWeapon.name + ACCURACY_LEVEL_KEY, 0);
                saveWeapon.BulletCountLevel = PlayerPrefs.GetInt(saveWeapon.name + BULLET_COUNT_LEVEL_KEY, 0);
                saveWeapon.ReloadSpeedLevel = PlayerPrefs.GetInt(saveWeapon.name + RELOAD_SPEED_LEVEL_KEY, 0);
            }
        }

        #endregion

        #region SaveData

        public void Save()
        {
            //Debug.Log("Saving");
            // Levels

            SaveLevels();
            
            // Weapons
            SaveWeapons();
            
            //Player stats
            PlayerPrefs.SetString("PlayerStatsData",_playerSaves.PlayerStatsData);
            
            PlayerPrefs.SetFloat("sensitivity", _playerSaves.sensitivity);
            
            PlayerPrefs.SetFloat("sound",_playerSaves.sound);
            
            // Economy
            PlayerPrefs.SetFloat("money", _playerSaves.money);

            if (AddManager.Instance != null)
            {
                AddManager.Instance.SaveDataToExternStorage(_playerSaves);
            }
            
            OnDataRefreshed?.Invoke();
        }

        private void SaveLevels()
        {
            if (_playerSaves.LevelSaves.Count > 0)
            {
                for (int i = 0; i < _playerSaves.LevelSaves.Count; i++)
                {
                    PlayerPrefs.SetInt(i + "isOpen",_playerSaves.LevelSaves[i].isOpen);
                    PlayerPrefs.SetInt(i + "completedStars",_playerSaves.LevelSaves[i].completedStars);
                }
            }
            else
            {
                Debug.Log("Error with saving levels!");
            }
        }

        private void SaveWeapons()
        {
            foreach (var saveWeapon in _playerSaves.weapons)
            {
                PlayerPrefs.SetInt(saveWeapon.name, saveWeapon.IsOpen);
                PlayerPrefs.SetString(saveWeapon.name + ATTACHMENTS_KEY,saveWeapon.attachments);
                PlayerPrefs.SetInt(saveWeapon.name + DAMAGE_LEVEL_KEY, saveWeapon.DamageLevel);
                PlayerPrefs.SetInt(saveWeapon.name + ACCURACY_LEVEL_KEY, saveWeapon.AccuracyLevel);
                PlayerPrefs.SetInt(saveWeapon.name + BULLET_COUNT_LEVEL_KEY, saveWeapon.BulletCountLevel);
                PlayerPrefs.SetInt(saveWeapon.name + RELOAD_SPEED_LEVEL_KEY, saveWeapon.ReloadSpeedLevel);
            }
        }

        #endregion

        private void Update()
        {
            d1 = (Input.GetKey(KeyCode.LeftShift) ? true : false);
            d2 = (Input.GetKey(KeyCode.Z) ? true : false); 
 
            if (d1 && d2)
            {   
                ResetData();
            }
        }

        private void ResetData()
        {
            Debug.Log("Reset data!");
            _playerSaves = new PlayerSaves();
            
            //Levels
            
            List<LevelSave> levelSaves = new List<LevelSave>();
            
            for (int i = 0; i < LevelsMechanic.Instance.GetTotalLevelCount(); i++)
            {
                levelSaves.Add(new LevelSave());
            }
            
            if (levelSaves.Count > 0)
            {
                for (int i = 0; i < levelSaves.Count; i++)
                {
                    if (i == 0)
                    {
                        levelSaves[i].isOpen = 1;
                        levelSaves[i].completedStars = 0;
                    }
                    else
                    {
                        levelSaves[i].isOpen = 0;
                        levelSaves[i].completedStars = 0;
                    }
                }
            }

            _playerSaves.LevelSaves = levelSaves;
            
            // Weapons
            
            foreach (var weapon in WeaponManagerMechanic.Instance.GetPlayerWeaponStatsSo())
            {
                SaveWeapon saveWeapon = new SaveWeapon(weapon.weaponName, "default", weapon.name == startedWeaponName ? 1 : 0);
                
                _playerSaves.weapons.Add(saveWeapon);
            }
            
            // Eco 
            _playerSaves.money = 0;

            //Save();
            Save();
        }
        
        public void SetPlayerInfo(string value)
        {
            PlayerSaves playerSavesLoad = JsonConvert.DeserializeObject<PlayerSaves>(value);
            _netPlayerSaves = playerSavesLoad;
            
            if (_playerSaves.IsMyDataBetter(_netPlayerSaves))
            {
                Debug.Log("I HAVE BETTER SAVES, SO LETS SAVE THIS ");
                Save();
            }
            else
            {
                Debug.Log("My saves is shit, loading yandex");
                _playerSaves = _netPlayerSaves;
                Save();
            }
        }

        public float GetPlayerSensitivity()
        {
            return _playerSaves.sensitivity;
        }
        
        public void SaveSensitivity(float value)
        {
            _playerSaves.sensitivity = value;
        }

        public PlayerSaves GetPlayerSaves()
        {
            return _playerSaves;
        }

        public SaveWeapon GetWeaponSave(string weaponName)
        {
            SaveWeapon saveWeapon = null;
            for (int i = 0; i < _playerSaves.weapons.Count; i++)
            {
                if (_playerSaves.weapons[i].name == weaponName)
                {
                    saveWeapon = _playerSaves.weapons[i];
                }
            }
            return saveWeapon;
        }

        public float GetMoneySave()
        {
            return _playerSaves.money;
        }

        public float GetSoundValue()
        {
            return _playerSaves.sound;
        }

        public void SaveWeapon(PlayerWeaponStats playerWeapon)
        {
            foreach (var saveWeapon in _playerSaves.weapons)
            {
                if (saveWeapon.name == playerWeapon.WeaponName)
                {
                    saveWeapon.IsOpen = playerWeapon.IsOpen;
                    saveWeapon.attachments = "default";
                    saveWeapon.AccuracyLevel = playerWeapon.AccuracyLevel;
                    saveWeapon.DamageLevel = playerWeapon.DamageLevel;
                    saveWeapon.BulletCountLevel = playerWeapon.BulletCountLevel;
                    saveWeapon.ReloadSpeedLevel = playerWeapon.ReloadSpeedLevel;
                    break;
                }
            }

            SaveMoney();
            Save();
        }

        public void SaveLeveSaves(List<LevelSave> levelSaves)
        {
            _playerSaves.LevelSaves = levelSaves;
            SaveMoney();
            Save();
        }

        public void SaveMoney()
        {
            _playerSaves.money = EconomyMechanic.Instance.GetCurrentMoney();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void SaveSound(float volume)
        {
            _playerSaves.sound = volume;
            Save();
        }
    }
}