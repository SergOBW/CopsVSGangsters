﻿using System.Collections;
using Abstract;
using Core;
using CrazyGames;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoLooseState : UiMonoState
    {
        [SerializeField] private Button backToHubButton;
        [SerializeField] private Button continueRewardedButton;
        [SerializeField] private ButtonWithCursor buttonWithCursor;

        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            continueRewardedButton.gameObject.SetActive(true);
            buttonWithCursor.gameObject.SetActive(false);
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

        private void SetupButtons()
        {
            AddManager.Instance.OnAddClose -= SetupButtons;
            backToHubButton.onClick.AddListener(BackToHubButton);
            continueRewardedButton.onClick.AddListener(ContinueRewardedButton);
        }

        public override void ExitState(IState monoState)
        {
            backToHubButton.onClick.RemoveListener(BackToHubButton);
            continueRewardedButton.onClick.RemoveListener(ContinueRewardedButton);
            base.ExitState(monoState);
        }

        private void BackToHubButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.ExitLevel();
        }

        #region CuntinueReward

        private void ContinueRewardedButton()
        {
            // Add 
            AddManager.Instance.OnRewarded += AddClosed;
            AddManager.Instance.ShowRewardAdd();
        }

        private void AddClosed()
        {
            AddManager.Instance.OnRewarded -= AddClosed;
            buttonWithCursor.gameObject.SetActive(true);
            continueRewardedButton.gameObject.SetActive(false);
            buttonWithCursor.OnPointerDownEvent += ReturnToGame;
        }

        #endregion
        

        private void ReturnToGame()
        {
            PlayerMonoMechanic.Instance.ReviveBonus();
            LevelStateMachine.Instance.SetLevelPlayState();
            ExitState(currentMonoStateMachine.uiMonoPlayState);
        }

        private void TryToShowAdd()
        {
            AddManager.Instance.OnAddClose += SetupButtons;
            AddManager.Instance.ShowInterstitialAdd();
        }
    }
}