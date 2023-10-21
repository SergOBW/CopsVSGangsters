using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Yandex.Plugins.Login;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Button onSoundButton;
    [SerializeField] private Button offSoundButton;

    [SerializeField] private Image soundImage;

    [SerializeField] private Sprite onSound;
    [SerializeField] private Sprite offSound;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button changeGraphicsButton;

    [SerializeField] private Button debugButton;

    public void Show()
    {
        gameObject.SetActive(true);
        /*
        if (SoundMonoMechanic.Instance.IsSoundOn())
        {
            offSoundButton.gameObject.SetActive(true);
            onSoundButton.gameObject.SetActive(false);
            soundImage.sprite = onSound;
        }
        else
        {
            offSoundButton.gameObject.SetActive(false);
            onSoundButton.gameObject.SetActive(true);
            soundImage.sprite = offSound;
        }
        */
        closeButton.onClick.AddListener(ClosePopup);
        onSoundButton.onClick.AddListener(OnSound);
        offSoundButton.onClick.AddListener(OffSound);
        changeGraphicsButton.onClick.AddListener(OnChangeGraphics);
        debugButton.onClick.AddListener(OnChangeDebug);
    }
    
    public void ClosePopup()
    {
        closeButton.onClick.RemoveListener(ClosePopup);
        onSoundButton.onClick.RemoveListener(OnSound);
        offSoundButton.onClick.RemoveListener(OffSound);
        changeGraphicsButton.onClick.RemoveListener(OnChangeGraphics);
        debugButton.onClick.RemoveListener(OnChangeDebug);
        gameObject.SetActive(false);
        SaveGameMechanic.Instance.Save();
    }

    private void OnChangeDebug()
    {
        GlobalSettings.Instance.isUsingDebug = !GlobalSettings.Instance.isUsingDebug;
        GlobalSettings.Instance.ChangeSettings();
    }

    private void OnChangeGraphics()
    {
        if (GlobalSettings.Instance.graphicsQuality == GraphicsQuality.Low)
        {
            GlobalSettings.Instance.graphicsQuality = GraphicsQuality.High;
            GlobalSettings.Instance.ChangeSettings();
            return;
        }

        if (GlobalSettings.Instance.graphicsQuality == GraphicsQuality.High)
        {
            GlobalSettings.Instance.graphicsQuality = GraphicsQuality.Low;
            GlobalSettings.Instance.ChangeSettings();
        }
    }

    private void OnSound()
    {
        SoundMonoMechanic.Instance.OnSound();
        onSoundButton.gameObject.SetActive(false);
        offSoundButton.gameObject.SetActive(true);
        soundImage.sprite = onSound;
    }

    private void OffSound()
    {
        SoundMonoMechanic.Instance.OffSound();
        onSoundButton.gameObject.SetActive(true);
        offSoundButton.gameObject.SetActive(false);
        soundImage.sprite = offSound;
    }
}
