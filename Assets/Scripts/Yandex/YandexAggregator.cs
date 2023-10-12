using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Save;
using UnityEngine;
using Yandex.Plugins.Login;

public class YandexAggregator : MonoBehaviour
{
    
    #region ExternalMethods

    [DllImport("__Internal")]
    private static extern void ShowAdv();
    
    [DllImport("__Internal")]
    private static extern void ShowAdvReward();
    
    [DllImport("__Internal")]
    private static extern void BuyItem(string id);
    [DllImport("__Internal")]
    private static extern string GetDeviceTypeFromYandex();
    [DllImport("__Internal")]
    private static extern bool IsSDKInitialized();
    [DllImport("__Internal")]
    private static extern string GetLang();
    
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);

    [DllImport("__Internal")]
    private static extern void LoadExtern();
    
    [DllImport("__Internal")]
    private static extern void CheckForPayments(string id);
    
    [DllImport("__Internal")]

    private static extern void TryToAuth();

    [DllImport("__Internal")]
    private static extern void TryToLogin();
    [DllImport("__Internal")]
    private static extern void GameReady();

    #endregion

    private bool _isSDKInitialized;
    private float _timer;

    public void Initialize()
    {
        _isSDKInitialized = false;
        InvokeRepeating("CheckForSdkInitialize",0,0.5f);
    }

    public void ShowInterstitial()
    {
        if (_isSDKInitialized)
        {
            ShowAdv();
        }
        else
        {
            AddManager.Instance.InterstitialAddClosedWithError();
        }
    }

    public void ShowRewarded()
    {
        if (_isSDKInitialized)
        {
            ShowAdvReward();
        }
        else
        {
            AddManager.Instance.RewardAddClosedWithError();
        }
    }

    private void CheckForSdkInitialize()
    {
        _isSDKInitialized = IsSDKInitialized();
        //Debug.Log("IS SDK INITIALIZED = " + _isSDKInitialized);
        if (_isSDKInitialized)
        {
            CancelInvoke("CheckForSdkInitialize");
            AddManager.Instance.ChangeIsMobile(IsMobile());
            LanguageManager.Instance.ChangeLanguage(GetLanguage());
            TryToLogin();
            GameReady();
        }
    }

    public bool IsMobile()
    {
        bool isMobile = false;
        
        if (_isSDKInitialized)
        {
            string deviceType = GetDeviceTypeFromYandex();
            switch (deviceType)
            {
                case "desktop" :
                    isMobile = false;
                    break;
                case "mobile" :
                    isMobile =  true;
                    break;
                case "tablet" :
                    isMobile = true;
                    break;
                case "tv" :
                    isMobile = false;
                    break;
                case "error" :
                    //Debug.Log("Error with YASDK");
                    return false;
                default:
                    return false;
            }
        }
        
        
        //Debug.Log(" IS MOBILE = " + isMobile);
        return isMobile;
    }

    public string GetLanguage()
    {
        string lang = "en";
        
        if (_isSDKInitialized)
        {
            lang = GetLang();
        }
        
        return lang;
    }

    public void LoadDataFromStorage()
    {
        //Debug.Log("TRY TO LOAD FROM YANDEX");
        if (!_isSDKInitialized)
        {
            //Debug.Log("NAH, CANT TO LOAD CZ SDK IS NOT INITIALIZED");
            return;
        }
        if (LoginManager.Instance.isLogin)
        {
            //Debug.Log("LOADING FROM YANDEX...");
            LoadExtern();
        }
        else
        {
            //Debug.Log("NAH, CANT TO LOAD CZ PLAYER IS NOT LOGIN");
        }
    }

    public void SaveDataToStorage(GameSaves gameSaves)
    {
        //Debug.Log("TRY TO SAVE TO YANDEX");
        if (!_isSDKInitialized)
        {
            //Debug.Log("NAH, CANT TO SAVE CZ SDK IS NOT INITIALIZED");
            return;
        }
        if (LoginManager.Instance.isLogin)
        {
            string jsonString = JsonConvert.SerializeObject(gameSaves);
            //Debug.Log("SAVING TO YANDEX..." + jsonString);
            SaveExtern(jsonString);
        }
        else
        {
            //Debug.Log("NAH, CANT TO SAVE CZ PLAYER IS NOT LOGIN");
        }
    }

    public void Login()
    {
        if (!_isSDKInitialized)
        {
            //Debug.Log("NAH, CANT TO TRY LOGIN CZ SDK IS NOT INITIALIZED");
            return;
        }
        TryToAuth();
    }

    public void SDKInitialized()
    {
        if (!_isSDKInitialized)
        {
            _isSDKInitialized = true;
            CancelInvoke("CheckForSdkInitialize");
            AddManager.Instance.ChangeIsMobile(IsMobile());
            LanguageManager.Instance.ChangeLanguage(GetLanguage());
            TryToLogin();
            //Debug.Log("Direct initialization");
        }
    }

    public void SendPlayerSaves(string json)
    {
        SaveGameMechanic.Instance.SetPlayerInfo(json);
    }
    
    public void SetName(string name)
    {
        LoginManager.Instance.SetName(name);
    }

    public void SetAvatar(string avatar)
    {
        LoginManager.Instance.SetAvatar(avatar);
    }

    public void LoginFirstTime()
    {
        LoginManager.Instance.LoginFirstTime();
    }

    public void SetLoginType(string type)
    {
        LoginManager.Instance.SetLoginType(type);
    }
}
