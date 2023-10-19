using System.Collections.Generic;
using Abstract.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Yandex.Plugins.Login.Ui;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Button hideButton;
    [SerializeField] private RectTransform itemsSlot;
    [SerializeField] private InventoryItemUi inventoryItemUiPrefab;

    private List<InventoryItemUi> _itemUis = new List<InventoryItemUi>();

    public void Show()
    {
        gameObject.SetActive(true);
        hideButton.onClick.AddListener(Hide);
        Refresh();
    }

    private void Hide()
    {
        hideButton.onClick.RemoveListener(Hide);
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        if (_itemUis.Count > 0)
        {
            foreach (var inventoryItem in _itemUis)
            {
                Destroy(inventoryItem.gameObject);
            }
        }
        _itemUis = new List<InventoryItemUi>();
        foreach (var inventoryItem in Inventory.Instance.GetCurrentInventoryItems())
        {
            InventoryItemUi inventoryItemUi = Instantiate(inventoryItemUiPrefab, itemsSlot);
            inventoryItemUi.Setup(inventoryItem.itemIcon);
            _itemUis.Add(inventoryItemUi);
        }
    }
}

