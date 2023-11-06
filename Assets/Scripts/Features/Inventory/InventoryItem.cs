﻿using UnityEngine;

namespace Abstract.Inventory
{
    public class InventoryItem
    {
        public Sprite itemIcon;
        public string name;
        public int price;

        public bool isBought;
        
        public string descriptionRu;
        public string descriptionEn;
        public string descriptionTr;

        public InventoryItem(InventoryItemSo inventoryItemSo)
        {
            itemIcon = inventoryItemSo.itemIcon;
            name = inventoryItemSo.itemName;
            price = inventoryItemSo.price;

            descriptionEn = inventoryItemSo.descriptionEn;
            descriptionRu = inventoryItemSo.descriptionRu;
            descriptionTr = inventoryItemSo.descriptionTr;
            
            isBought = false;
        }
    }
}