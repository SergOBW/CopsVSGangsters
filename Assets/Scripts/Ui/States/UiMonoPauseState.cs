using Abstract;
using CrazyGames;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoPauseState : UiMonoState
    {
        [SerializeField] private ButtonWithCursor backToGameButton;
        [SerializeField] private Button backToHubButton;
        [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button settingsButton;

        [SerializeField] private SettingsPopup settingsPopup;

        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backToGameButton.OnPointerDownEvent += UnPauseButton;
            backToHubButton.onClick.AddListener(BackToHubButton);
            restartLevelButton.onClick.AddListener(RestartLevelButton);
            settingsButton.onClick.AddListener(OpenSettings);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                if (currentMonoStateMachine.PreviousMonoState is UiMonoPlayState)
                {
                    CrazyEvents.Instance.GameplayStop();
                }
            }
        }

        private void UnPauseButton()
        {
            LevelMonoStateMachine.Instance.UnPause();
            ExitState(currentMonoStateMachine.uiMonoPlayState);
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

        private void OpenSettings()
        {
            settingsPopup.gameObject.SetActive(true);
            settingsPopup.Show();
        }

        public override void ExitState(IMonoState monoState)
        {
            backToHubButton.onClick.RemoveListener(BackToHubButton);
            backToGameButton.OnPointerDownEvent -= UnPauseButton;
            restartLevelButton.onClick.RemoveListener(RestartLevelButton);
            settingsButton.onClick.RemoveListener(OpenSettings);
            base.ExitState(monoState);
        }
    }
}