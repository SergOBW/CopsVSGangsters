using System;
using Abstract;
using UnityEngine;

public enum Language
{
    en = 0,
    ru = 1,
    tr = 2
}

public class LanguageManager : IMechanic
{
    public static LanguageManager Instance;
    private Language language;
    public event Action<Language> OnLanguageChanged;
    
    private bool _isUnityEditor;
    private string _startedLanguage = "ru";
    
    public void Initialize()
    {
        Instance = this;
#if UNITY_EDITOR
        _isUnityEditor = true;
#endif
        string lang = "ru";
        if (_isUnityEditor)
        {
            ChangeLanguage(_startedLanguage);
            return;
        }
        if (AddManager.Instance != null)
        {
            lang = AddManager.Instance.GetCurrentLanguage();
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian :
                    lang = "ru";
                    break;
                case SystemLanguage.English :
                    lang = "en";
                    break;
                case SystemLanguage.Turkish :
                    lang = "tr";
                    break;
                default :
                    lang = "en";
                    break;
            }
        }
        
        ChangeLanguage(lang);
    }
    
    public void ChangeLanguage(string languageString)
    {
        switch (languageString)
        {
            case "ru" :
                language = Language.ru;
                break;
            case "en" :
                language = Language.en;
                break;
            case "tr" :
                language = Language.tr;
                break;
            default: language = Language.en;
                break;
        }
        
        OnLanguageChanged?.Invoke(language);
    }

    public Language GetLanguage()
    {
        return language;
    }
}
