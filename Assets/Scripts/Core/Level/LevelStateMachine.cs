using System;
using Abstract;
using Level.States;
using UnityEngine;

public class LevelStateMachine : StateMachine
{
    public static LevelStateMachine Instance;
    
    public LevelMonoNoneState levelMonoNoneState = new LevelMonoNoneState();
    public LevelMonoLoadingState levelMonoLoadingState = new LevelMonoLoadingState();
    public LevelMonoPlayState levelMonoPlayState = new LevelMonoPlayState();
    public LevelMonoPauseState levelMonoPauseState = new LevelMonoPauseState();
    public LevelMonoEndState levelMonoEndState = new LevelMonoEndState();
    public LevelMonoTutorialPauseState levelMonoTutorialPauseState = new LevelMonoTutorialPauseState();
    public event Action OnLevelPaused;
    public event Action OnLevelUnPaused;
    
    public override void Initialize()
    {
        Instance = this;
        ChangeState(levelMonoNoneState);
    }

    public void SetLevelPlayState()
    {
        ChangeState(levelMonoPlayState);
    }
    
    public bool IsPlayState()
    {
        return CurrentState == levelMonoPlayState;
    }
    
    public void Pause()
    {
        if (IsPlayState())
        {
            CurrentState.ExitState(levelMonoPauseState);
            OnLevelPaused?.Invoke();
        }
    }

    public void Tutorial()
    {
        if (IsPlayState())
        {
            CurrentState.ExitState(levelMonoTutorialPauseState);
        }
    }

    public void UnPause()
    {
        if (CurrentState == levelMonoPauseState)
        {
            CurrentState.ExitState(PreviousState);
            OnLevelUnPaused?.Invoke();
        }
        else
        {
            Debug.Log("Error with unpause Game");
        }
    }

    public void LevelUnloaded()
    {
        ChangeState(levelMonoNoneState);
    }
}
