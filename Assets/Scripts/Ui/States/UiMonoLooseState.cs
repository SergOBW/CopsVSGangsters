using Abstract;
using CrazyGames;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoLooseState : UiMonoState
    {
        [SerializeField] private Button backToHubButton;
        [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button continueRewardedButton;
        [SerializeField] private ButtonWithCursor buttonWithCursor;
        
        [SerializeField] private Animation mainAnimation;
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            mainAnimation.Play();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            continueRewardedButton.gameObject.SetActive(true);
            buttonWithCursor.gameObject.SetActive(false);
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                if (currentMonoStateMachine.PreviousMonoState is UiMonoPlayState)
                {
                    CrazyEvents.Instance.GameplayStop();
                }
            }
        }

        private void SetupButtons()
        {
            AddManager.Instance.OnAddClose -= SetupButtons;
            backToHubButton.onClick.AddListener(BackToHubButton);
            restartLevelButton.onClick.AddListener(RestartLevelButton);
            continueRewardedButton.onClick.AddListener(ContinueRewardedButton);
        }

        public override void ExitState(IMonoState monoState)
        {
            backToHubButton.onClick.RemoveListener(BackToHubButton);
            restartLevelButton.onClick.RemoveListener(RestartLevelButton);
            continueRewardedButton.onClick.RemoveListener(ContinueRewardedButton);
            base.ExitState(monoState);
        }

        private void BackToHubButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.ExitLevel();
        }

        private void RestartLevelButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.RestartLevel();
        }

        #region CuntinueReward

        private void ContinueRewardedButton()
        {
            // Add 
            AddManager.Instance.OnRewardedAddClosed += AddClosed;
            AddManager.Instance.ShowRewardAdd();
        }

        private void AddClosed()
        {
            AddManager.Instance.OnRewardedAddClosed -= AddClosed;
            buttonWithCursor.gameObject.SetActive(true);
            continueRewardedButton.gameObject.SetActive(false);
            buttonWithCursor.OnPointerDownEvent += ReturnToGame;
        }

        #endregion
        

        private void ReturnToGame()
        {
            FindObjectOfType<PlayerStatsController>().ReviveBonus();
            LevelMonoStateMachine.Instance.SetLevelPlayState();
            ExitState(currentMonoStateMachine.uiMonoPlayState);
        }

        private void TryToShowAdd()
        {
            AddManager.Instance.OnAddClose += SetupButtons;
            AddManager.Instance.ShowInterstitialAdd();
        }
        
        public void OnAnimationCompleted()
        {
            if (AddManager.Instance.canShowAdd)
            {
                SoundMechanic.Instance.DisableSound();
                TryToShowAdd();
            }
            else
            {
                SetupButtons();
            }
        }
    }
}