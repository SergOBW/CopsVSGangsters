using Abstract;
using CrazyGames;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoPlayState : UiMonoState
    {
        private Crosshair crosshair;
        [SerializeField] private Button pauseButton;
        [SerializeField] private TutorialUi _tutorialUi;
        [SerializeField] private QuestsUi _questsUi;
        [SerializeField] private InteractionUi _interactionUi;
        [SerializeField] private bl_IndicatorManager _indicatorManager;
        [SerializeField] private bl_HudDamageManager _hudDamageManager;
        private bool isTutorual;
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SubscribeOnEvents();
            crosshair = GetComponentInChildren<Crosshair>();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _interactionUi.Initialize();
            
            transform.rotation = quaternion.identity;
            
            /*
            
            if (LevelsMechanic.Instance.GetLastSavedLevel() == 0 && !isTutorual)
            {
                _tutorialUi.StartTutorial(this);
                isTutorual = true;
                return;
            }
            */

            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazyEvents.Instance.GameplayStart();
            }
            //FindObjectOfType<StarterAssetsInputs>().Initialize();
            switch (LevelsMechanic.Instance.GetCurrentScenario())
            {
                case WaveDefenseScenario waveDefenseScenario :
                    //_questsUi.gameObject.SetActive(false);
                    break;
                case LevelPassScenario levelPassScenario:
                    //_questsUi.gameObject.SetActive(true);
                    _questsUi.Initialize();
                    break;
            }
            _indicatorManager.SetupIndicators();
        }
        
        private void SubscribeOnEvents()
        {
            LevelMonoStateMachine.Instance.OnLevelPaused += OnLevelPaused;
            //FindObjectOfType<Character>().OnFire += OnFire;
            LevelsMechanic.Instance.OnLevelWin += OnLevelWin;
            LevelsMechanic.Instance.OnLevelLoose += OnLevelLoose;
            pauseButton.onClick.AddListener(PauseGame);
        }
        
        private void UnSubscribeOnEvents()
        {
            LevelMonoStateMachine.Instance.OnLevelPaused -= OnLevelPaused;
            //FindObjectOfType<Character>().OnFire -= OnFire;
            LevelsMechanic.Instance.OnLevelWin -= OnLevelWin;
            LevelsMechanic.Instance.OnLevelLoose -= OnLevelLoose;
            pauseButton.onClick.RemoveListener(PauseGame);
        }
        
        private void OnFire()
        {
            crosshair.Shot();
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
            LevelMonoStateMachine.Instance.Pause();
        }
        
        public override void ExitState(IMonoState monoState)
        {
            UnSubscribeOnEvents();
            _indicatorManager.UnRegister();
            _hudDamageManager.UnRegister();
            base.ExitState(monoState);
        }

        public void ShowQuests()
        {
            _questsUi.Initialize();
        }
    }
}