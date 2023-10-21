using UnityEngine;
using UnityEngine.Serialization;

namespace Abstract.Inventory
{
    [CreateAssetMenu()]
    public class InventoryItemSo : ScriptableObject
    {
        public Sprite itemIcon;
        [FormerlySerializedAs("name")] public string itemName;
        public int price;
    }
}