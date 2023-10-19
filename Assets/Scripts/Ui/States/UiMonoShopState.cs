using System.Collections.Generic;
using Abstract;
using Abstract.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoShopState : UiMonoState
    {
        [SerializeField]private Button backButton;
        [SerializeField] private RectTransform shopItemsSlot;
        
        [SerializeField] private ShopItemUi shopItemUiPrefab;
        private List<ShopItemUi> _shopItemUis = new List<ShopItemUi>();

        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backButton.onClick.AddListener(GoToMenu);
            RefreshUi();
        }

        public override void ExitState(IState monoState)
        {
            backButton.onClick.RemoveListener(GoToMenu);
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

            _shopItemUis = new List<ShopItemUi>();

            List<InventoryItem> allInventoryItems = Inventory.Instance.GetAllInventoryItems();
            List<InventoryItem> currentInventoryItems = Inventory.Instance.GetCurrentInventoryItems();

            foreach (var inventoryItem in allInventoryItems)
            {
                if (!currentInventoryItems.Contains(inventoryItem))
                {
                    ShopItemUi shopItemUi = Instantiate(shopItemUiPrefab, shopItemsSlot);
                    shopItemUi.Initialize(inventoryItem,this);
                    _shopItemUis.Add(shopItemUi);
                }
            }
        }

        private void GoToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }
    }
}