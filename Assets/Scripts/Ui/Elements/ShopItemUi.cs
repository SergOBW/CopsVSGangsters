using Abstract.Inventory;
using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUi : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemPriceText;
    
    [SerializeField] private Button buyButton;

    [SerializeField] private Button openInfoButton;

    private InventoryItem _currentInventoryItem;
    private UiMonoShopState _shopState;
    public void Initialize(InventoryItem inventoryItem, UiMonoShopState uiMonoShopState)
    {
        _currentInventoryItem = inventoryItem;
        _shopState = uiMonoShopState;
        
        itemIconImage.sprite = inventoryItem.itemIcon;
        itemNameText.text = inventoryItem.name;
        itemPriceText.text = inventoryItem.price.ToString();
        buyButton.onClick.AddListener(BuyInventoryItem);
        openInfoButton.onClick.AddListener(ShowInfoPopup);
    }

    private void BuyInventoryItem()
    {
        if (EconomyMonoMechanic.Instance.TryToSpend(_currentInventoryItem.price))
        {
            buyButton.onClick.RemoveListener(BuyInventoryItem);
            Inventory.Instance.AddItem(_currentInventoryItem.name);
            _shopState.RefreshUi();
        }
    }

    private void ShowInfoPopup()
    {
        _shopState.ShowInfoPopup(_currentInventoryItem);
    }
}
