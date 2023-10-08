using Abstract;
using CrazyGames;
using EnemyCore;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.States
{
    public class UiMonoWinState : UiMonoState
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartLevelButton;

        [SerializeField] private Animation checkUpgradeAnimation;
        [SerializeField] private Animation mainAnimation;
        [SerializeField] private GameObject checkUpgradeUi;

        [SerializeField] private GameObject[] headshotImages;
        [SerializeField] private GameObject[] hpImages;

        [SerializeField] private WinningStar[] winningStars;

        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            mainAnimation.Play();
            CheckForUpgrades();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazySDK.Instance.HappyTime();
                CrazySDK.Instance.GameplayStop();
            }
            
            if (EnemyHandleMechanic.Instance.IsHeadBonus())
            {
                foreach (var gameObject in headshotImages)
                {
                    gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var gameObject in headshotImages)
                {
                    gameObject.SetActive(false);
                }
            }

            if (FindObjectOfType<PlayerStatsController>().IsHpBonus())
            {
                foreach (var gameObject in hpImages)
                {
                    gameObject.SetActive(true);
                }
            }else
            {
                foreach (var gameObject in hpImages)
                {
                    gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < winningStars.Length; i++)
            {
                if (i <= LevelsMechanic.Instance.GetStarts() - 1)
                {
                    winningStars[i].SetupStar(true);
                }
                else
                {
                    winningStars[i].SetupStar(false);
                }
            }
        }

        private void SetupButtons()
        {
            mainMenuButton.onClick.AddListener(MainMenu);
            nextLevelButton.onClick.AddListener(NextLevel);
            restartLevelButton.onClick.AddListener(RestartLevel);
            AddManager.Instance.OnAddClose -= SetupButtons;
        }

        private void MainMenu()
        {
            mainMenuButton.onClick.RemoveListener(MainMenu);
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.ExitLevel();
        }

        private void NextLevel()
        {
            nextLevelButton.onClick.RemoveListener(NextLevel);
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.NextLevel();
        }

        private void RestartLevel()
        {
            restartLevelButton.onClick.RemoveListener(RestartLevel);
            ExitState(currentMonoStateMachine.uiMonoLoadingState);
            LevelsMechanic.Instance.RestartLevel();
        }

        public override void ExitState(IMonoState monoState)
        {
            base.ExitState(monoState);
            mainMenuButton.onClick.RemoveListener(MainMenu);
            nextLevelButton.onClick.RemoveListener(NextLevel);
            restartLevelButton.onClick.RemoveListener(RestartLevel);
        }

        private void CheckForUpgrades()
        {
            if (WeaponManagerMechanic.Instance.CanBuyUpgradeOnCurrentWeapon())
            {
                checkUpgradeAnimation.Play("CheckUpgrade");
                checkUpgradeUi.SetActive(true);
            }
            else
            {
                checkUpgradeAnimation.Stop();
                checkUpgradeUi.SetActive(false);
            }
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