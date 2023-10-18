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
            stageButton.onClick.AddListener(StageButton);
            weaponSetupButton.onClick.AddListener(GoWeaponSetupButton);
            playButton.onClick.AddListener(PlayButton);
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
            LevelsMonoMechanic.Instance.SelectLevel();
        }

        public override void ExitState(IState monoState)
        {
            stageButton.onClick.RemoveListener(StageButton);
            weaponSetupButton.onClick.RemoveListener(GoWeaponSetupButton);
            playButton.onClick.RemoveListener(PlayButton);
            base.ExitState(monoState);
        }
    }
}