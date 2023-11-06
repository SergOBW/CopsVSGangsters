using Abstract;
using ForWeapon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoWeaponSetupState : UiMonoState
    {
        [SerializeField] private Button backButton;
        [SerializeField]private GameObject weaponCamera;

        [FormerlySerializedAs("weaponUi")] [SerializeField] private ChooseWeaponUi chooseWeaponUi;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backButton.onClick.AddListener(BackToMenu);
            if (weaponCamera != null)
            {
                weaponCamera.SetActive(true);
            }
            chooseWeaponUi.Initialize();
        }

        private void BackToMenu()
        {
            chooseWeaponUi.DeInitialize();
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        public override void ExitState(IState monoState)
        {
            backButton.onClick.RemoveListener(BackToMenu);
            if (weaponCamera != null)
            {
                weaponCamera.SetActive(false);
            }
            base.ExitState(monoState);
        }
    }
}