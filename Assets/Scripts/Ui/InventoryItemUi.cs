using UnityEngine;
using UnityEngine.UI;

namespace Yandex.Plugins.Login.Ui
{
    public class InventoryItemUi : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;

        public void Setup(Sprite sprite)
        {
            itemIcon.sprite = sprite;
        }
    }
}