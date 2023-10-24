using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoStageState : UiMonoState
    {
        [SerializeField] private Transform levelsGrid;
        [SerializeField] private StageItem stageItemPrefab;

        private List<StageItem> _currentStageItems = new List<StageItem>();

        [SerializeField] private Button backToMainMenuButton;
        

        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SetupStageItems();
            SetupButtons();
        }

        private void SetupButtons()
        {
            backToMainMenuButton.onClick.AddListener(BackToMenu);
        }

        private void SetupStageItems()
        {
            if (_currentStageItems.Count > 0)
            {
                foreach (var stageItem in _currentStageItems)
                {
                    Destroy(stageItem.gameObject);
                }
            }
            
            
            for (int i = 0; i < LevelsMonoMechanic.Instance.GetMapsCount(); i++)
            {
                StageItem stageItem = Instantiate(stageItemPrefab, levelsGrid);
                stageItem.Initialize(i,this);
                _currentStageItems.Add(stageItem);
            }
        }
        
        public void SelectLevel(int levelNumber)
        {
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMonoMechanic.Instance.SelectLevel(levelNumber);
        }

        private void BackToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        public override void ExitState(IState monoState)
        {
            backToMainMenuButton.onClick.RemoveListener(BackToMenu);
            base.ExitState(monoState);
        }
    }
}