using Abstract;
using ForWeapon;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoWeaponSetupState : UiMonoState
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Camera weaponCamera;

        [SerializeField] private WeaponUi weaponUi;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backButton.onClick.AddListener(BackToMenu);
            weaponCamera.gameObject.SetActive(true);
            weaponUi.Initialize();
        }

        private void BackToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        public override void ExitState(IState monoState)
        {
            weaponCamera.gameObject.SetActive(false);
            base.ExitState(monoState);
        }
    }
}