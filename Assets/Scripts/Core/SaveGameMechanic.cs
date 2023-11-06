using System;
using System.Collections.Generic;
using Abstract;
using ForWeapon.New;
using Newtonsoft.Json;
using Save;
using UnityEngine;
 public class SaveGameMechanic : IMechanic
    {
        public static SaveGameMechanic Instance;
        
        private GameSaves _gameSaves;
        private GameSaves _netGameSaves;
        // Constants
        private const string GAME_SAVES_KEY = "GameSaves";
        // Events
        public event Action<GameSaves> OnDataRefreshed;
        
        public void Initialize()
        {
            Instance = this;
            LoadLastSave();
        }

        public void LoadLastSave()
        {
            _gameSaves = new GameSaves();
            _gameSaves.CreateStartedSaves();
            
            string gameSavesJson = PlayerPrefs.GetString(GAME_SAVES_KEY);

            _gameSaves.LoadNew(JsonConvert.DeserializeObject<GameSaves>(gameSavesJson));
            
            if (AddManager.Instance != null)
            {
                AddManager.Instance.LoadFromExternStorage();
            }
            
            OnDataRefreshed?.Invoke(_gameSaves);

        }

        public void Save()
        {
            string savesJson = JsonConvert.SerializeObject(_gameSaves);
            PlayerPrefs.SetString(GAME_SAVES_KEY,savesJson);

            if (AddManager.Instance != null)
            {
                AddManager.Instance.SaveDataToExternStorage(_gameSaves);
            }
            
            OnDataRefreshed?.Invoke(_gameSaves);
        }
        
        public void ResetData()
        {
            Debug.Log("Reset data!");
            _gameSaves = new GameSaves();
            _gameSaves.CreateStartedSaves();
            Save();
            OnDataRefreshed?.Invoke(_gameSaves);
        }
        
        public void SetPlayerInfo(string value)
        {
            GameSaves gameSavesLoad = JsonConvert.DeserializeObject<GameSaves>(value);
            _netGameSaves = gameSavesLoad;
            
            _gameSaves.LoadNew(_netGameSaves);
            Save();
        }
        public GameSaves GetGameSaves()
        {
            return _gameSaves;
        }
        
        public void SaveWeapon(PlayerWeaponStats playerWeapon)
        {
            foreach (var saveWeapon in _gameSaves.weapons)
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
            
            Save();
        }

        public void SaveLeveSaves(List<SaveLevel> levelSaves)
        {
            _gameSaves.LevelSaves = levelSaves;
            Save();
        }

        public void SaveMoney()
        {
            _gameSaves.money = EconomyMonoMechanic.Instance.GetCurrentMoney();
            Save();
        }

        public void SaveSound(float volume)
        {
            _gameSaves.sound = volume;
            Save();
        }

        public void SaveInventory(List<SaveInventory> currentInventoryItems)
        {
            _gameSaves.InventoryItems = currentInventoryItems;
            Debug.Log(_gameSaves.InventoryItems);
            Save();
        }
    }