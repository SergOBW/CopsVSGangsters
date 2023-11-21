using System;
using System.Collections.Generic;
using Save;
using UnityEngine;

namespace Abstract.Inventory
{
    public class Inventory : IMechanic
    {
        private List<InventoryItem> currentInventoryItems;
        private List<InventoryItem> allInventoryItems;

        public event Action OnInventoryChanged;

        public static Inventory Instance;
        
        public void Initialize()
        {
            Instance = this;
            SaveGameMechanic.Instance.OnDataRefreshed += Refresh;
            Refresh(SaveGameMechanic.Instance.GetGameSaves());
        }

        private void Refresh(GameSaves gameSaves)
        {
            currentInventoryItems = new List<InventoryItem>();
            allInventoryItems = new List<InventoryItem>();
            InventoryItemSo[] inventoryItemSos = Resources.LoadAll<InventoryItemSo>("ScriptableObjects/InventoryItems");
            
            foreach (var inventoryItemSo in inventoryItemSos)
            {
                allInventoryItems.Add(new InventoryItem(inventoryItemSo));
            }
            allInventoryItems.Add(new InventoryItem("Money Pack"));
            List<SaveInventory> saveInventory = gameSaves.InventoryItems;
            if (saveInventory.Count > 0)
            {
                foreach (var inventoryItem in allInventoryItems)
                {
                    foreach (var saveInventoryItem in saveInventory)
                    {
                        if (inventoryItem.name == saveInventoryItem.name )
                        {
                            inventoryItem.isBought = saveInventoryItem.isBought;
                            currentInventoryItems.Add(inventoryItem);
                        }
                    }
                }
            }
            
            OnInventoryChanged?.Invoke();
        }

        public List<InventoryItem> GetAllInventoryItems()
        {
            return allInventoryItems;
        }

        public List<InventoryItem> GetCurrentInventoryItems()
        {
            return currentInventoryItems;
        }

        public bool HasItem(string itemName)
        {
            foreach (var inventoryItem in currentInventoryItems)
            {
                Debug.Log($"Has item {itemName} : {inventoryItem.name}");
                if (inventoryItem.name == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddItem(string name)
        {
            if (HasItem(name))
            {
                Save();
                return;
            }
            foreach (var inventoryItem in allInventoryItems)
            {
                if (inventoryItem.name == name)
                {
                    Debug.Log("Adding item " + inventoryItem.name);
                    inventoryItem.isBought = true;
                    currentInventoryItems.Add(inventoryItem);
                    Debug.Log($"Current invetnory items = {currentInventoryItems.Count}");
                    break;
                }
            }
            Save();
            if (name == "Money Pack")
            {
                EconomyMonoMechanic.Instance.AddMoney(1000000);
            }
        }

        private void Save()
        {
            Debug.Log("Saving");
            List<SaveInventory> list = new List<SaveInventory>();
            foreach (var inventoryItem in currentInventoryItems)
            {
                SaveInventory saveInventory = new SaveInventory();
                saveInventory.SetupSave(inventoryItem);
                list.Add(saveInventory);
            }
            SaveGameMechanic.Instance.SaveInventory(list);
            OnInventoryChanged?.Invoke();
        }
    }
}