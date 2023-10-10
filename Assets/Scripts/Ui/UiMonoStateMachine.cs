using System;
using System.Collections.Generic;
using Abstract;
using Ui.States;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMonoStateMachine : MonoStateMachine
{
    public UiMonoMainMenuState uiMonoMainMenuState;
    public UiMonoWeaponSetupState uiMonoWeaponSetupState;
    public UiMonoPlayState uiMonoPlayState;
    public UiMonoLoadingState uiMonoLoadingState;
    public UiMonoPauseState uiMonoPauseState;
    public UiMonoWinState uiMonoWinState;
    public UiMonoLooseState uiMonoLooseState;

    public static UiMonoStateMachine Instance;

    [SerializeField] private List<StateMachinesDebugger> _stateMachineDebugger;

    public override void Initialize()
    {
        Instance = this;
        ChangeState(uiMonoLoadingState);
        
        SceneManager.sceneLoaded += OnFirstLaunchSceneLoaded;

        foreach (var stateMachines in _stateMachineDebugger)
        {
            stateMachines.Initialize();
        }
    }

    private void OnFirstLaunchSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 1)
        {
            // This is the hub
            SceneManager.sceneLoaded -= OnFirstLaunchSceneLoaded;
            CurrentState.ExitState(uiMonoMainMenuState);
        }
    }

    public void InitializeStandalone()
    {
        ChangeState(uiMonoPlayState);
    }
}
