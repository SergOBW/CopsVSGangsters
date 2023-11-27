using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Abstract.Inventory;
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
    private static extern void GetPurchases();
    
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
        }
    }

    public void TrowGameReadyEvent()
    {
        if (_isSDKInitialized)
        {
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
    
    public void PurchaseSuccess(string id)
    {
        Debug.Log("Purchase success id = " + id);
        switch (id)
        {
            case "0":
                Inventory.Instance.AddItem("Money Pack");
                break;
            case "1":
                Inventory.Instance.AddItem("Big drill");
                break;
            case "2":
                Inventory.Instance.AddItem("Body Armour");
                break;
            case "3":
                Inventory.Instance.AddItem("Great Bag");
                break;
            case "4":
                Inventory.Instance.AddItem("Great Bomb");
                break;
            case "5":
                Inventory.Instance.AddItem("Hacker");
                break;
            case "6":
                Inventory.Instance.AddItem("Tactical Gloves");
                break;
        }
    }

    public void PurchaseError(string id)
    {
        Debug.Log("There some error with buying " + id);
    }

    public void SetLoginType(string type)
    {
        LoginManager.Instance.SetLoginType(type);
    }

    public void TryToBuyItem(string itemName)
    {
        Debug.Log($"Item name = {itemName}");
        switch (itemName)
        {
            case "Money Pack":
                BuyItem("0");
                break;
            case "Big drill":
                BuyItem("1");
                break;
            case "Body Armour":
                BuyItem("2");
                break;
            case "Great Bag":
                BuyItem("3");
                break;
            case "Great Bomb":
                BuyItem("4");
                break;
            case "Hacker":
                BuyItem("5");
                break;
            case "Tactical Gloves":
                BuyItem("6");
                break;
        }
    }
    
    public class JsonObject
    {
        public string productID  { get; set; }
        public string purchaseToken { get; set; }
        public string developerPayload { get; set; }
    }
    
    public void SetPurchases(string purchases)
    {
        List<JsonObject> json = new List<JsonObject>();
        json = JsonConvert.DeserializeObject<List<JsonObject>>(purchases);
        Debug.Log("Purchases : " + json);
        if (json.Count > 0)
        {
            foreach (var jsonObject in json)
            {
                PurchaseSuccess(jsonObject.productID);
            }
        }
        else
        {
            Debug.Log("There is no more purchases");
        }
    }
    public void CheckForPurchases()
    {
        if (LoginManager.Instance.isLogin && _isSDKInitialized)
        {
            GetPurchases();
        }
        else
        {
            Debug.LogError("You try to load purchases without login or sdk initialized");
        }
    }
}
