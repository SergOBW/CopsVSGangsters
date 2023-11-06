using System;
using System.Collections.Generic;
using Abstract.Inventory;
using ForWeapon;
using ForWeapon.New;
using UnityEngine;

namespace Save 
{ 
    [Serializable]
    public class GameSaves
    {
        // Levels
        public List<SaveLevel> LevelSaves;
        // Weapons
        public List<SaveWeapon> weapons;
        // Inventory
        public List<SaveInventory> InventoryItems;
        // Economy
        public float money;
        // Settings
        public float sound;
        public float sensitivity;
        public bool IsMyDataBetter(GameSaves gameSaves)
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
            
            for (int i = 0; i < gameSaves.LevelSaves.Count; i++)
            {
                if (gameSaves.LevelSaves[i].isOpen == 0)
                {
                    onotherSavedLevel = i - 1;
                    break;
                }
            }
            Debug.Log("Another last completed level = " + onotherSavedLevel );
            bool isDataBetter = lastSavedLevel > onotherSavedLevel;

            return isDataBetter;
        }

        public void CreateStartedSaves()
        {
            LevelSaves = new List<SaveLevel>();
            weapons = new List<SaveWeapon>();
            InventoryItems = new List<SaveInventory>();
            money = 0;
            sensitivity = 1f;
            sound = 1f;
            
            MapsSo[] mapsSos = Resources.LoadAll<MapsSo>("ScriptableObjects/Maps");

            for (int i = 0; i < mapsSos.Length; i++)
            {
                SaveLevel saveLevel = new SaveLevel();
                if (i == 0)
                {
                    saveLevel.isOpen = 1;
                }

                saveLevel.levelName = mapsSos[i].sceneName;
                LevelSaves.Add(saveLevel);
            }

            PlayerWeaponStatsSo[] playerWeaponStatsSos = Resources.LoadAll<PlayerWeaponStatsSo>("ScriptableObjects/PlayerWeapons");
            for (int i = 0; i < playerWeaponStatsSos.Length; i++)
            {
                SaveWeapon newSaveWeapon = new SaveWeapon(playerWeaponStatsSos[i].weaponName, "default",playerWeaponStatsSos[i].isStarted);
                weapons.Add(newSaveWeapon);
            }
        }

        public void LoadNew(GameSaves deserializeObject)
        {
            if (deserializeObject == null)
            {
                Debug.LogError("Loaded data is null!");
                return;
            }
            if (deserializeObject.LevelSaves is { Count: > 0})
            {
                LevelSaves = deserializeObject.LevelSaves;
            }

            if (deserializeObject.weapons is { Count: > 0 })
            {
                weapons = deserializeObject.weapons;
            }

            if (deserializeObject.InventoryItems is { Count: > 0 })
            {
                InventoryItems = deserializeObject.InventoryItems;
            }

            money = deserializeObject.money;
            sensitivity = deserializeObject.sensitivity;
            sound = deserializeObject.sound;
        }
        
    }

    public class SaveInventory
    {
        public string name;
        public bool isBought;

        public void SetupSave(InventoryItem inventoryItem)
        {
            name = inventoryItem.name;
            isBought = inventoryItem.isBought;
        }
    }
    public class SaveLevel
    {
        public string levelName;
        public float lootedMoney;
        public int isOpen;
    }
}