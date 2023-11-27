using System;
using Abstract;
using Abstract.Inventory;
using CrazyGames;
using Save;
using UnityEngine;
using UnityEngine.Serialization;

public enum AddAggregator
{
    None = 0,
    CrazyGames = 1,
    YandexGames = 2,
    UnityAds = 3
}
public class AddManager : GlobalMonoMechanic
{
    public static AddManager Instance;
    
    [SerializeField] private AddAggregator currentAddAggregator;
    public AddAggregator AddAggregator  => currentAddAggregator;
    public event Action OnAddOpen;
    public event Action OnAddClose;
    public event Action OnRewarded;
    
    public bool canShowAdd;
    public float lastInterstitialAddTime { get; private set; }
    public float lastRewardAddTime { get; private set; }
    public bool isMobile { get; private set; }
    
    private const int CRAZY_GAMES_MIDGAME_COUNTDOWN = 185;
    
    private const int YANDEX_GAMES_MIDGAME_COUNTDOWN = 65;
    
    private float _gameTime;

    private Interstitial _interstitial;
    private Rewarded _rewarded;

    private YandexAggregator _yandexAggregator;
    private float addTimer;
    
    public override void Initialize()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;


        SetDefaults();
#if UNITY_EDITOR
        currentAddAggregator = AddAggregator.None;
#endif
#if PLATFORM_ANDROID
        isMobile = true;
#endif
#if PLATFORM_STANDALONE_WIN
        isMobile = false;
#endif
        
        if (currentAddAggregator == AddAggregator.UnityAds)
        {
            GetComponentInChildren<UnityAds>().InitializeAds();
            _interstitial = GetComponent<Interstitial>();
            _rewarded = GetComponent<Rewarded>();
        }
        if (currentAddAggregator == AddAggregator.CrazyGames)
        {
            CrazySDK.ResetDomain();
            CrazyAds.ResetDomain();
            CrazyEvents.ResetDomain();
            CrazyUser.ResetDomain();
            
            CrazySDK.Instance.InitSDK();
            CrazySDK.Instance.GetSystemInfo(systemInfo =>
            { 
                // possible values: "desktop", "tablet", "mobile"
                switch (systemInfo.device.type)
                {
                    case "desktop" :
                        isMobile = false;
                        break;
                    case "tablet" :
                        isMobile = true;
                        break;
                    case "mobile" :
                        isMobile = true;
                        break;
                }
                
                Debug.Log(systemInfo.device.type);
            });
        }
        if (currentAddAggregator == AddAggregator.YandexGames)
        {
            _yandexAggregator = GetComponentInChildren<YandexAggregator>();
            _yandexAggregator.Initialize();
            isMobile = _yandexAggregator.IsMobile();
        }
    }

    private void SetDefaults()
    {
        _gameTime = 0f;
        lastInterstitialAddTime = 0f;
        lastRewardAddTime = -60;
        isMobile = false;
    }
    
    public void ChangeIsMobile(bool value)
    {
        isMobile = value;
    }
    private void Update()
    {
        _gameTime += Time.deltaTime;
        canShowAdd = _gameTime >= CanShowAdd();
        /*
        if (canShowAdd && LevelStateMachine.Instance.IsPlayState())
        {
            addTimer -= Time.deltaTime;
            if (addTimer <= 0f)
            {
                LevelStateMachine.Instance.Pause();
                ShowInterstitialAdd();
            }
            UiMonoStateMachine.Instance.ShowAddPopup(addTimer);
        }
        else
        {
            UiMonoStateMachine.Instance.DisableAddPopup();
            addTimer = 3f;
        }
        */
    }
    private float CanShowAdd()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.CrazyGames:
                return lastInterstitialAddTime + CRAZY_GAMES_MIDGAME_COUNTDOWN;
            case AddAggregator.YandexGames :
                return lastInterstitialAddTime + YANDEX_GAMES_MIDGAME_COUNTDOWN;
            case AddAggregator.UnityAds :
                return lastInterstitialAddTime + 61;
            default:
                return lastInterstitialAddTime + 61;
        }
    }

    public void ShowInterstitialAdd()
    {
        if (canShowAdd)
        {
            lastInterstitialAddTime = _gameTime;
            SoundMonoMechanic.Instance.DisableSound();
            OnAddOpen?.Invoke();
            switch (currentAddAggregator)
            {
                case AddAggregator.CrazyGames:
                    CrazyAds.Instance.beginAdBreak(InterstitialAddClosed, InterstitialAddClosedWithError);
                    break;
                case AddAggregator.YandexGames:
                    _yandexAggregator.ShowInterstitial();
                    break;
                case AddAggregator.UnityAds:
                    _interstitial.Initialize();
                    break;
                case AddAggregator.None:
                    InterstitialAddClosed();
                    break;
            }
        }
    }

    public void GameReady()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.TrowGameReadyEvent();
                break;
        }
    }

    public void ShowRewardAdd()
    {
        lastRewardAddTime = _gameTime;
        OnAddOpen?.Invoke();
        SoundMonoMechanic.Instance.DisableSound();
        switch (currentAddAggregator)
        {
            case AddAggregator.CrazyGames:
                CrazyAds.Instance.beginAdBreakRewarded(RewardAddClosed, RewardAddClosedWithError);
                break;
            case AddAggregator.YandexGames:
                _yandexAggregator.ShowRewarded();
                break;
            case AddAggregator.UnityAds:
                _rewarded.Initialize();
                break;
            case AddAggregator.None:
                RewardAddClosed();
                break;
        }
    }

    public void InterstitialAddClosed()
    {
        Debug.Log("Interstitial Add closed");
        OnAddClose?.Invoke();
    }
    
    public void InterstitialAddClosedWithError()
    {
        Debug.LogError("Interstitial Add closed with error");
        OnAddClose?.Invoke();
    }
    public void RewardAddClosed()
    {
        Debug.Log("Add reward closed");
        OnRewarded?.Invoke();
        OnAddClose?.Invoke();
    }
    
    public void RewardAddClosedWithError()
    {
        Debug.Log("Add reward closed with error");
        OnAddClose?.Invoke();
    }
    public void SaveDataToExternStorage(GameSaves gameSaves)
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.SaveDataToStorage(gameSaves);
                break;
        }
    }
    public void LoadFromExternStorage()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.LoadDataFromStorage();
                break;
        }
    }
    public string GetCurrentLanguage()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                return _yandexAggregator.GetLanguage();
            case AddAggregator.CrazyGames :
                string lang = "en";
                CrazySDK.Instance.GetSystemInfo(systemInfo =>
                {
                
                    if (systemInfo.countryCode == "US")
                    {
                        lang = "en";
                    }
                
                    if (systemInfo.countryCode == "TR")
                    {
                        lang = "tr";
                    }
                
                    if (systemInfo.countryCode == "RU")
                    {
                        lang = "ru";
                    }
                });
                return lang;
            default: return "en";
        }
        
    }
    public void Login()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.Login();
                break;
        }
    }

    public void TryToBuyItem(string itemName)
    {
        if (Inventory.Instance.HasItem(itemName))
        {
            Debug.Log($"There already has item {itemName}");
            return;
        }
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.TryToBuyItem(itemName);
                break;
            case AddAggregator.None:
                Inventory.Instance.AddItem(itemName);
                break;
        }
        
    }

    public void CheckForPurchases()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.YandexGames:
                _yandexAggregator.CheckForPurchases();
                break;
        }
    }

    public bool CanShowRewardAdd()
    {
        return lastRewardAddTime + 60 <= _gameTime;
    }
}
