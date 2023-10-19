using System.Collections.Generic;
using Save;
using UnityEngine;

namespace Abstract.Inventory
{
    public class Inventory : IMechanic
    {
        private List<InventoryItem> currentInventoryItems;
        private List<InventoryItem> allInventoryItems;

        public static Inventory Instance;
        
        public void Initialize()
        {
            Instance = this;
            currentInventoryItems = new List<InventoryItem>();
            allInventoryItems = new List<InventoryItem>();
            InventoryItemSo[] inventoryItemSos = Resources.LoadAll<InventoryItemSo>("ScriptableObjects/InventoryItems");
            
            foreach (var inventoryItemSo in inventoryItemSos)
            {
                allInventoryItems.Add(new InventoryItem(inventoryItemSo));
            }
            
            
            List<SaveInventory> saveInventory = SaveGameMechanic.Instance.GetGameSaves().InventoryItems;
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
                if (inventoryItem.name == itemName)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddItem(string name)
        {
            foreach (var inventoryItem in allInventoryItems)
            {
                if (inventoryItem.name == name)
                {
                    currentInventoryItems.Add(inventoryItem);
                    break;
                }
            }
            Debug.Log(currentInventoryItems.Count);
            List<SaveInventory> list = new List<SaveInventory>();
            foreach (var inventoryItem in currentInventoryItems)
            {
                SaveInventory saveInventory = new SaveInventory();
                saveInventory.SetupSave(inventoryItem);
                list.Add(saveInventory);
            }
            SaveGameMechanic.Instance.SaveInventory(list);
        }
    }
}