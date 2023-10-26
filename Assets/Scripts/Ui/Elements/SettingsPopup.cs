using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [SerializeField] private UiSwitcher debugSwitcher;
    [SerializeField] private UiSwitcher soundSwitcher;
    

    public void Show()
    {
        gameObject.SetActive(true);
        
        closeButton.onClick.AddListener(ClosePopup);
        
        debugSwitcher.Initialize(false);
        soundSwitcher.Initialize(true);

        soundSwitcher.OnSwitcherStatusChanged += OnSoundChanged;
        debugSwitcher.OnSwitcherStatusChanged += OnChangeDebug;
    }

    private void ClosePopup()
    {
        closeButton.onClick.RemoveListener(ClosePopup);
        
        debugSwitcher.Deinitialize();
        soundSwitcher.Deinitialize();

        soundSwitcher.OnSwitcherStatusChanged -= OnSoundChanged;
        debugSwitcher.OnSwitcherStatusChanged -= OnChangeDebug;
        
        gameObject.SetActive(false);
        
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
