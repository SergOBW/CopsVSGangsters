using System;
using Abstract;
using CrazyGames;
using Save;
using UnityEngine;

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
    public float lastAddtime;
    public bool isMobile { get; private set; }
    
    private const int CRAZY_GAMES_MIDGAME_COUNTDOWN = 185;
    
    private const int YANDEX_GAMES_MIDGAME_COUNTDOWN = 65;
    
    private float _gameTime;

    private Interstitial _interstitial;
    private Rewarded _rewarded;

    private YandexAggregator _yandexAggregator;
    
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
        lastAddtime = 0f;
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
    }
    private float CanShowAdd()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.CrazyGames:
                return lastAddtime + CRAZY_GAMES_MIDGAME_COUNTDOWN;
            case AddAggregator.YandexGames :
                return lastAddtime + YANDEX_GAMES_MIDGAME_COUNTDOWN;
            case AddAggregator.UnityAds :
                return lastAddtime + 61;
            default:
                return 0f;
        }
    }

    public void ShowInterstitialAdd()
    {
        if (canShowAdd)
        {
            lastAddtime = _gameTime;
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

    public void ShowRewardAdd()
    {
        OnAddOpen?.Invoke();
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
}
