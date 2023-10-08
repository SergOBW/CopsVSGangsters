﻿using Abstract;
using UnityEngine.SceneManagement;

namespace Level.States
{
    public class LevelMonoLoadingState : LevelMonoState
    {
        private string sceneName = "Hub";
        public override void EnterState(MonoStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public override void UpdateState()
        {
            
        }

        public override void ExitState(IMonoState monoState)
        {
            currentMonoStateMachine.ChangeState(monoState);
        }
    }
}