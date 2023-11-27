using System.Collections;
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
            SoundMonoMechanic.Instance.DisableSound();
            if (AddManager.Instance.canShowAdd)
            {
                StartCoroutine(ShowAddWithDelay());
            }
            else
            {
                SetupButtons();
            }
        }
        
        private IEnumerator ShowAddWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            TryToShowAdd();
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
        }

        private void SetupButtons()
        {
            claimButton.onClick.AddListener(MainMenu);
            claimDoubleButton.onClick.AddListener(DoubleDoubleButton);
            moneySlider.maxValue = EconomyMonoMechanic.Instance.GetCurrentMoneyToWin();
            moneySlider.value = EconomyMonoMechanic.Instance.GetCurrentTempMoney();
            moneyText.text = ((int)EconomyMonoMechanic.Instance.GetCurrentTempMoney()).ToString();
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
            moneySlider.maxValue = EconomyMonoMechanic.Instance.GetCurrentMoneyToWin();
            moneySlider.value = EconomyMonoMechanic.Instance.GetCurrentTempMoney();
            moneyText.text = ((int)EconomyMonoMechanic.Instance.GetCurrentTempMoney()).ToString();
        }

        private void MainMenu()
        {
            EconomyMonoMechanic.Instance.CalculateMoney();
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.ExitLevel();
        }
        
    }
}