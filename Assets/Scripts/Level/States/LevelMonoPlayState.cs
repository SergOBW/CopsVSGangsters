using Abstract;
using UnityEngine;
using Yandex.Plugins.Login;

namespace Level.States
{
    public class LevelMonoPlayState : LevelMonoState
    {
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            LevelsMechanic.Instance.OnLevelWin += OnLevelEnd;
            GameObject.FindObjectOfType<PlayerCharacter>().SetupSensitivity(SaveManagerMechanic.Instance.GetPlayerSensitivity());
            SoundMechanic.Instance.SetupVolume();
        }

        private void OnLevelEnd()
        {
            LevelsMechanic.Instance.OnLevelWin -= OnLevelEnd;
            ExitState(LevelMonoStateMachine.Instance.levelMonoEndState);
        }
    }
}