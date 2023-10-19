﻿using System;
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

        public List<LevelSave> LevelSaves;
        
        public float sensitivity;
        // Economy
        public float money;
        

        public List<SaveWeapon> weapons;
        public float sound;
        
        public List<SaveInventory> InventoryItems;
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
            
            Debug.Log("My  money = " + money );
            Debug.Log("Another money = " + gameSaves.money );
            if (lastSavedLevel == onotherSavedLevel)
            {
                isDataBetter = money > gameSaves.money;
            }
            Debug.Log("IS MY BETTER = " + isDataBetter);

            return isDataBetter;
        }

        public void CreateStartedSaves()
        {
            LevelSaves = new List<LevelSave>();
            weapons = new List<SaveWeapon>();
            InventoryItems = new List<SaveInventory>();
            money = 0;
            sensitivity = 1f;
            sound = 1f;

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
    
    [Serializable]
    public class LevelSave
    {
        public int completedStars;
        public int isOpen;
    }
}