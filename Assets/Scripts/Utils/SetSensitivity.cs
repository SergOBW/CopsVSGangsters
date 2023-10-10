using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yandex.Plugins.Login;

public class SetSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        slider.onValueChanged.AddListener(SaveSens);
        slider.value = SaveGameMechanic.Instance.GetPlayerSensitivity();
#if PLATFORM_ANDROID
        slider.maxValue = 10;
        slider.value = 5;
        SaveSens(slider.value);
#endif
        if (AddManager.Instance.isMobile)
        {
            slider.maxValue = 10;
            slider.value = 5;
            SaveSens(slider.value);
        }
    }

    private void SaveSens(float value)
    {
        SaveGameMechanic.Instance.SaveSensitivity(value);
        string s = String.Format("{0:0.00}", value);
        _text.text = s;
    }
}
