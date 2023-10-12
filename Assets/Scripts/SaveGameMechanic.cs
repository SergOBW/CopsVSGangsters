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
        public event Action OnDataRefreshed;

        private bool d1;
        private bool d2;
        
        public void Initialize()
        {
            Instance = this;
            _gameSaves = new GameSaves();
            _gameSaves.CreateStartedSaves();
        }

        #region LoadData

        public void LoadLastSave()
        {
            _gameSaves = new GameSaves();

            string gameSavesJson = PlayerPrefs.GetString(GAME_SAVES_KEY);
            _gameSaves = JsonConvert.DeserializeObject<GameSaves>(gameSavesJson);
            
            if (AddManager.Instance != null)
            {
                AddManager.Instance.LoadFromExternStorage();
            }
            
            OnDataRefreshed?.Invoke();

        }

        #endregion

        #region SaveData

        public void Save()
        {
            PlayerPrefs.SetString(GAME_SAVES_KEY,JsonConvert.SerializeObject(_gameSaves));

            if (AddManager.Instance != null)
            {
                AddManager.Instance.SaveDataToExternStorage(_gameSaves);
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
            _gameSaves = new GameSaves();
            
            //Save();
            Save();
        }
        
        public void SetPlayerInfo(string value)
        {
            GameSaves gameSavesLoad = JsonConvert.DeserializeObject<GameSaves>(value);
            _netGameSaves = gameSavesLoad;
            
            if (_gameSaves.IsMyDataBetter(_netGameSaves))
            {
                Debug.Log("I HAVE BETTER SAVES, SO LETS SAVE THIS ");
                Save();
            }
            else
            {
                Debug.Log("My saves is shit, loading yandex");
                _gameSaves = _netGameSaves;
                Save();
            }
        }

        public float GetPlayerSensitivity()
        {
            return _gameSaves.sensitivity;
        }
        
        public void SaveSensitivity(float value)
        {
            _gameSaves.sensitivity = value;
        }

        public GameSaves GetGameSaves()
        {
            return _gameSaves;
        }

        public SaveWeapon GetWeaponSave(string weaponName)
        {
            SaveWeapon saveWeapon = null;
            for (int i = 0; i < _gameSaves.weapons.Count; i++)
            {
                if (_gameSaves.weapons[i].name == weaponName)
                {
                    saveWeapon = _gameSaves.weapons[i];
                }
            }
            return saveWeapon;
        }

        public float GetMoneySave()
        {
            return _gameSaves.money;
        }

        public float GetSoundValue()
        {
            return _gameSaves.sound;
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

            SaveMoney();
            Save();
        }

        public void SaveLeveSaves(List<LevelSave> levelSaves)
        {
            _gameSaves.LevelSaves = levelSaves;
            SaveMoney();
            Save();
        }

        public void SaveMoney()
        {
            _gameSaves.money = EconomyMonoMechanic.Instance.GetCurrentMoney();
        }

        public void SaveSound(float volume)
        {
            _gameSaves.sound = volume;
            Save();
        }
    }