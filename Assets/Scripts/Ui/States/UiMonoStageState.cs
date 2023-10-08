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

        [SerializeField] private Button nextPage;
        [SerializeField] private Button previousPage;

        [SerializeField] private StageNavigation stageNavigation;

        private List<GameObject> levelPages = new List<GameObject>();
        
        private int _currentPage;
        private int _totalPages;

        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SetupPage();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetupButtons();
        }

        private void SetupButtons()
        {
            AddManager.Instance.OnAddClose -= SetupButtons;
            backToMainMenuButton.onClick.AddListener(BackToMenu);
            nextPage.onClick.AddListener(NextPage);
            previousPage.onClick.AddListener(PreviousPage);
        }
        
        private void TryToShowAdd()
        {
            AddManager.Instance.OnAddClose += SetupButtons;
            AddManager.Instance.ShowInterstitialAdd();
        }

        private void SetupPage()
        {
            if (levelPages.Count > 0)
            {
                foreach (var gameObject in levelPages)
                {
                    Destroy(gameObject);
                }
                levelPages = new List<GameObject>();
                _currentStageItems = new List<StageItem>();
            }
            
            int lockStars;
            int levelsPerPage = 15;
            int totalLevelCount = LevelsMechanic.Instance.GetTotalLevelCount();
            
            _totalPages = Mathf.CeilToInt((float)totalLevelCount / levelsPerPage);

            for (int pageIndex = 0; pageIndex < _totalPages; pageIndex++)
            {
                GameObject page = Instantiate(levelsGrid.gameObject,transform);
                page.name = pageIndex.ToString();
                for (int levelIndex = 0; levelIndex < levelsPerPage; levelIndex++)
                {
                    int levelNumber = pageIndex * levelsPerPage + levelIndex;
                    if (levelNumber <= totalLevelCount - 1)
                    {
                        StageItem stageItem =  Instantiate(stageItemPrefab,page.transform);
                        
                        LevelSave levelSave = LevelsMechanic.Instance.GetInfoAboutLevel(levelNumber);
                        lockStars = levelSave.completedStars;
                            
                        stageItem.Initialize(3 - lockStars,lockStars,levelSave.isOpen == 0,levelNumber,this);

                        _currentStageItems.Add(stageItem);
                    }
                }
                levelPages.Add(page);
            }

            foreach (var page in levelPages)
            {
                page.SetActive(false);
            }
            //_currentPage = 0;
            levelPages[_currentPage].SetActive(true);
            stageNavigation.SetPage(_currentPage);
        }

        private void NextPage()
        {
            if (_currentPage + 1 <= levelPages.Count - 1)
            {
                levelPages[_currentPage].SetActive(false);
                _currentPage++;
            }

            else if (_currentPage +1 > levelPages.Count - 1)
            {
                levelPages[_currentPage].SetActive(false);
                _currentPage = 0;
            }
            levelPages[_currentPage].SetActive(true);
            stageNavigation.SetPage(_currentPage);
        }
        
        private void PreviousPage()
        {
            if (_currentPage - 1 < 0)
            {
                levelPages[_currentPage].SetActive(false);
                _currentPage = levelPages.Count - 1;
            } else if (_currentPage - 1 >= 0)
            {
                levelPages[_currentPage].SetActive(false);
                _currentPage--;
            }
            levelPages[_currentPage].SetActive(true);
            stageNavigation.SetPage(_currentPage);
        }

        public void SelectLevel(int levelNumber)
        {
            foreach (var stageItem in _currentStageItems)
            {
                stageItem.SetActiveInteractionFalse();
            }
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.SelectLevel(levelNumber);
        }

        private void BackToMenu()
        {
            ExitState(currentMonoStateMachine.uiMonoMainMenuState);
        }

        public override void ExitState(IMonoState monoState)
        {
            backToMainMenuButton.onClick.RemoveListener(BackToMenu);
            nextPage.onClick.RemoveListener(NextPage);
            previousPage.onClick.RemoveListener(PreviousPage);
            base.ExitState(monoState);
        }
    }
}