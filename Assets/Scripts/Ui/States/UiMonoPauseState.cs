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
        

        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            backToGameButton.OnPointerDownEvent += UnPauseButton;
            backToHubButton.onClick.AddListener(BackToHubButton);
            restartLevelButton.onClick.AddListener(RestartLevelButton);
            settingsButton.onClick.AddListener(OpenSettings);
            
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                if (currentMonoStateMachine.PreviousState is UiMonoPlayState)
                {
                    CrazyEvents.Instance.GameplayStop();
                }
            }
        }

        private void UnPauseButton()
        {
            LevelStateMachine.Instance.UnPause();
            ExitState(currentMonoStateMachine.uiMonoPlayState);
        }

        private void BackToHubButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.ExitLevel();
        }

        private void RestartLevelButton()
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.RestartLevel();
        }

        private void OpenSettings()
        {
            currentMonoStateMachine.ShowSettings();
        }

        public override void ExitState(IState monoState)
        {
            backToHubButton.onClick.RemoveListener(BackToHubButton);
            backToGameButton.OnPointerDownEvent -= UnPauseButton;
            restartLevelButton.onClick.RemoveListener(RestartLevelButton);
            settingsButton.onClick.RemoveListener(OpenSettings);
            base.ExitState(monoState);
        }
    }
}