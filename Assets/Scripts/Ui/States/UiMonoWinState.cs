using Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoWinState : UiMonoState
    {
        [SerializeField] private Button claimButton;
        [SerializeField] private Button claimDoubleButton;

        [SerializeField] private Slider moneySlider;
        [SerializeField] private TMP_Text moneyText;
         
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            if (AddManager.Instance.canShowAdd)
            {
                TryToShowAdd();
            }
            else
            {
                SetupButtons();
            }
        }
        public override void ExitState(IState monoState)
        {
            base.ExitState(monoState);
            claimButton.onClick.RemoveListener(MainMenu);
            claimDoubleButton.onClick.RemoveListener(DoubleDoubleButton);
            AddManager.Instance.OnRewarded -= OnRewarded;
            AddManager.Instance.OnAddClose -= MainMenu;
            AddManager.Instance.OnAddClose -= SetupButtons;
        }
        
        private void TryToShowAdd()
        {
            AddManager.Instance.OnAddClose += SetupButtons;
            AddManager.Instance.ShowInterstitialAdd();
            moneySlider.maxValue = EconomyMonoMechanic.Instance.GetCurrentMoneyToWin();
            moneySlider.value = EconomyMonoMechanic.Instance.GetCurrentTempMoney();
            moneyText.text = ((int)EconomyMonoMechanic.Instance.GetCurrentTempMoney()).ToString();
        }

        private void SetupButtons()
        {
            claimButton.onClick.AddListener(MainMenu);
            claimDoubleButton.onClick.AddListener(DoubleDoubleButton);
        }

        private void DoubleDoubleButton()
        {
            AddManager.Instance.OnRewarded += OnRewarded;
            AddManager.Instance.OnAddClose += MainMenu;
            AddManager.Instance.ShowRewardAdd();
        }
        
        private void OnRewarded()
        {
            EconomyMonoMechanic.Instance.DoDoubleBonus();
        }

        private void MainMenu()
        {
            EconomyMonoMechanic.Instance.CalculateMoney();
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.ExitLevel();
        }
        
    }
}