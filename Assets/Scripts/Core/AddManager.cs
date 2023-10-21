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
    public event Action OnRewardedAddClosed;
    public event Action OnAddOpen;
    public event Action OnAddClose;
    
    private const int CRAZY_GAMES_MIDGAME_COUNTDOWN = 185;
    private const int YANDEX_GAMES_MIDGAME_COUNTDOWN = 65;
    private const int YANDEX_GAMES_ADDSHOW_COUNTDOWN = 90;
    
    public float GameTime { get; private set; }
    public bool canShowAdd;
    public float lastAddtime;
    private bool canShowMidgameAdd;

    private bool isUnityEditor;
    public bool isMobile { get; private set; }

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
        
        
        GameTime = 0f;
        lastAddtime = 0f;
        canShowMidgameAdd = true;
        isMobile = false;
#if UNITY_EDITOR
        currentAddAggregator = AddAggregator.None;
#endif

        if (currentAddAggregator == AddAggregator.UnityAds)
        {
            GetComponentInChildren<UnityAds>().InitializeAds();
            _interstitial = GetComponent<Interstitial>();
            _rewarded = GetComponent<Rewarded>();
#if PLATFORM_ANDROID
            isMobile = true;
#endif
#if PLATFORM_STANDALONE_WIN
            isMobile = false;
#endif
        }
        if (isUnityEditor)
        {
            return;
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
    
    public void ChangeIsMobile(bool value)
    {
        isMobile = value;
    }
    
    
    private void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime >= CanShowAdd())
        {
            canShowAdd = true;
        }
        else
        {
            canShowAdd = false;
        }

        if (GameTime >= GetNextAddTime() && canShowMidgameAdd && canShowAdd)
        {
            canShowMidgameAdd = false;
        }
    }

    private float GetNextAddTime()
    {
        switch (currentAddAggregator)
        {
            case AddAggregator.CrazyGames:
                return lastAddtime + CRAZY_GAMES_MIDGAME_COUNTDOWN;
            case AddAggregator.YandexGames :
                return lastAddtime + YANDEX_GAMES_ADDSHOW_COUNTDOWN;
            default:
                return 0f;
        }
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
            lastAddtime = GameTime;
            if (!isUnityEditor)
            {
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
        OnAddOpen?.Invoke();

        if (isUnityEditor && AddAggregator != AddAggregator.UnityAds)
        {
            InterstitialAddClosed();
        }

        if (AddAggregator == AddAggregator.UnityAds)
        {
            _interstitial.Initialize();
        }
    }

    public void ShowRewardAdd()
    {
        if (!isUnityEditor)
        {
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
        OnAddOpen?.Invoke();

        if (isUnityEditor && AddAggregator != AddAggregator.UnityAds)
        {
            RewardAddClosed();
        }

        if (AddAggregator == AddAggregator.UnityAds)
        {
            _rewarded.Initialize();
        }
    }

    public void InterstitialAddClosed()
    {
        Debug.Log("Interstitial Add closed");
        canShowMidgameAdd = true;
        OnAddClose?.Invoke();
    }
    
    public void InterstitialAddClosedWithError()
    {
        Debug.LogError("Interstitial Add error");
        canShowMidgameAdd = true;
        OnAddClose?.Invoke();
    }
    public void RewardAddClosed()
    {
        Debug.Log("Add reward closed");
        OnRewardedAddClosed?.Invoke();
        OnAddClose?.Invoke();
    }
    
    public void RewardAddClosedWithError()
    {
        Debug.Log("Add reward closed with error");
        OnRewardedAddClosed?.Invoke();
        OnAddClose?.Invoke();
    }

    public void TryToBuy(int id)
    {
        Debug.Log(id.ToString());
        OnAddOpen?.Invoke();


        if (isUnityEditor)
        {
            PurchaseSuccess(id.ToString());
        }
        
    }

    public void PurchaseSuccess(string id)
    {
        int.TryParse(id, out int parseResualt);
        OnAddClose?.Invoke();
    }
    
    public void PurchaseClose(string id)
    {
        Debug.Log("The purchase with id " + id + " failed!");
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
