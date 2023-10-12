using Abstract;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoPlayState : UiMonoState
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private TutorialUi _tutorialUi;
        [SerializeField] private QuestsUi _questsUi;
        [SerializeField] private InteractionUi _interactionUi;
        [SerializeField] private bl_IndicatorManager _indicatorManager;
        [SerializeField] private bl_HudDamageManager _hudDamageManager;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SubscribeOnEvents();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SetupQuests();
            
            _interactionUi.Initialize();
            _indicatorManager.SetupIndicators();
        }
        
        public override void ExitState(IState monoState)
        {
            UnSubscribeOnEvents();
            _indicatorManager.UnRegister();
            _hudDamageManager.UnRegister();
            base.ExitState(monoState);
        }

        private void SubscribeOnEvents()
        {
            LevelStateMachine.Instance.OnLevelPaused += OnLevelPaused;
            LevelsMonoMechanic.Instance.OnLevelWin += OnLevelWin;
            LevelsMonoMechanic.Instance.OnLevelLoose += OnLevelLoose;
            pauseButton.onClick.AddListener(PauseGame);
        }
        
        private void UnSubscribeOnEvents()
        {
            LevelStateMachine.Instance.OnLevelPaused -= OnLevelPaused;
            LevelsMonoMechanic.Instance.OnLevelWin -= OnLevelWin;
            LevelsMonoMechanic.Instance.OnLevelLoose -= OnLevelLoose;
            pauseButton.onClick.RemoveListener(PauseGame);
        }
        
        private void OnLevelPaused()
        {
            ExitState(currentMonoStateMachine.uiMonoPauseState);
        }

        private void OnLevelLoose()
        {
            ExitState(currentMonoStateMachine.uiMonoLooseState);
        }

        private void OnLevelWin()
        {
            ExitState(currentMonoStateMachine.uiMonoWinState);
        }
        
        private void PauseGame()
        {
            LevelStateMachine.Instance.Pause();
        }
        
        private void SetupQuests()
        {
            Scenario scenario;
            scenario = LevelsMonoMechanic.Instance.GetCurrentScenario();
            if (scenario == null)
            {
                return;
            }
            switch (scenario)
            {
                case WaveDefenseScenario waveDefenseScenario :
                    break;
                case LevelPassScenario levelPassScenario:
                    _questsUi.Initialize();
                    break;
            }
        }
        
    }
}