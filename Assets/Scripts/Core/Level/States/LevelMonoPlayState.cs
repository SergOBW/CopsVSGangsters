using Abstract;
using CrazyGames;

namespace Level.States
{
    public class LevelMonoPlayState : LevelMonoState
    {
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            LevelsMonoMechanic.Instance.OnLevelWin += OnLevelWin;
            LevelsMonoMechanic.Instance.OnLevelLoose += OnLevelLoose;
            SoundMonoMechanic.Instance.SetupVolume();
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazyEvents.Instance.GameplayStart();
            }
        }

        private void OnLevelWin()
        {
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazySDK.Instance.HappyTime();
                CrazySDK.Instance.GameplayStop();
            }
            ExitState(LevelStateMachine.Instance.levelMonoEndState);
        }

        private void OnLevelLoose()
        {
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazySDK.Instance.HappyTime();
                CrazySDK.Instance.GameplayStop();
            }
            LevelStateMachine.Instance.ChangeState(LevelStateMachine.Instance.levelMonoEndState);
            ExitState(LevelStateMachine.Instance.levelMonoEndState);
        }

        public override void ExitState(IState IState)
        {
            LevelsMonoMechanic.Instance.OnLevelWin -= OnLevelWin;
            LevelsMonoMechanic.Instance.OnLevelLoose -= OnLevelLoose;
            base.ExitState(IState);
        }
    }
}