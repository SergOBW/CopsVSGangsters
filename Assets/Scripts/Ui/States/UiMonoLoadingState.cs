using Abstract;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui.States
{
    public class UiMonoLoadingState : UiMonoState
    {
        [SerializeField] private GameObject loadingText;
        [SerializeField] private GameObject clickText;
        [SerializeField] private ButtonWithCursor clickButton;
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            loadingText.SetActive(true);
            clickText.SetActive(false);
            LevelsMonoMechanic.Instance.OnLevelLoaded += OnLevelLoaded;
            LevelsMonoMechanic.Instance.OnLevelUnLoaded += OnLevelUnloaded;
        }

        private void OnLevelLoaded()
        {
            loadingText.SetActive(false);
            clickText.SetActive(true);
            clickButton.gameObject.SetActive(true);
            clickButton.OnPointerDownEvent += MobileGoToGame;
        }

        public void MobileGoToGame()
        {
            clickText.SetActive(false);
            clickButton.gameObject.SetActive(false);
            LevelStateMachine.Instance.SetLevelPlayState();
            ExitState(currentMonoStateMachine.uiMonoPlayState);
        }

        public void GoToGame(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    //Performed.
                    //clickText.SetActive(false);
                    //LevelStateMachine.Instance.LevelLoaded();
                    //ExitState(currentStateMachine.uiPlayState);
                    break;
            }
        }

        private void OnLevelUnloaded()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        public override void ExitState(IState monoState)
        {
            LevelsMonoMechanic.Instance.OnLevelLoaded -= OnLevelLoaded;
            LevelsMonoMechanic.Instance.OnLevelUnLoaded -= OnLevelUnloaded;
            clickButton.OnPointerDownEvent -= MobileGoToGame;
            base.ExitState(monoState);
        }
    }
}