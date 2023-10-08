using System;
using UnityEngine;

public enum Language
{
    en = 0,
    ru = 1,
    tr = 2
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    private Language language;
    public event Action<Language> OnLanguageChanged;
    
    private bool _isUnityEditor;
    [SerializeField] private string startedLanguage = "en";
    

    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        _isUnityEditor = true;
#endif
    }

    private void Start()
    {
        
        string lang = "en";
        if (_isUnityEditor)
        {
            ChangeLanguage(startedLanguage);
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
