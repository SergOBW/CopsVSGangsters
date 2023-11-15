using Abstract.Inventory;
using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopItemUi : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    
    [SerializeField] private TMP_Text itemNameText; 
    [SerializeField] private TMP_Text itemMoneyPrice;
    [SerializeField] private TMP_Text itemYanPrice;
    
    [SerializeField] private Button buyButton;
    [SerializeField] private Button buyYanButton;

    [SerializeField] private Button openInfoButton;

    private InventoryItem _currentInventoryItem;
    private UiMonoShopState _shopState;

    private bool _isYan;
    public void Initialize(InventoryItem inventoryItem, UiMonoShopState uiMonoShopState, bool isYan = false)
    {
        _currentInventoryItem = inventoryItem;
        _shopState = uiMonoShopState;
        _isYan = isYan;
        
        itemIconImage.sprite = inventoryItem.itemIcon;
        itemNameText.text = inventoryItem.name;
        itemMoneyPrice.text = _isYan ? inventoryItem.yanPrice.ToString() : inventoryItem.price.ToString();
        openInfoButton.onClick.AddListener(ShowInfoPopup);
        if (_isYan)
        {
            buyYanButton.onClick.AddListener(BuyItemForYan);
            buyButton.gameObject.SetActive(false);
            itemYanPrice.text = inventoryItem.yanPrice.ToString();
        }
        else
        {
            buyButton.onClick.AddListener(BuyInventoryItem);
            buyYanButton.gameObject.SetActive(false);
            itemMoneyPrice.text = inventoryItem.price.ToString();
        }
    }

    private void BuyItemForYan()
    {
        AddManager.Instance.TryToBuyItem(_currentInventoryItem.name);
        Debug.Log("Buy yan");
    }

    private void BuyInventoryItem()
    {
        if (EconomyMonoMechanic.Instance.TryToSpend(_currentInventoryItem.price))
        {
            buyButton.onClick.RemoveListener(BuyInventoryItem);
            buyYanButton.onClick.RemoveListener(BuyInventoryItem);
            Inventory.Instance.AddItem(_currentInventoryItem.name);
            _shopState.RefreshUi();
        }
    }

    private void ShowInfoPopup()
    {
        _shopState.ShowInfoPopup(_currentInventoryItem);
    }
}
