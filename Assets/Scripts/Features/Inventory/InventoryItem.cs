using UnityEngine;

namespace Abstract.Inventory
{
    public class InventoryItem
    {
        public Sprite itemIcon;
        public string name;
        public int price;
        public int yanPrice;

        public bool isBought;
        
        public string descriptionRu;
        public string descriptionEn;
        public string descriptionTr;
        public string nameRu;
        public string nameTr;

        public InventoryItem(InventoryItemSo inventoryItemSo)
        {
            itemIcon = inventoryItemSo.itemIcon;
            name = inventoryItemSo.itemName;
            nameRu = inventoryItemSo.nameRu;
            nameTr = inventoryItemSo.nameTr;
            price = inventoryItemSo.price;
            yanPrice = inventoryItemSo.yanPrice;

            descriptionEn = inventoryItemSo.descriptionEn;
            descriptionRu = inventoryItemSo.descriptionRu;
            descriptionTr = inventoryItemSo.descriptionTr;
            
            isBought = false;
        }

        public InventoryItem(string name)
        {
            this.name = name;
        }
    }
}