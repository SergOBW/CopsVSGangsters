using System;
using Abstract;
using Level.States;
using UnityEngine;

public class LevelMonoStateMachine : MonoStateMachine
{
    public static LevelMonoStateMachine Instance;
    
    public LevelMonoNoneState levelMonoNoneState = new LevelMonoNoneState();
    public LevelMonoLoadingState levelMonoLoadingState = new LevelMonoLoadingState();
    public LevelMonoPlayState levelMonoPlayState = new LevelMonoPlayState();
    public LevelMonoPauseState levelMonoPauseState = new LevelMonoPauseState();
    public LevelMonoEndState levelMonoEndState = new LevelMonoEndState();
    public LevelMonoTutorialPauseState levelMonoTutorialPauseState = new LevelMonoTutorialPauseState();
    

    public event Action OnLevelPaused;
    public event Action OnLevelUnPaused;
    
    private void Awake()
    {
        Instance = this;
        ChangeState(levelMonoNoneState);
    }

    public override void Initialize()
    {
        ChangeState(levelMonoLoadingState);
    }

    public void SetLevelPlayState()
    {
        ChangeState(levelMonoPlayState);
    }
    
    public bool IsPlayState()
    {
        return CurrentMonoState == levelMonoPlayState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && IsPlayState())
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (IsPlayState())
        {
            CurrentMonoState.ExitState(levelMonoPauseState);
            OnLevelPaused?.Invoke();
        }
    }

    public void Tutorial()
    {
        if (IsPlayState())
        {
            CurrentMonoState.ExitState(levelMonoTutorialPauseState);
        }
    }

    public void UnPause()
    {
        if (CurrentMonoState == levelMonoPauseState)
        {
            CurrentMonoState.ExitState(PreviousMonoState);
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
