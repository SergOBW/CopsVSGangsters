using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoMainMenuState : UiMonoState
    {
        [SerializeField] private Button stageButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button weaponSetupButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button settingsButton;
        
        [SerializeField] private List<UiElement> uiElements;
        [SerializeField] private InventoryPopup inventoryPopup;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SetupButtons();
            foreach (var uiElement in uiElements)
            {
                uiElement.Show();
            }
        }
        public override void ExitState(IState monoState)
        {
            stageButton.onClick.RemoveListener(StageButton);
            weaponSetupButton.onClick.RemoveListener(GoWeaponSetupButton);
            playButton.onClick.RemoveListener(PlayButton);
            shopButton.onClick.RemoveListener(ShopButton);
            inventoryButton.onClick.RemoveListener(InventoryButton);
            settingsButton.onClick.RemoveListener(OpenSettings);
            base.ExitState(monoState);
        }
        
        
        private void SetupButtons()
        {
            stageButton.onClick.AddListener(StageButton);
            weaponSetupButton.onClick.AddListener(GoWeaponSetupButton);
            playButton.onClick.AddListener(PlayButton);
            shopButton.onClick.AddListener(ShopButton);
            inventoryButton.onClick.AddListener(InventoryButton);
            settingsButton.onClick.AddListener(OpenSettings);
        }

        private void OpenSettings()
        {
            currentMonoStateMachine.ShowSettings();
        }

        private void InventoryButton()
        {
            inventoryPopup.Show();
        }

        private void ShopButton()
        {
            ExitState(currentMonoStateMachine.uiMonoShopState);
        }

        private void GoWeaponSetupButton()
        {
            ExitState(currentMonoStateMachine.uiMonoWeaponSetupState);
        }
        private void StageButton()
        {
            ExitState(currentMonoStateMachine.uiMonoStageState);
        }

        private void PlayButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.Play();
        }
        
    }
}