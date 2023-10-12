using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoMainMenuState : UiMonoState
    {
        [SerializeField] private Button playGameButton;
        [SerializeField] private Button weaponSetupButton;
        [SerializeField] private List<UiElement> uiElements;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SetupButtons();
            
            foreach (var uiElement in uiElements)
            {
                uiElement.Show();
            }
        }
        
        private void SetupButtons()
        {
            playGameButton.onClick.AddListener(GoPlayButton);
            weaponSetupButton.onClick.AddListener(GoWeaponSetupButton);
        }

        private void GoWeaponSetupButton()
        {
            ExitState(currentMonoStateMachine.uiMonoWeaponSetupState);
        }
        private void GoPlayButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.SelectLevel();
        }

        public override void ExitState(IState monoState)
        {
            playGameButton.onClick.RemoveListener(GoPlayButton);
            weaponSetupButton.onClick.RemoveListener(GoWeaponSetupButton);
            base.ExitState(monoState);
        }
    }
}