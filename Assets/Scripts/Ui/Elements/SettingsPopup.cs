using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [SerializeField] private UiSwitcher debugSwitcher;
    [SerializeField] private UiSwitcher soundSwitcher;

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;


    public void Show()
    {
        gameObject.SetActive(true);

        closeButton.onClick.AddListener(ClosePopup);

        debugSwitcher.Initialize(false);
        soundSwitcher.Initialize(true);

        soundSwitcher.OnSwitcherStatusChanged += OnSoundChanged;
        debugSwitcher.OnSwitcherStatusChanged += OnChangeDebug;
        
        soundSlider.maxValue = 5f;
        soundSlider.minValue = -3f;

        soundSlider.value = GlobalSettings.Instance.soundValue;

        sensitivitySlider.maxValue = 5f;
        sensitivitySlider.minValue = 0f;
        
        sensitivitySlider.value = GlobalSettings.Instance.sensitivity;
        
        sensitivitySlider.onValueChanged.AddListener(ChangeSens);
        soundSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeSens(float value)
    {
        GlobalSettings.Instance.ChangeSensitivity(value);
    }

    private void ChangeVolume(float value)
    {
        GlobalSettings.Instance.ChangeVolume(value);
    }

    private void ClosePopup()
    {
        closeButton.onClick.RemoveListener(ClosePopup);
        
        debugSwitcher.Deinitialize();
        soundSwitcher.Deinitialize();

        soundSwitcher.OnSwitcherStatusChanged -= OnSoundChanged;
        debugSwitcher.OnSwitcherStatusChanged -= OnChangeDebug;
        
        gameObject.SetActive(false);
        
        sensitivitySlider.onValueChanged.RemoveListener(ChangeSens);
        soundSlider.onValueChanged.RemoveListener(ChangeVolume);
        SaveGameMechanic.Instance.Save();
    }

    private void OnChangeDebug(bool value)
    {
        GlobalSettings.Instance.isUsingDebug = value;
        GlobalSettings.Instance.ChangeSettings();
    }

    private void OnSoundChanged(bool value)
    {
        if (value)
        {
            SoundMonoMechanic.Instance.OnSound();
        }
        else
        {
            SoundMonoMechanic.Instance.OffSound();
        }
    }
}
