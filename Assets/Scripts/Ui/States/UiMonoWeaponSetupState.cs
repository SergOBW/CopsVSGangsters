using Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoWeaponSetupState : UiMonoState
    {
        [SerializeField] private Button backButton;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backButton.onClick.AddListener(BackToMenu);
        }

        private void BackToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }
    }
}