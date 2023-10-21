using UnityEngine;

namespace Abstract.Inventory
{
    public class InventoryItem
    {
        public Sprite itemIcon;
        public string name;
        public int price;

        public bool isBought;

        public InventoryItem(InventoryItemSo inventoryItemSo)
        {
            itemIcon = inventoryItemSo.itemIcon;
            name = inventoryItemSo.itemName;
            price = inventoryItemSo.price;
            isBought = false;
        }
    }
}