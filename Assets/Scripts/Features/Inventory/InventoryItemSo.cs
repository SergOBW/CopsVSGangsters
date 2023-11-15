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
        public int yanPrice;
        public string descriptionRu;
        public string descriptionEn;
        public string descriptionTr;
    }
}