using Abstract;
using CrazyGames;

namespace Level.States
{
    public class LevelMonoPlayState : LevelMonoState
    {
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            LevelsMonoMechanic.Instance.OnLevelWin += OnLevelEnd;
            SoundMonoMechanic.Instance.SetupVolume();
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazyEvents.Instance.GameplayStart();
            }
        }

        private void OnLevelEnd()
        {
            LevelsMonoMechanic.Instance.OnLevelWin -= OnLevelEnd;
            if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
            {
                CrazyEvents.Instance.GameplayStop();
            }
            ExitState(LevelStateMachine.Instance.levelMonoEndState);
        }
    }
}