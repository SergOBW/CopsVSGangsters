
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageText : MonoBehaviour
{
    private TMP_Text text;
    private Text _text;
    private Language currentLanguage;
    [SerializeField] private string ruText;
    [SerializeField] private string engText;
    [SerializeField] private string trText;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        _text = GetComponent<Text>();
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += OnLanguageChanged;
        }
    }

    private void Start()
    {
        if (LanguageManager.Instance != null)
        {
            ChangeLanguage(LanguageManager.Instance.GetLanguage());
        }
        else
        {
            ChangeLanguage(Language.en);
        }
    }

    private void OnLanguageChanged(Language obj)
    {
        ChangeLanguage(obj);
    }

    private void ChangeLanguage(Language language)
    {
        currentLanguage = language;

        if (text != null)
        {
            switch (language)
            {
                case Language.en:
                    text.text = engText;
                    break;
                case Language.ru:
                    text.text = ruText;
                    break;
                case Language.tr:
                    text.text = trText;
                    break;
                default: 
                    text.text = engText;
                    break;
            }
        }

        if (_text != null)
        {
            switch (language)
            {
                case Language.en:
                    _text.text = engText;
                    break;
                case Language.ru:
                    _text.text = ruText;
                    break;
                case Language.tr:
                    _text.text = trText;
                    break;
                default: 
                    _text.text = engText;
                    break;
            }
        }
        
    }
}
