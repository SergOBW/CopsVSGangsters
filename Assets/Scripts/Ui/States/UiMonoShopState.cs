using System.Collections.Generic;
using Abstract;
using Abstract.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoShopState : UiMonoState
    {
        [SerializeField]private Button backButton;
        [SerializeField] private RectTransform shopItemsSlot;
        
        [SerializeField] private ShopItemUi shopItemUiPrefab;
        private List<ShopItemUi> _shopItemUis = new List<ShopItemUi>();
        [SerializeField] private RectTransform[] itemsToRebuild;
        [SerializeField] private InfoPopup infoPopup;

        [SerializeField] private Button yanButton;
        [SerializeField] private Button moneyButton;
        

        private bool _isYan;

        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backButton.onClick.AddListener(GoToMenu);
            Inventory.Instance.OnInventoryChanged += RefreshUi;
            _isYan = false;
            RefreshState();
        }

        public override void ExitState(IState monoState)
        {
            backButton.onClick.RemoveListener(GoToMenu);
            Inventory.Instance.OnInventoryChanged -= RefreshUi;
            infoPopup.Close();
            base.ExitState(monoState);
        }

        public void RefreshUi()
        {
            if (_shopItemUis.Count > 0)
            {
                foreach (var shopItemUi in _shopItemUis)
                {
                    Destroy(shopItemUi.gameObject);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(shopItemsSlot);

            _shopItemUis = new List<ShopItemUi>();

            List<InventoryItem> allInventoryItems = Inventory.Instance.GetAllInventoryItems();
            List<InventoryItem> currentInventoryItems = Inventory.Instance.GetCurrentInventoryItems();

            foreach (var inventoryItem in allInventoryItems)
            {
                if (!currentInventoryItems.Contains(inventoryItem))
                {
                    if (inventoryItem.price <= 0)
                    {
                        return;
                    }
                    ShopItemUi shopItemUi = Instantiate(shopItemUiPrefab, shopItemsSlot);
                    shopItemUi.Initialize(inventoryItem,this,_isYan);
                    _shopItemUis.Add(shopItemUi);
                }
            }

            foreach (var rectTransform in itemsToRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }

        private void GoToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        private void RefreshState()
        {
            if (AddManager.Instance.AddAggregator != AddAggregator.YandexGames)
            {
                yanButton.gameObject.SetActive(false);
                moneyButton.interactable = false;
                RefreshUi();
                return;
            }
            if (_isYan)
            {
                yanButton.interactable = false;
                moneyButton.interactable = true;
            }
            else
            {
                yanButton.interactable = true;
                moneyButton.interactable = false;
            }
            RefreshUi();
        }

        public void SetYan()
        {
            _isYan = true;
            RefreshState();
        }
        
        public void SetMoney()
        {
            _isYan = false;
            RefreshState();
        }
        public void ShowInfoPopup(InventoryItem currentInventoryItem)
        {
            infoPopup.Show(currentInventoryItem);
        }
    }
}