using UnityEngine;

namespace Abstract.Inventory
{
    [CreateAssetMenu()]
    public class InventoryItemSo : ScriptableObject
    {
        public Sprite itemIcon;
        public string name;
        public int price;
    }
}