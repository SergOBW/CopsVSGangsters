using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using DefaultNamespace;
using Tayx.Graphy;
using TMPro;
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
    public UiMonoStageState uiMonoStageState;
    public UiMonoShopState uiMonoShopState;

    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private GraphyManager graphyManager;

    [SerializeField] private GameObject noBigDrillPopup;
    [SerializeField] private GameObject addPopup;
    private TMP_Text addPopupText;

    public static UiMonoStateMachine Instance;

    [SerializeField] private List<StateMachinesDebugger> _stateMachineDebugger;

    public override void Initialize()
    {
        Instance = this;
        addPopupText = addPopup.GetComponentInChildren<TMP_Text>();
        ChangeState(uiMonoLoadingState);
        
        SceneManager.sceneLoaded += OnFirstLaunchSceneLoaded;

        foreach (var stateMachines in _stateMachineDebugger)
        {
            stateMachines.Initialize();
        }

        GlobalSettings.Instance.OnSettingsChanged += OnSettingsChanged;
        OnSettingsChanged();
    }

    private void OnSettingsChanged()
    {
        graphyManager.gameObject.SetActive(GlobalSettings.Instance.isUsingDebug);
    }

    private void OnFirstLaunchSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 1)
        {
            // This is the hub
            SceneManager.sceneLoaded -= OnFirstLaunchSceneLoaded;
            AddManager.Instance.GameReady();
            CurrentState.ExitState(uiMonoMainMenuState);
        }
    }

    public void ShowSettings()
    {
        settingsPopup.Show();
    }

    public void ShowNoBigDrillPopup()
    {
        if (!noBigDrillPopup.activeInHierarchy)
        {
            noBigDrillPopup.SetActive(true);
        }
    }

    public void HideNoBigDrillPopup()
    {
        if (noBigDrillPopup.activeInHierarchy)
        {
            noBigDrillPopup.SetActive(false);
        }
    }
    
    public void ShowAddPopup(float time)
    {
        addPopup.gameObject.SetActive(true);
        string timeString;
        if (time <= 1f)
        {
            timeString = Math.Round(time, 2,MidpointRounding.AwayFromZero).ToString();
        }
        else
        {
            timeString = Math.Ceiling(time).ToString();
        }

        switch (LanguageManager.Instance.GetLanguage())
        {
            case Language.en:
                addPopupText.text = $"Advertising through  {timeString}";
                break;
            case Language.ru: 
                addPopupText.text = $"Реклама через   {timeString}";
                break;
            case Language.tr: 
                addPopupText.text = $"Reklam aracılığıyla   {timeString}";
                break;
            default: 
                addPopupText.text = $"Advertising through  {timeString}";
                break;
        }
    }

    public void DisableAddPopup()
    {
        addPopup.gameObject.SetActive(false);
    }
}
