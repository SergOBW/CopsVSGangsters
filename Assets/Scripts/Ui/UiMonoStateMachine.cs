using Abstract;
using Ui.States;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UiMonoStateMachine : MonoStateMachine
{
    [FormerlySerializedAs("uiMainMenuState")] public UiMonoMainMenuState uiMonoMainMenuState;
    [FormerlySerializedAs("uiStageState")] public UiMonoStageState uiMonoStageState;
    [FormerlySerializedAs("uiPlayState")] public UiMonoPlayState uiMonoPlayState;
    [FormerlySerializedAs("uiLoadingState")] public UiMonoLoadingState uiMonoLoadingState;
    [FormerlySerializedAs("uiPauseState")] public UiMonoPauseState uiMonoPauseState;
    [FormerlySerializedAs("uiWinState")] public UiMonoWinState uiMonoWinState;
    [FormerlySerializedAs("uiLooseState")] public UiMonoLooseState uiMonoLooseState;

    public static UiMonoStateMachine Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Initialize()
    {
        ChangeState(uiMonoLoadingState);
        
        SceneManager.sceneLoaded += OnFirstLaunchSceneLoaded;
    }

    private void OnFirstLaunchSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 1)
        {
            // This is the hub
            SceneManager.sceneLoaded -= OnFirstLaunchSceneLoaded;
            CurrentMonoState.ExitState(uiMonoMainMenuState);
        }
    }
}
