using Abstract;
using CrazyGames;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoMainMenuState : UiMonoState
    {
        [SerializeField] private Button gameStageButton;
        [SerializeField] private Button moneyAddButton;
        [SerializeField] private GameObject moneyAddPopup;
        [SerializeField] private WeaponStageUi weaponStageUi;
        [SerializeField] private SettingsPopup _settingsPopup;
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            moneyAddPopup.SetActive(false);
            weaponStageUi.Initialize();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                if (currentMonoStateMachine.PreviousMonoState is UiMonoPlayState)
                {
                    CrazyEvents.Instance.GameplayStop();
                }
            }
            
            SetupButtons();
        }
        
        private void SetupButtons()
        {
            AddManager.Instance.OnAddClose -= SetupButtons;
            gameStageButton.onClick.AddListener(ToStageUi);
            moneyAddButton.onClick.AddListener(ShowMoneyAddPopup);
        }
        
        private void TryToShowAdd()
        {
            AddManager.Instance.OnAddClose += SetupButtons;
            AddManager.Instance.ShowInterstitialAdd();
        }

        private void ToStageUi()
        {
            ExitState(currentMonoStateMachine.uiMonoStageState);
        }

        private void ShowMoneyAddPopup()
        {
            moneyAddPopup.SetActive(true);
            Animation animation = moneyAddPopup.GetComponent<Animation>();
            animation.Play("PopupShowUp");
        }

        public void HideMoneyAddPopup()
        {
            Animation animation = moneyAddPopup.GetComponent<Animation>();
            animation.Play("PopupShowDown");
            moneyAddPopup.SetActive(false);
        }

        public override void ExitState(IMonoState monoState)
        {
            gameStageButton.onClick.RemoveListener(ToStageUi);
            moneyAddButton.onClick.RemoveListener(ShowMoneyAddPopup);
            weaponStageUi.DeInitialize();
            base.ExitState(monoState);
        }
    }
}