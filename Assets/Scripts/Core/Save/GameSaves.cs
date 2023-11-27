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
            bool isDataBetter = false;
            int myOpenedLevels = 0;
            int onotherOpenedLevels = 0;
            foreach (var saveLevel in LevelSaves)
            {
                if (saveLevel.isOpen == 1)
                {
                    myOpenedLevels++;
                }
            }
            foreach (var saveLevel in gameSaves.LevelSaves)
            {
                if (saveLevel.isOpen == 1)
                {
                    onotherOpenedLevels++;
                }
            }
            int myOpenedWeapons = 0;
            int onotherOpenedWeapons = 0;
            foreach (var saveWeapon in weapons)
            {
                if (saveWeapon.IsOpen)
                {
                    myOpenedWeapons++;
                }
            }
            foreach (var saveWeapon in gameSaves.weapons)
            {
                if (saveWeapon.IsOpen)
                {
                    onotherOpenedWeapons++;
                }
            }
            
            bool isLevelsBetter = myOpenedLevels > onotherOpenedLevels;

            int myPoints = myOpenedWeapons + InventoryItems.Count;
            int otherPoints = onotherOpenedWeapons + gameSaves.InventoryItems.Count;

            if (isLevelsBetter)
            {
                isDataBetter = true;
                return isDataBetter;
            }

            if (myOpenedLevels == onotherOpenedLevels)
            {
                if (myPoints > otherPoints)
                {
                    isDataBetter = true;
                }

                if (myPoints == otherPoints)
                {
                    isDataBetter = money > gameSaves.money;
                }
            }

            return isDataBetter;
        }

        public void CreateStartedSaves()
        {
            LevelSaves = new List<SaveLevel>();
            weapons = new List<SaveWeapon>();
            InventoryItems = new List<SaveInventory>();
            money = 0;
            sensitivity = 1f;
            sound = 0.4f;
            
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
                SaveWeapon newSaveWeapon = new SaveWeapon(playerWeaponStatsSos[i].weaponName, playerWeaponStatsSos[i].isStarted,playerWeaponStatsSos[i].isStarted);
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

            if (IsMyDataBetter(deserializeObject))
            {
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